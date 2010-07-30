using System;

namespace SettingsLibrary
{
	class BooleanOption : Option
	{
		private bool _value;

		public override string Value
		{
			get
			{
				return _value.ToString();
			}
			set
			{
				_value = Convert.ToBoolean(value);
			}
		}

		public override string Type
		{
			get
			{
				return "bool";
			}
		}

		public bool BooleanValue
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
