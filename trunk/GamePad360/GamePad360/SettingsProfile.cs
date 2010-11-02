using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using GamePadUtilities;

namespace GamePad360
{
    public class SettingsProfile
    {
        public double MouseLookSensitivity { get; set; }
        public double CursorMoveSensitivity { get; set; }
        public Vector2 CursorOffset { get; set; }

        public Dictionary<GamePadTriggers, Dictionary<GamePadButtons, KeyBind>> Keybindings { get; set; }
    }
}
