using System.Windows;
using MahApps.Metro.Controls;

namespace Phrike.GroundControl
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick_Settings(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = TabItemSettings;
        }
    }
}
