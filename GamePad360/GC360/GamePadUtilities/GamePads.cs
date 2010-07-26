using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GamePadUtilities
{
    /// <summary>
    /// Represents an Xbox360 Game Controller and handles getting the state and setting
    /// vibration amount.  Includes events that can be subscribed to for button presses, etc.
    /// </summary>
    public class GamePads
    {
        #region Delegates
        public delegate void ButtonStateChangedHandler(object sender, ButtonStateEventArgs args);
        public delegate void TriggerStateUpdatedHandler(object sender, TriggerStateEventArgs args);
        public delegate void ThumbStickStateUpdatedHandler(object sender, ThumbStickStateEventArgs args);
        public delegate void ControllerConnectedHandler(object sender, GamePadEventArgs args);
        public delegate void ControllerDisconnectedHandler(object sender, GamePadEventArgs args);
        #endregion Delegates

        #region Events
        /// <summary>
        /// Event fired when any button is pressed or released
        /// </summary>
        public event ButtonStateChangedHandler ButtonStateChanged;

        /// <summary>
        /// Event fired when either trigger has had it's value updated
        /// </summary>
        public event TriggerStateUpdatedHandler TriggerStateUpdated;

        /// <summary>
        /// Event fired when either thumbstick has had it's value updated
        /// </summary>
        public event ThumbStickStateUpdatedHandler ThumbStickStateUpdated;

        /// <summary>
        /// Event fired when a controller is connected
        /// </summary>
        public event ControllerConnectedHandler ControllerConnected;

        /// <summary>
        /// Event fired when a controller is disconnected
        /// </summary>
        public event ControllerDisconnectedHandler ControllerDisconnected;

        private void OnButtonStateChanged(object sender, ButtonStateEventArgs args)
        {
            if (ButtonStateChanged != null) ButtonStateChanged(sender, args); 
        }

        private void OnTriggerStateUpdated(object sender, TriggerStateEventArgs args)
        {
            if (TriggerStateUpdated != null) TriggerStateUpdated(sender, args);
        }

        private void OnThumbStickStateUpdated(object sender, ThumbStickStateEventArgs args)
        {
            if (ThumbStickStateUpdated != null) ThumbStickStateUpdated(sender, args);
        }

        private void OnControllerConnected(object sender, GamePadEventArgs args)
        {
            if (ControllerConnected != null) ControllerConnected(sender, args);
        }

        private void OnControllerDisconnected(object sender, GamePadEventArgs args)
        {
            if (ControllerDisconnected != null) ControllerDisconnected(sender, args);
        }
        #endregion Events

        private Dictionary<GamePadNumber, GamePadState> _previousState = new Dictionary<GamePadNumber, GamePadState>(4);

        public GamePads()
        {
            // Get the previous state of each controller
            foreach (GamePadNumber cn in Enum.GetValues(typeof(GamePadNumber)))
            {
                // Skip the None controller since it doesn't exist in XNA
                if (cn == GamePadNumber.None) continue;

                PlayerIndex pi = GetPlayerIndexFromControllerNumber(cn);
                _previousState.Add(cn, GamePad.GetState(pi));

                // Turn off vibration to start
                GamePad.SetVibration(pi, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// Get the XNA enumerated player index from local mapping
        /// </summary>
        /// <param name="ControllerNumber">Local enumerated controller number type</param>
        /// <returns>XNA enumerated player index type</returns>
        private PlayerIndex GetPlayerIndexFromControllerNumber(GamePadNumber ControllerNumber)
        {
            PlayerIndex pi = PlayerIndex.One;
            switch (ControllerNumber)
            {
                case GamePadNumber.One:
                    pi = PlayerIndex.One;
                    break;
                case GamePadNumber.Two:
                    pi = PlayerIndex.Two;
                    break;
                case GamePadNumber.Three:
                    pi = PlayerIndex.Three;
                    break;
                case GamePadNumber.Four:
                    pi = PlayerIndex.Four;
                    break;
                default:
                    break;
            }
            return pi;
        }

        /// <summary>
        /// Update the controller state and fire off any events related to changed state
        /// </summary>
        /// <param name="ControllerNum">The controller to poll</param>
        public void UpdateControllerState(GamePadNumber ControllerNum)
        {
            PlayerIndex pi = GetPlayerIndexFromControllerNumber(ControllerNum);

            // Get the current gamepad state.
            GamePadState currentState = GamePad.GetState(pi);

            GamePadState previousState = _previousState[ControllerNum];

            if (previousState.IsConnected != currentState.IsConnected)
            {
                // connected state changed
                if (currentState.IsConnected) OnControllerConnected(this, new GamePadEventArgs(ControllerNum));
                else OnControllerDisconnected(this, new GamePadEventArgs(ControllerNum));
            }

            // Process input only if connected.
            if (currentState.IsConnected)
            {
                // Check buttons vs previous state
                CheckButtonsChanged(ControllerNum, GamePadButtons.A, currentState.Buttons.A, previousState.Buttons.A);
                CheckButtonsChanged(ControllerNum, GamePadButtons.B, currentState.Buttons.B, previousState.Buttons.B);
                CheckButtonsChanged(ControllerNum, GamePadButtons.X, currentState.Buttons.X, previousState.Buttons.X);
                CheckButtonsChanged(ControllerNum, GamePadButtons.Y, currentState.Buttons.Y, previousState.Buttons.Y);
                CheckButtonsChanged(ControllerNum, GamePadButtons.Back, currentState.Buttons.Back, previousState.Buttons.Back);
                CheckButtonsChanged(ControllerNum, GamePadButtons.LeftShoulder, currentState.Buttons.LeftShoulder, previousState.Buttons.LeftShoulder);
                CheckButtonsChanged(ControllerNum, GamePadButtons.LeftStick, currentState.Buttons.LeftStick, previousState.Buttons.LeftStick);
                CheckButtonsChanged(ControllerNum, GamePadButtons.RightShoulder, currentState.Buttons.RightShoulder, previousState.Buttons.RightShoulder);
                CheckButtonsChanged(ControllerNum, GamePadButtons.RightStick, currentState.Buttons.RightStick, previousState.Buttons.RightStick);
                CheckButtonsChanged(ControllerNum, GamePadButtons.Start, currentState.Buttons.Start, previousState.Buttons.Start);
                CheckButtonsChanged(ControllerNum, GamePadButtons.BigButton, currentState.Buttons.BigButton, previousState.Buttons.BigButton);
                CheckButtonsChanged(ControllerNum, GamePadButtons.DPadDown, currentState.DPad.Down, previousState.DPad.Down);
                CheckButtonsChanged(ControllerNum, GamePadButtons.DPadLeft, currentState.DPad.Left, previousState.DPad.Left);
                CheckButtonsChanged(ControllerNum, GamePadButtons.DPadRight, currentState.DPad.Right, previousState.DPad.Right);
                CheckButtonsChanged(ControllerNum, GamePadButtons.DPadUp, currentState.DPad.Up, previousState.DPad.Up);

                // Update previous gamepad state.
                _previousState[ControllerNum] = currentState;

                // Check Analog controls
                OnTriggerStateUpdated(this, new TriggerStateEventArgs(ControllerNum, GamePadTriggers.Left, currentState.Triggers.Left));
                OnTriggerStateUpdated(this, new TriggerStateEventArgs(ControllerNum, GamePadTriggers.Right, currentState.Triggers.Right));
                OnThumbStickStateUpdated(this, new ThumbStickStateEventArgs(ControllerNum, GamePadThumbSticks.Left, currentState.ThumbSticks.Left.X, currentState.ThumbSticks.Left.Y));
                OnThumbStickStateUpdated(this, new ThumbStickStateEventArgs(ControllerNum, GamePadThumbSticks.Right, currentState.ThumbSticks.Right.X, currentState.ThumbSticks.Right.Y));
            }
        }

        /// <summary>
        /// Checks if the buttons have changed and if so triggers the event
        /// </summary>
        /// <param name="ControllerNum">The currently polling controller</param>
        /// <param name="button">The local enum button being checked</param>
        /// <param name="current">The current value of the button being checked</param>
        /// <param name="previous">The previous value of the button being checked</param>
        private void CheckButtonsChanged(GamePadNumber ControllerNum, GamePadButtons button, ButtonState current, ButtonState previous)
        {
            if (current != previous)
                OnButtonStateChanged(this, new ButtonStateEventArgs(ControllerNum, button, current == ButtonState.Pressed));
        }

        /// <summary>
        /// Sets the controller vibration to the values specified
        /// </summary>
        /// <param name="ControllerNum">The controller to receive the vibration change</param>
        /// <param name="LeftMotor">Amount of the vibration of the left motor</param>
        /// <param name="RightMotor">Amount of the vibration of the right motor</param>
        public void SetControllerVibration(GamePadNumber ControllerNum, float LeftMotor, float RightMotor)
        {
            LeftMotor = MathHelper.Clamp(LeftMotor, 0.0f, 1.0f);
            RightMotor = MathHelper.Clamp(RightMotor, 0.0f, 1.0f);
            GamePad.SetVibration(GetPlayerIndexFromControllerNumber(ControllerNum), LeftMotor, RightMotor);
        }
    }
}
