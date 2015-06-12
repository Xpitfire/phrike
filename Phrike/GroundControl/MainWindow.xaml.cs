using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using NLog;
using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void ButtonClick_Settings(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = TabItemSettings;
        }

        public void ApplicationClose(object sender, CancelEventArgs e)
        {
            StressTestViewModel.Instance.ApplicationClose();
            Logger.Info("Successfully closed application!");
        }
    }
}
