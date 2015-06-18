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

        /// <summary>
        /// Switch viewed tab to the Settings category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_Settings(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = TabItemSettings;
        }

        /// <summary>
        /// Save close of all application instances operating on hardware, sockets or
        /// multiple threads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ApplicationClose(object sender, CancelEventArgs e)
        {
            Logger.Info("Application close triggered! Preventive stop send to all tasks.");
            StressTestViewModel.Instance.ApplicationClose();
            Logger.Info("Successfully closed application!");
        }
    }
}
