using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SettingsLibrary
{
	public class SettingsProfile
	{
		private readonly Dictionary<string, OptionGroup> _dict = new Dictionary<string, OptionGroup>();

		public List<string> GetProfileList()
		{
			if (_dict.Count == 0)
			{
				// Create a default group if we don't have any groups
				_dict.Add("Default", new OptionGroup());
			}

			return new List<string>(_dict.Keys);
		}

		public OptionGroup GetOptionGroup(string name)
		{
			if (string.IsNullOrEmpty(name)) throw new NullReferenceException();

			if (_dict.ContainsKey(name)) return _dict[name];

			// If the key is not found, create a new blank option group
			OptionGroup og = new OptionGroup { Name = name };
			_dict.Add(name, og);
			return og;
		}

		public void SaveSettings(string filename)
		{
			XmlSerializer serializer =
				  new XmlSerializer(typeof(SettingsProfile));
			TextWriter writer = new StreamWriter(filename);
			serializer.Serialize(writer, this);
			writer.Close();
		}

		public static SettingsProfile LoadSettings(string filename)
		{
			if (!File.Exists(filename)) return new SettingsProfile();

			XmlSerializer serializer =
				  new XmlSerializer(typeof(SettingsProfile));
			FileStream fs = new FileStream(filename, FileMode.Open);
			SettingsProfile sc = (SettingsProfile)serializer.Deserialize(fs);
			return sc;
		}
	}
}
