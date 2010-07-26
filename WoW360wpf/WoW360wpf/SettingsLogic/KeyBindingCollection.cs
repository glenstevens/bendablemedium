using System;
using System.Collections.Generic;
using GameControllerLogic;
using System.Collections;

namespace SettingsLogic
{
    public class KeyBindingCollection : ICollection<KeyBinding>
    {
        public Dictionary<string, KeyBinding> _dict = new Dictionary<string, KeyBinding>();

        public KeyBinding GetKeyBinding(GameController.Triggers TriggerModifier, GameController.Buttons Button)
        {
            string key = KeyBinding.CreateKey(TriggerModifier, Button);
            if (_dict.ContainsKey(key))
                return _dict[key];
            return null;
        }

        public ICollection Values
        {
            get { return _dict.Values; }
        }

        #region ICollection<KeyBinding> Members

        public void Add(KeyBinding item)
        {
            _dict.Add(item.Key, item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyBinding item)
        {
            return _dict.ContainsKey(item.Key);
        }

        public void CopyTo(KeyBinding[] array, int arrayIndex)
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

        public bool Remove(KeyBinding item)
        {
            return _dict.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyBinding> Members

        public IEnumerator<KeyBinding> GetEnumerator()
        {
            foreach (KeyBinding kb in _dict.Values)
            {
                yield return kb;
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
