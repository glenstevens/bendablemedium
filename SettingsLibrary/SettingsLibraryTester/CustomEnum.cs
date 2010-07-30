
using System;
namespace SettingsLibraryTester
{
	public enum CustomEnum
	{
		EnumValue1, EnumValue2, EnumValue3
	}

	public enum Triggers
	{
		None, Left, Right, Both
	}

	public enum Buttons
	{
		None, A, B, X, Y
	}

	[Flags]
	public enum Modifiers
	{
		None = 0,
		Shift = 1,
		Alt = 2,
		Ctrl = 4
	}

	public enum KeyboardKeys
	{
		A, B, C, D, E, F, G
	}
}
