using System;
using System.Collections.Generic;

namespace SettingsLogic
{
    public class OptionCollection : ICollection<Option>
    {
        public Dictionary<OptionNames, Option> _dict = new Dictionary<OptionNames, Option>();

        public Option GetOption(OptionNames key)
        {
            return _dict[key];
        }

        #region ICollection<Option> Members

        public void Add(Option item)
        {
            _dict.Add(item.Name, item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(Option item)
        {
            return _dict.ContainsKey(item.Name);
        }

        public void CopyTo(Option[] array, int arrayIndex)
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

        public bool Remove(Option item)
        {
            return _dict.Remove(item.Name);
        }

        #endregion

        #region IEnumerable<Option> Members

        public IEnumerator<Option> GetEnumerator()
        {
            foreach (Option o in _dict.Values)
            {
                yield return o;
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
