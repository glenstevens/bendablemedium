using System;
using System.Windows.Input;
using System.Xml.Serialization;
using GameControllerLogic;

namespace SettingsLogic
{
    public class KeyBinding
    {
        string _key = null;

        [XmlAttribute]
        public GameController.Triggers TriggerModifier { get; set; }
        [XmlAttribute]
        public GameController.Buttons Button { get; set; }
        [XmlAttribute]
        public Key KeyBoardKey { get; set; }
        [XmlAttribute]
        public ModifierKeys Modifier { get; set; }

        [XmlIgnore]
        public string Key
        {
            get
            {
                if (_key == null)
                    _key = CreateKey(TriggerModifier, Button);
                return _key;
            }
        }

        public static string CreateKey(GameController.Triggers TriggerModifier, GameController.Buttons Button)
        {
            return String.Concat(TriggerModifier.ToString(), Button.ToString());
        }
    }
}
