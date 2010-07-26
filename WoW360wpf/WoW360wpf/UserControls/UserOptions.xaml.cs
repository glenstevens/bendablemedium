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

namespace WoW360wpf.UserControls
{
    /// <summary>
    /// Interaction logic for UserOptions.xaml
    /// </summary>
    public partial class UserOptions : UserControl
    {
        SettingsLogic.Settings _settings;

        public UserOptions()
        {
            InitializeComponent();
        }

        public void LoadSettings(SettingsLogic.Settings settings)
        {
            _settings = settings;

            MouseLookSlider.Value = _settings.Options.GetOption(SettingsLogic.OptionNames.MouseLookSensitivity).GetValueAsDouble();
            CursorMoveSlider.Value = _settings.Options.GetOption(SettingsLogic.OptionNames.CursorMoveSensitivity).GetValueAsDouble();
            CursorCenterOffsetXSlider.Value = _settings.Options.GetOption(SettingsLogic.OptionNames.CursorCenterOffsetX).GetValueAsDouble();
            CursorCenterOffsetYSlider.Value = _settings.Options.GetOption(SettingsLogic.OptionNames.CursorCenterOffsetY).GetValueAsDouble();
        }

        private void MouseLookSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_settings != null)
                _settings.Options.GetOption(SettingsLogic.OptionNames.MouseLookSensitivity).Value = e.NewValue.ToString();
        }

        private void CursorMoveSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_settings != null)
                _settings.Options.GetOption(SettingsLogic.OptionNames.CursorMoveSensitivity).Value = e.NewValue.ToString();
        }

        private void CursorCenterOffsetXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_settings != null)
                _settings.Options.GetOption(SettingsLogic.OptionNames.CursorCenterOffsetX).Value = e.NewValue.ToString();
        }

        private void CursorCenterOffsetYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_settings != null)
                _settings.Options.GetOption(SettingsLogic.OptionNames.CursorCenterOffsetY).Value = e.NewValue.ToString();
        }
    }
}
