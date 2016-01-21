using Phrike.GroundControl.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void BtnFile_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new Microsoft.Win32.OpenFileDialog() { Filter = "Unreal Engine (UE4Editor.exe)|UE4Editor.exe" };
            var result = ofd.ShowDialog();
            if (result == false) return;
            ((SettingsViewModel)this.FindResource("SettingsViewModel")).UEPath = ofd.FileName;
        }
    }
}
