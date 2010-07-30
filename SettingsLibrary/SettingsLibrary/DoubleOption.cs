using System;

namespace SettingsLibrary
{
	class DoubleOption : Option
	{
		private double _value;

		public override string Value
		{
			get
			{
				return _value.ToString();
			}
			set
			{
				_value = Convert.ToDouble(value);
			}
		}

		public override string Type
		{
			get
			{
				return "double";
			}
		}

		public double DoubleValue
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
