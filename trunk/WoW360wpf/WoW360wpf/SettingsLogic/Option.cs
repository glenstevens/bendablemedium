using System.Xml.Serialization;
using System;
using GameControllerLogic;

namespace SettingsLogic
{
    public enum ValueTypes
    {
        String,
        Integer,
        Double,
        Boolean,
        ThumbSticks
    }

    public enum OptionNames
    {
        MoveStick,
        LookStick,
        MouseLookSensitivity,
        CursorMoveSensitivity,
        CursorCenterOffsetX,
        CursorCenterOffsetY
    }

    public class Option
    {
        [XmlAttribute]
        public OptionNames Name { get; set; }
        [XmlAttribute]
        public string Value { get; set; }
        [XmlAttribute]
        public ValueTypes ValueType { get; set; }

        public Int32 GetValueAsInt32() { return Convert.ToInt32(Value); }
        public Double GetValueAsDouble() { return Convert.ToDouble(Value); }
        public Boolean GetValueAsBoolean() { return Convert.ToBoolean(Value); }
        public GameController.ThumbSticks GetValueAsThumbSticks() { return (GameController.ThumbSticks)Enum.Parse(typeof(GameController.ThumbSticks), Value); }
    }
}
