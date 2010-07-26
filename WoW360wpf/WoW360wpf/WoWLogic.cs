using System;
using GameControllerLogic;
using SettingsLogic;
using MethodExtensions.Enum;

namespace WoW360wpf
{
    /// <summary>
    /// The logic between the controller and the game
    /// </summary>
    public class WoWLogic
    {
        #region Enums
        [Flags]
        enum Movements : int
        {
            None = 0,
            LeftStrafe = 1,
            Forward = 2,
            RightStrafe = 4,
            Left = 8,
            Backward = 16,
            Right = 32,
            Up = 64,
            Down = 128,
            All = LeftStrafe | Forward | RightStrafe | Left | Backward | Right | Up | Down
        }
        #endregion Enums

        GameController.ThumbSticks _moveStick = GameController.ThumbSticks.Left;
        GameController.ThumbSticks _lookStick = GameController.ThumbSticks.Right;
        double _mouseLookSensitivity = 1.0;
        double _cursorMoveSensitivity = 1.0;
        double _cursorCenterOffsetX = 0.0;
        double _cursorCenterOffsetY = 0.0;
        float _cursorPositionX = 0.0f;
        float _cursorPositionY = 0.0f;
        SettingsLogic.Settings _settings = null;
        bool _paused = false;
        bool _cursorMode = true;
        GameController.Triggers _triggersPressed = GameController.Triggers.None;
        Movements _moveFlag = Movements.None;

        public void AttachEvents(GameController gc)
        {
            gc.ButtonStateChanged += new GameController.ButtonStateChangedHandler(gc_ButtonStateChanged);
            gc.ThumbStickStateUpdated += new GameController.ThumbStickStateUpdatedHandler(gc_ThumbStickStateUpdated);
            gc.TriggerStateUpdated += new GameController.TriggerStateUpdatedHandler(gc_TriggerStateUpdated);
            gc.ControllerDisconnected += new GameController.ControllerDisconnectedHandler(gc_ControllerDisconnected);
            gc.ControllerConnected += new GameController.ControllerConnectedHandler(gc_ControllerConnected);
        }

        public void InitializeSettings(SettingsLogic.Settings settings)
        {
            try
            {
                _settings = settings;
                // It's possible the settings file doesn't include the options or there is a
                // conversion exception
                _moveStick = _settings.Options.GetOption(OptionNames.MoveStick).GetValueAsThumbSticks();
                _lookStick = _settings.Options.GetOption(OptionNames.LookStick).GetValueAsThumbSticks();
                _mouseLookSensitivity = _settings.Options.GetOption(OptionNames.MouseLookSensitivity).GetValueAsDouble();
                _cursorMoveSensitivity = _settings.Options.GetOption(OptionNames.CursorMoveSensitivity).GetValueAsDouble();
                _cursorCenterOffsetX = _settings.Options.GetOption(OptionNames.CursorCenterOffsetX).GetValueAsDouble();
                _cursorCenterOffsetY = _settings.Options.GetOption(OptionNames.CursorCenterOffsetY).GetValueAsDouble();
            }
            finally { }
        }

        void MoveKeyPress(Movements btn, bool press)
        {
            // Handling each if separately allows sending All for key up when connection lost
            // For each key, check if it's requested and that the state isn't already set
            if (btn.Has(Movements.Backward) && _moveFlag.Has(Movements.Backward) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.S, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.Left) && _moveFlag.Has(Movements.Left) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.A, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.Right) && _moveFlag.Has(Movements.Right) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.D, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.LeftStrafe) && _moveFlag.Has(Movements.LeftStrafe) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.Q, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.RightStrafe) && _moveFlag.Has(Movements.RightStrafe) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.E, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.Forward) && _moveFlag.Has(Movements.Forward) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.W, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.Up) && _moveFlag.Has(Movements.Up) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.Space, System.Windows.Input.ModifierKeys.None, press);
            }
            if (btn.Has(Movements.Down) && _moveFlag.Has(Movements.Down) != press)
            {
                UnsafeWin32Calls.SendKeyClick(System.Windows.Input.Key.X, System.Windows.Input.ModifierKeys.None, press);
            }

            // Update the flags
            if (press)
            {
                _moveFlag.Add(btn);
            }
            else
            {
                _moveFlag.Remove(btn);
            }
        }

        protected void OnControllerConnected(GameController.ControllerNumber cNum)
        {
            _paused = false;
        }

        protected void OnControllerDisconnected(GameController.ControllerNumber cNum)
        {
            // release all movement buttons
            MoveKeyPress(Movements.All, false);
            _moveFlag = Movements.None;
            _triggersPressed = GameController.Triggers.None;
            _paused = true;
        }

        protected void OnTriggerStateUpdated(GameController.ControllerNumber cNum, GameController.Triggers trigger, float triggerValue)
        {
            if (!_paused)
            {
                if (triggerValue > 0.5f)
                {
                    _triggersPressed.Add(trigger);
                }
                else
                {
                    _triggersPressed.Remove(trigger);
                }
            }
        }

        protected void OnThumbSticksUpdated(GameController.ControllerNumber cNum, GameController.ThumbSticks thumbstick, float x, float y)
        {
            if (!_paused)
            {
                if (thumbstick == _moveStick)
                {
                    // Move the character
                    MoveKeyPress(Movements.RightStrafe, (x > 0.5f));
                    MoveKeyPress(Movements.LeftStrafe, (x < -0.5f));
                    MoveKeyPress(Movements.Forward, (y > 0.5f));
                    MoveKeyPress(Movements.Backward, (y < -0.5f));
                }
                else
                {
                    // Update the position of the mouse, don't send mouse moves until
                    // a click is made (in cursor mode), or switch back to look mode
                    _cursorPositionX += x;
                    _cursorPositionY += y;
                    if (_cursorMode)
                    {
                        UnsafeWin32Calls.SendMouseMove(_cursorPositionX, _cursorPositionY);
                    }
                }
            }
        }

        protected void OnButtonStateChanged(GameController.ControllerNumber cNum, GameController.Buttons button, bool pressed)
        {
            if (!_paused)
            {
                // Take into account special cases:
                // 1. Thumbsticks are mouse clicks in cursor mode
                if (_cursorMode && button == GameController.Buttons.LeftStick)
                {
                    UnsafeWin32Calls.SendMouseClick(_cursorPositionX, _cursorPositionY, System.Windows.Input.MouseButton.Left, pressed);
                    return;
                }
                if (_cursorMode && button == GameController.Buttons.RightStick)
                {
                    UnsafeWin32Calls.SendMouseClick(_cursorPositionX, _cursorPositionY, System.Windows.Input.MouseButton.Right, pressed);
                    return;
                }

                KeyBinding kb = _settings.KeyBindings.GetKeyBinding(_triggersPressed, button);

                // 2. Swap look/cursor mode
                if (pressed && kb.KeyBoardKey == System.Windows.Input.Key.Attn)
                {
                    _cursorMode = !_cursorMode;
                    return;
                }

                // Handle normal key clicks
                UnsafeWin32Calls.PostKeyClick(kb.KeyBoardKey, kb.Modifier, pressed);
            }
        }

        #region Events
        void gc_ControllerConnected(object sender, GameControllerEventArgs args)
        {
            OnControllerConnected(args.ControllerNum);
        }

        void gc_ControllerDisconnected(object sender, GameControllerEventArgs args)
        {
            OnControllerDisconnected(args.ControllerNum);
        }

        void gc_TriggerStateUpdated(object sender, TriggerStateEventArgs args)
        {
            OnTriggerStateUpdated(args.ControllerNum, args.Trigger, args.TriggerValue);
        }

        void gc_ThumbStickStateUpdated(object sender, ThumbStickStateEventArgs args)
        {
            OnThumbSticksUpdated(args.ControllerNum, args.ThumbStick, args.XValue, args.YValue);
        }

        void gc_ButtonStateChanged(object sender, ButtonStateEventArgs args)
        {
            OnButtonStateChanged(args.ControllerNum, args.Button, args.Pressed);
        }
        #endregion Events
    }
}
