using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SettingsLogic
{
    public class SettingsCollection : ICollection<Settings>
    {
        public Dictionary<string, Settings> _dict = new Dictionary<string, Settings>();

        public void SaveSettings(string filename)
        {
            XmlSerializer serializer =
                  new XmlSerializer(typeof(SettingsCollection));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, this);
            writer.Close();
        }

        public static SettingsCollection LoadSettings(string filename)
        {
            if (!File.Exists(filename)) return new SettingsCollection();

            XmlSerializer serializer =
                  new XmlSerializer(typeof(SettingsCollection));
            FileStream fs = new FileStream(filename, FileMode.Open);
            SettingsCollection sc = (SettingsCollection)serializer.Deserialize(fs);
            return sc;
        }

        public Settings GetSettings(string key)
        {
            if (_dict.ContainsKey(key))
            {
                return _dict[key];
            }

            // If the key is not found, attempt to get the first setting in the collection
            string defaultkey = null;
            foreach (string k in _dict.Keys)
            {
                defaultkey = k;
                break;
            }
            if (defaultkey != null)
                return _dict[defaultkey];

            // If there are no items in the collection, create a new one
            Settings s = new Settings { DisplayName = key };
            _dict.Add(key, s);
            return s;
        }

        public List<string> GetSettingsNamesList()
        {
            List<string> names = new List<string>();

            foreach (Settings s in _dict.Values)
            {
                names.Add(s.DisplayName);
            }
            return names;
        }

        #region ICollection<Settings> Members

        public void Add(Settings item)
        {
            _dict.Add(item.Key, item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(Settings item)
        {
            return _dict.ContainsKey(item.Key);
        }

        public void CopyTo(Settings[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Settings item)
        {
            return _dict.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<Settings> Members

        public IEnumerator<Settings> GetEnumerator()
        {
            foreach (Settings s in _dict.Values)
            {
                yield return s;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
