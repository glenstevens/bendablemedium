using System.Xml.Serialization;

namespace SettingsLibrary
{
	public class Option
	{
		[XmlAttribute]
		public virtual string Key { get; set; }

		[XmlAttribute]
		public virtual string Value { get; set; }

		[XmlAttribute]
		public virtual string Type { get { return "string"; } }
	}
}
