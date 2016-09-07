using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using NLog;
using Phrike.GroundControl.ViewModels;
using System;

using Phrike.GroundControl.Views;

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
            DataContext = MainViewModel.Instance;

            Logger.Info("Application successfully started!");
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
            //StressTestView?.ApplicationClose(); //TODO Notify through viemodels for app close
            Logger.Info("Successfully closed application!");
        }

    }
}
