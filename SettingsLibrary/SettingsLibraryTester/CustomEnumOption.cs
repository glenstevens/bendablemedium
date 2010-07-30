using System;
using SettingsLibrary;

namespace SettingsLibraryTester
{
	public class CustomEnumOption : Option
	{
		private CustomEnum _value;

		public override string Value
		{
			get
			{
				return _value.ToString();
			}
			set
			{
				_value = (CustomEnum)Enum.Parse(typeof(CustomEnum), value);
			}
		}

		public override string Type
		{
			get
			{
				return "CustomEnum";
			}
		}

		public CustomEnum CustomEnumValue
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}
	}

	public class KeybindOption : Option
	{
		private Triggers _trigger;
		private Buttons _button;
		private Modifiers _modifier;
		private KeyboardKeys _keyboardKey;

		public static string CreateKey(Triggers trigger, Buttons button)
		{
			return string.Concat(trigger, "_", button);
		}

		public override string Key
		{
			get
			{
				return CreateKey(_trigger, _button);
			}
			set
			{
				string[] split = value.Split(Convert.ToChar("_"));
				_trigger = (Triggers) Enum.Parse(typeof (Triggers), split[0]);
				_button = (Buttons)Enum.Parse(typeof(Buttons), split[1]);
			}
		}

		public override string Value
		{
			get
			{
				return string.Concat(_modifier, "_", _keyboardKey);
			}
			set
			{
				string[] split = value.Split(Convert.ToChar("_"));
				_modifier = (Modifiers)Enum.Parse(typeof(Modifiers), split[0]);
				_keyboardKey = (KeyboardKeys)Enum.Parse(typeof(KeyboardKeys), split[1]);
			}
		}

		public override string Type
		{
			get
			{
				return "Keybind";
			}
		}

		public Modifiers Modifier
		{
			get
			{
				return _modifier;
			}
			set
			{
				_modifier = value;
			}
		}

		public KeyboardKeys KeyboardKey
		{
			get
			{
				return _keyboardKey;
			}
			set
			{
				_keyboardKey = value;
			}
		}
	}
}
