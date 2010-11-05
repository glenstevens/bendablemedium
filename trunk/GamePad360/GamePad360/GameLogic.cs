using System;
using GamePadUtilities;
//using SettingsLogic;
using KernelUtilities;
using System.Windows.Forms;
using MouseButtons=KernelUtilities.MouseButtons;

namespace GamePad360
{
    /// <summary>
    /// The logic between the controller and the game
    /// </summary>
    public class GameLogic
    {
        #region Enums
        [Flags]
        enum Movements
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

        GamePadThumbSticks _moveStick = GamePadThumbSticks.Left;
        float _mouseLookSensitivity = 15.0f;
        float _cursorMoveSensitivity = 15.0f;
        //double _cursorCenterOffsetX = 0.0;
        //double _cursorCenterOffsetY = 0.0;
        bool _paused;
        bool _cursorMode = true;
        GamePadTriggers _triggersPressed = GamePadTriggers.None;
        Movements _moveFlag = Movements.None;

        public IntPtr WindowHandle { get; set; }

		public void AttachEvents(GamePads gc)
        {
			gc.ButtonStateChanged += gc_ButtonStateChanged;
			gc.ThumbStickStateUpdated += gc_ThumbStickStateUpdated;
			gc.TriggerStateUpdated += gc_TriggerStateUpdated;
			gc.ControllerDisconnected += gc_ControllerDisconnected;
			gc.ControllerConnected += gc_ControllerConnected;
        }

        void MoveKeyPress(Movements btn, bool press)
        {
            // Handling each if separately allows sending All for key up when connection lost
            // For each key, check if it's requested and that the state isn't already set
            if (EnumHas(btn, Movements.Backward) && EnumHas(_moveFlag, Movements.Backward) != press)
            {
                KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.S, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.Left) && EnumHas(_moveFlag, Movements.Left) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.A, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.Right) && EnumHas(_moveFlag, Movements.Right) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.D, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.LeftStrafe) && EnumHas(_moveFlag, Movements.LeftStrafe) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.Q, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.RightStrafe) && EnumHas(_moveFlag, Movements.RightStrafe) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.E, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.Forward) && EnumHas(_moveFlag, Movements.Forward) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.W, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.Up) && EnumHas(_moveFlag, Movements.Up) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.Space, ModifierKeys.None, press);
            }
            if (EnumHas(btn, Movements.Down) && EnumHas(_moveFlag, Movements.Down) != press)
            {
				KeyboardUtilities.SendKeyClick(WindowHandle, KeyboardKeys.X, ModifierKeys.None, press);
            }

            // Update the flags
            if (press)
            {
				_moveFlag = EnumAdd(_moveFlag, btn);
            }
            else
            {
				_moveFlag = EnumRemove(_moveFlag, btn);
            }
        }

		protected void OnControllerConnected(GamePadNumber cNum)
        {
            _paused = false;
        }

        protected void OnControllerDisconnected(GamePadNumber cNum)
        {
            // release all movement buttons
            MoveKeyPress(Movements.All, false);
            _moveFlag = Movements.None;
            _triggersPressed = GamePadTriggers.None;
            _paused = true;
        }

        protected void OnTriggerStateUpdated(GamePadNumber cNum, GamePadTriggers trigger, float triggerValue)
        {
        	if (_paused) return;

        	if (triggerValue > 0.5f)
        	{
				_triggersPressed = EnumAdd(_triggersPressed, trigger);
        	}
        	else
        	{
				_triggersPressed = EnumRemove(_triggersPressed, trigger);
        	}
        }

    	protected void OnThumbSticksUpdated(GamePadNumber cNum, GamePadThumbSticks thumbstick, float x, float y)
        {
        	if (_paused) return;

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
        		// Check deadzone
        		if (Math.Abs(x) < 0.1) x = 0;
        		if (Math.Abs(y) < 0.1) y = 0;

        		// Update the position of the mouse, don't send mouse moves until
        		// a click is made (in cursor mode), or switch back to look mode
        		System.Drawing.Point pos = new System.Drawing.Point();
                // Do floating point math separately to avoid rounding errors with the int math which causes creep
                float xChange = x * (_cursorMode ? _cursorMoveSensitivity : _mouseLookSensitivity);
                float yChange = y * (_cursorMode ? _cursorMoveSensitivity : _mouseLookSensitivity);
                pos.X = (int)(Cursor.Position.X + xChange);
                pos.Y = (int)(Cursor.Position.Y - yChange);

                Cursor.Position = pos;
                //if (_cursorMode)
                //{
                //    MouseUtilities.SendMouseMove(WindowHandle, Cursor.Position.X, Cursor.Position.Y);
                //}
        	}
        }

    	protected void OnButtonStateChanged(GamePadNumber cNum, GamePadButtons button, bool pressed)
        {
    		if (_paused) return;

    		// Take into account special cases:
    		// 1. Thumbsticks are mouse clicks in cursor mode
    		if (_cursorMode && button == GamePadButtons.LeftStick)
    		{
    			MouseUtilities.SendMouseClick(WindowHandle, Cursor.Position.X, Cursor.Position.Y, MouseButtons.Left, pressed);
    			return;
    		}
    		if (_cursorMode && button == GamePadButtons.RightStick)
    		{
    			MouseUtilities.SendMouseClick(WindowHandle, Cursor.Position.X, Cursor.Position.Y, MouseButtons.Right, pressed);
    			return;
    		}

    		KeyboardKeys key = KeyboardKeys.None;
    		ModifierKeys modifierKeys = ModifierKeys.None;

			if (!GetKeyBinding(_triggersPressed, button, ref key, ref modifierKeys)) return;

    		// 2. Swap look/cursor mode
			if (pressed && key == KeyboardKeys.VK_SELECT)
    		{
    			MouseUtilities.SendMouseClick(WindowHandle, Cursor.Position.X, Cursor.Position.Y, MouseButtons.Right, _cursorMode);
    			_cursorMode = !_cursorMode;
    			return;
    		}

    		// Handle normal key clicks
			KeyboardUtilities.PostKeyClick(WindowHandle, key, modifierKeys, pressed);
        }

        #region GetKeyBinding
		private bool GetKeyBinding(GamePadTriggers triggersPressed, GamePadButtons button, ref KeyboardKeys key, ref ModifierKeys modifierKeys)
        {
            switch (triggersPressed)
            {
                case GamePadTriggers.None:
                    switch (button)
                    {
                        case GamePadButtons.A:
                            key = KeyboardKeys.D1;
                    		return true;
                        case GamePadButtons.B:
                            key = KeyboardKeys.D2;
							return true;
						case GamePadButtons.X:
                            key = KeyboardKeys.D3;
							return true;
						case GamePadButtons.Y:
                            key = KeyboardKeys.D4;
							return true;
						case GamePadButtons.Back:
                            key = KeyboardKeys.Escape;
							return true;
						case GamePadButtons.Start:
                            key = KeyboardKeys.M;
							return true;
						case GamePadButtons.LeftShoulder:
                            key = KeyboardKeys.Tab;
							return true;
						case GamePadButtons.RightShoulder:
                            key = KeyboardKeys.VK_SELECT;
							return true;
						case GamePadButtons.LeftStick:
                            key = KeyboardKeys.Tilde;
							return true;
						case GamePadButtons.RightStick:
                            key = KeyboardKeys.Space;
							return true;
						case GamePadButtons.DPadUp:
                            key = KeyboardKeys.Shift;
							return true;
						case GamePadButtons.DPadLeft:
                            key = KeyboardKeys.Alt;
							return true;
						case GamePadButtons.DPadDown:
                            key = KeyboardKeys.Ctrl;
							return true;
						case GamePadButtons.DPadRight:
                            key = KeyboardKeys.B;
							return true;
					}
                    break;
                case GamePadTriggers.Left:
                    switch (button)
                    {
                        case GamePadButtons.A:
                            key = KeyboardKeys.D5;
							return true;
						case GamePadButtons.B:
                            key = KeyboardKeys.D6;
							return true;
						case GamePadButtons.X:
                            key = KeyboardKeys.D7;
							return true;
						case GamePadButtons.Y:
                            key = KeyboardKeys.D8;
							return true;
						case GamePadButtons.Back:
                            key = KeyboardKeys.Escape;
							return true;
						case GamePadButtons.Start:
                            key = KeyboardKeys.M;
							return true;
						case GamePadButtons.LeftShoulder:
                            key = KeyboardKeys.Tab;
							return true;
						case GamePadButtons.RightShoulder:
							key = KeyboardKeys.VK_SELECT;
							return true;
						case GamePadButtons.LeftStick:
							key = KeyboardKeys.Tilde;
							return true;
						case GamePadButtons.RightStick:
                            key = KeyboardKeys.Space;
							return true;
						case GamePadButtons.DPadUp:
							key = KeyboardKeys.Shift;
							return true;
						case GamePadButtons.DPadLeft:
							key = KeyboardKeys.Alt;
							return true;
						case GamePadButtons.DPadDown:
							key = KeyboardKeys.Ctrl;
							return true;
						case GamePadButtons.DPadRight:
                            key = KeyboardKeys.B;
							return true;
					}
                    break;
                case GamePadTriggers.Right:
                    switch (button)
                    {
                        case GamePadButtons.A:
                            key = KeyboardKeys.D9;
							return true;
						case GamePadButtons.B:
                            key = KeyboardKeys.D0;
							return true;
						case GamePadButtons.X:
                            key = KeyboardKeys.Minus;
							return true;
						case GamePadButtons.Y:
                            key = KeyboardKeys.Plus;
							return true;
						case GamePadButtons.Back:
                            key = KeyboardKeys.Escape;
							return true;
						case GamePadButtons.Start:
                            key = KeyboardKeys.M;
							return true;
						case GamePadButtons.LeftShoulder:
                            key = KeyboardKeys.Tab;
							return true;
						case GamePadButtons.RightShoulder:
							key = KeyboardKeys.VK_SELECT;
							return true;
						case GamePadButtons.LeftStick:
							key = KeyboardKeys.Tilde;
							return true;
						case GamePadButtons.RightStick:
                            key = KeyboardKeys.Space;
							return true;
						case GamePadButtons.DPadUp:
							key = KeyboardKeys.Shift;
							return true;
						case GamePadButtons.DPadLeft:
							key = KeyboardKeys.Alt;
							return true;
						case GamePadButtons.DPadDown:
							key = KeyboardKeys.Ctrl;
							return true;
						case GamePadButtons.DPadRight:
                            key = KeyboardKeys.B;
							return true;
					}
                    break;
                case GamePadTriggers.Both:
                    switch (button)
                    {
                        case GamePadButtons.A:
                            key = KeyboardKeys.F2;
							return true;
						case GamePadButtons.B:
                            key = KeyboardKeys.F3;
							return true;
						case GamePadButtons.X:
                            key = KeyboardKeys.F4;
							return true;
						case GamePadButtons.Y:
                            key = KeyboardKeys.F5;
							return true;
						case GamePadButtons.Back:
                            key = KeyboardKeys.Escape;
							return true;
						case GamePadButtons.Start:
                            key = KeyboardKeys.M;
							return true;
						case GamePadButtons.LeftShoulder:
                            key = KeyboardKeys.Tab;
                    		modifierKeys = ModifierKeys.Shift;
							return true;
						case GamePadButtons.RightShoulder:
							key = KeyboardKeys.VK_SELECT;
							return true;
						case GamePadButtons.LeftStick:
							key = KeyboardKeys.Tilde;
							return true;
						case GamePadButtons.RightStick:
                            key = KeyboardKeys.Space;
							return true;
						case GamePadButtons.DPadUp:
							key = KeyboardKeys.Shift;
							return true;
						case GamePadButtons.DPadLeft:
							key = KeyboardKeys.Alt;
							return true;
						case GamePadButtons.DPadDown:
							key = KeyboardKeys.Ctrl;
							return true;
						case GamePadButtons.DPadRight:
                            key = KeyboardKeys.B;
							return true;
					}
                    break;
            }
			return false;
        }
        #endregion GetKeyBinding

        #region Events
        void gc_ControllerConnected(object sender, GamePadEventArgs args)
        {
            OnControllerConnected(args.ControllerNum);
        }

        void gc_ControllerDisconnected(object sender, GamePadEventArgs args)
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

		static bool EnumHas<T>(T type, T value)
		{
			try
			{
				return (((int)(object)type & (int)(object)value) == (int)(object)value);
			}
			catch
			{
				return false;
			}
		}

        //appends a value
        static T EnumAdd<T>(T type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }

        //completely removes the value
        static T EnumRemove<T>(T type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
    }
}
