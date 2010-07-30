using System;

namespace SettingsLibrary
{
	class IntegerOption : Option
	{
		private int _value;

		public override string Value
		{
			get
			{
				return _value.ToString();
			}
			set
			{
				_value = Convert.ToInt32(value);
			}
		}

		public override string Type
		{
			get
			{
				return "int";
			}
		}

		public int IntegerValue
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
}
