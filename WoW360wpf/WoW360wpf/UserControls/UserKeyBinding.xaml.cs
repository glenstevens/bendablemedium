using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameControllerLogic;

namespace WoW360wpf.UserControls
{
    /// <summary>
    /// Interaction logic for UserKeyBinding.xaml
    /// </summary>
    public partial class UserKeyBinding : UserControl
    {
        SettingsLogic.Settings _settings;
        GameController.Triggers _currentTrigger;

        public UserKeyBinding()
        {
            InitializeComponent();
            BindDropDowns();
        }

        private void BindDropDowns()
        {
            TriggersPressed.ItemsSource = Enum.GetValues(typeof(GameController.Triggers));
            ModifierA.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierB.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierX.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierY.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierStart.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierBack.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierLeftShoulder.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierRightShoulder.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierLeftStick.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierRightStick.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierDPadDown.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierDPadLeft.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierDPadRight.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            ModifierDPadUp.ItemsSource = Enum.GetValues(typeof(ModifierKeys));
            KeyA.ItemsSource = Enum.GetValues(typeof(Key));
            KeyB.ItemsSource = Enum.GetValues(typeof(Key));
            KeyX.ItemsSource = Enum.GetValues(typeof(Key));
            KeyY.ItemsSource = Enum.GetValues(typeof(Key));
            KeyStart.ItemsSource = Enum.GetValues(typeof(Key));
            KeyBack.ItemsSource = Enum.GetValues(typeof(Key));
            KeyLeftShoulder.ItemsSource = Enum.GetValues(typeof(Key));
            KeyRightShoulder.ItemsSource = Enum.GetValues(typeof(Key));
            KeyLeftStick.ItemsSource = Enum.GetValues(typeof(Key));
            KeyRightStick.ItemsSource = Enum.GetValues(typeof(Key));
            KeyDPadDown.ItemsSource = Enum.GetValues(typeof(Key));
            KeyDPadLeft.ItemsSource = Enum.GetValues(typeof(Key));
            KeyDPadRight.ItemsSource = Enum.GetValues(typeof(Key));
            KeyDPadUp.ItemsSource = Enum.GetValues(typeof(Key));
        }

        public void LoadSettings(SettingsLogic.Settings settings)
        {
            _settings = settings;

            _currentTrigger = GameController.Triggers.None;
            TriggersPressed.SelectedValue = _currentTrigger;

            SetDropDowns();
        }

        private void SetDropDowns()
        {
            ModifierA.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.A).Modifier;
            ModifierB.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.B).Modifier;
            ModifierX.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.X).Modifier;
            ModifierY.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.Y).Modifier;
            ModifierStart.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.Start).Modifier;
            ModifierBack.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.Back).Modifier;
            ModifierLeftShoulder.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.LeftShoulder).Modifier;
            ModifierRightShoulder.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.RightShoulder).Modifier;
            ModifierLeftStick.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.LeftStick).Modifier;
            ModifierRightStick.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.RightStick).Modifier;
            ModifierDPadDown.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadDown).Modifier;
            ModifierDPadLeft.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadLeft).Modifier;
            ModifierDPadRight.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadRight).Modifier;
            ModifierDPadUp.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadUp).Modifier;
            KeyA.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.A).KeyBoardKey;
            KeyB.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.B).KeyBoardKey;
            KeyX.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.X).KeyBoardKey;
            KeyY.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.Y).KeyBoardKey;
            KeyStart.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.Start).KeyBoardKey;
            KeyBack.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.Back).KeyBoardKey;
            KeyLeftShoulder.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.LeftShoulder).KeyBoardKey;
            KeyRightShoulder.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.RightShoulder).KeyBoardKey;
            KeyLeftStick.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.LeftStick).KeyBoardKey;
            KeyRightStick.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.RightStick).KeyBoardKey;
            KeyDPadDown.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadDown).KeyBoardKey;
            KeyDPadLeft.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadLeft).KeyBoardKey;
            KeyDPadRight.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadRight).KeyBoardKey;
            KeyDPadUp.SelectedValue = _settings.KeyBindings.GetKeyBinding(_currentTrigger, GameController.Buttons.DPadUp).KeyBoardKey;
        }

        private void TriggersPressed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentTrigger = (GameController.Triggers)e.AddedItems[0];
            SetDropDowns();
        }

        private void Modifier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboBoxName = (e.Source as ComboBox).Name;

            GameController.Buttons currentButton = GetButtonByComboBoxName(comboBoxName);
            _settings.KeyBindings.GetKeyBinding(_currentTrigger, currentButton).Modifier = (ModifierKeys)e.AddedItems[0];
        }

        private void Key_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string comboBoxName = (e.Source as ComboBox).Name;

            GameController.Buttons currentButton = GetButtonByComboBoxName(comboBoxName);
            _settings.KeyBindings.GetKeyBinding(_currentTrigger, currentButton).KeyBoardKey = (Key)e.AddedItems[0];
        }

        private static GameController.Buttons GetButtonByComboBoxName(string comboBoxName)
        {
            GameController.Buttons currentButton = GameController.Buttons.None;

            switch (comboBoxName)
            {
                case "ModifierA":
                case "KeyA":
                    currentButton = GameController.Buttons.A;
                    break;
                case "ModifierB":
                case "KeyB":
                    currentButton = GameController.Buttons.B;
                    break;
                case "ModifierX":
                case "KeyX":
                    currentButton = GameController.Buttons.X;
                    break;
                case "ModifierY":
                case "KeyY":
                    currentButton = GameController.Buttons.Y;
                    break;
                case "ModifierStart":
                case "KeyStart":
                    currentButton = GameController.Buttons.Start;
                    break;
                case "ModifierBack":
                case "KeyBack":
                    currentButton = GameController.Buttons.Back;
                    break;
                case "ModifierLeftShoulder":
                case "KeyLeftShoulder":
                    currentButton = GameController.Buttons.LeftShoulder;
                    break;
                case "ModifierRightShoulder":
                case "KeyRightShoulder":
                    currentButton = GameController.Buttons.RightShoulder;
                    break;
                case "ModifierLeftStick":
                case "KeyLeftStick":
                    currentButton = GameController.Buttons.LeftStick;
                    break;
                case "ModifierRightStick":
                case "KeyRightStick":
                    currentButton = GameController.Buttons.RightStick;
                    break;
                case "ModifierDPadUp":
                case "KeyDPadUp":
                    currentButton = GameController.Buttons.DPadUp;
                    break;
                case "ModifierDPadDown":
                case "KeyDPadDown":
                    currentButton = GameController.Buttons.DPadDown;
                    break;
                case "ModifierDPadLeft":
                case "KeyDPadLeft":
                    currentButton = GameController.Buttons.DPadLeft;
                    break;
                case "ModifierDPadRight":
                case "KeyDPadRight":
                    currentButton = GameController.Buttons.DPadRight;
                    break;
            }
            return currentButton;
        }
    }
}
