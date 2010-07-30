using System.Collections.Generic;
using System.Xml.Serialization;

namespace SettingsLibrary
{
	public class OptionGroup
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlAttribute]
		public Dictionary<string, Option> Options { get; set; }
	}
}
