using System;
using System.Windows.Forms;
using SettingsLibrary;

namespace SettingsLibraryTester
{
	public partial class Form1 : Form
	{
		private const string filename = "settings.xml";
		private SettingsProfile sp;
		private OptionGroup og;
		private Triggers curTrigger = Triggers.None;

		public Form1()
		{
			InitializeComponent();

			sp = SettingsProfile.LoadSettings(filename);
			cboProfiles.DataSource = sp.GetProfileList();
			cboTrigger.DataSource = Enum.GetValues(typeof (Triggers));
		}

		private void txtButtonA_MouseClick(object sender, MouseEventArgs e)
		{
			txtButtonA.Text = string.Empty;
		}

		private void txtButtonB_MouseClick(object sender, MouseEventArgs e)
		{
			txtButtonB.Text = string.Empty;
		}

		private void txtButtonX_MouseClick(object sender, MouseEventArgs e)
		{
			txtButtonX.Text = string.Empty;
		}

		private void txtButtonY_MouseClick(object sender, MouseEventArgs e)
		{
			txtButtonY.Text = string.Empty;
		}

		private void txtButtonA_KeyUp(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonB_KeyUp(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonX_KeyUp(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonY_KeyUp(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonA_KeyDown(object sender, KeyEventArgs e)
		{
			txtButtonA.Text = getText(e);
			e.Handled = true;
		}

		private void txtButtonB_KeyDown(object sender, KeyEventArgs e)
		{
			txtButtonB.Text = getText(e);
			e.Handled = true;
		}

		private void txtButtonX_KeyDown(object sender, KeyEventArgs e)
		{
			txtButtonX.Text = getText(e);
			e.Handled = true;
		}

		private void txtButtonY_KeyDown(object sender, KeyEventArgs e)
		{
			txtButtonY.Text = getText(e);
			string key = KeybindOption.CreateKey(curTrigger, Buttons.Y);
			Option op;
			if (og.Options.ContainsKey(key)) op = og.Options[key];
			else op = new KeybindOption();

			KeybindOption ko = op as KeybindOption;
			if (ko == null) return;

			if (e.Control) ko.Modifier = ko.Modifier | Modifiers.Ctrl;
			if (e.Alt) ko.Modifier = ko.Modifier | Modifiers.Alt;
			if (e.Shift) ko.Modifier = ko.Modifier | Modifiers.Shift;

			if (e.KeyCode != Keys.ShiftKey
				&& e.KeyCode != Keys.ControlKey
				&& e.KeyCode != Keys.Menu
				&& e.KeyCode != Keys.None)
			{
				key = e.KeyCode.ToString();
			}
			e.Handled = true;
		}

		private string getText(KeyEventArgs e)
		{
			string ctrl = e.Control ? "Ctrl " : "";
			string alt = e.Alt ? "Alt " : "";
			string shift = e.Shift ? "Shift " : "";
			string key = string.Empty;

			if (e.KeyCode != Keys.ShiftKey
			    && e.KeyCode != Keys.ControlKey
			    && e.KeyCode != Keys.Menu
			    && e.KeyCode != Keys.None)
			{
				key = e.KeyCode.ToString();
			}

			return ctrl + alt + shift + key;
		}

		private void txtButtonA_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonB_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonX_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void txtButtonY_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void cboProfiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(sender is ComboBox)) return;

			ComboBox cb = sender as ComboBox;
			string txt = cb.SelectedItem as string;
			//MessageBox.Show((sender as ComboBox).SelectedItem as string);
			og = sp.GetOptionGroup(txt);
		}

		private void cboTrigger_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(sender is ComboBox)) return;

			ComboBox cb = sender as ComboBox;
			string txt = cb.SelectedItem as string;

			if (string.IsNullOrEmpty(txt)) return;

			curTrigger = (Triggers)Enum.Parse(typeof(Triggers), txt);
		}
	}
}
