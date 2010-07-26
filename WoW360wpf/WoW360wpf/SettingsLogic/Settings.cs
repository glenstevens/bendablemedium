using System.Xml.Serialization;

namespace SettingsLogic
{
    public class Settings
    {
        string _key = null;

        [XmlIgnore]
        public string Key
        {
            get
            {
                if (_key == null)
                {
                    _key = GetKeyFromDisplayName(DisplayName);
                }
                return _key;
            }
        }

        [XmlAttribute]
        public string DisplayName { get; set; }
        public OptionCollection Options = new OptionCollection();
        public KeyBindingCollection KeyBindings = new KeyBindingCollection();

        public string GetKeyFromDisplayName(string displayName)
        {
            return displayName.ToLowerInvariant().Replace(" ", "");
        }
    }
}
