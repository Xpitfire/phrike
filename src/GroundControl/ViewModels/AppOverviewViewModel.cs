﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using MahApps.Metro.Controls.Dialogs;
using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{

    public class AppOverviewViewModel : INotifyPropertyChanged
    {
        public static AppOverviewViewModel Instance
            => instance ?? (instance = new AppOverviewViewModel());

        private AppOverviewViewModel()
        {
            IsEnabled = true;
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isEnabled;

        #region Tab Control

        private int selectedTab;

        /// <summary>
        /// Handle index of the current tab selection.
        /// </summary>
        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Change the current tab view to the "Settings" view.
        /// </summary>
        public void SelectTabSettings()
        {
            SelectedTab = 3;
        }

        /// <summary>
        /// Change the current tab view to the "Debug" view.
        /// </summary>
        public void SelectTabDebug()
        {
            SelectedTab = 4;
        }

        /// <summary>
        /// Change the current tab view to the "NewStresstest" view.
        /// </summary>
        public void SelectTabNewStresstest()
        {
            SelectedTab = 1;
        }

        /// <summary>
        /// Change the current tab view to the "Analysis" view.
        /// </summary>
        public void SelectTabAnalysis()
        {
            SelectedTab = 2;
        }

        public void SelectTabUser()
        {
            //SelectedTab = 4;
        }
        #endregion

        #region PropertyChanged Handling
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handle the Property change Binding updates.
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region UI Interaction methods

        // Process controller of the process overlay screen
        private ProgressDialogController progressDialogController;

        private static AppOverviewViewModel instance;

        /// <summary>
        /// Show UI progress dialog messages.
        /// </summary>
        /// <param name="title">The dialog title info.</param>
        /// <param name="message">The info message to be displayed.</param>
        public void ShowProgressMessage(string title, string message)
        {
            MainWindow.Instance.Dispatcher.Invoke(async () =>
            {
                progressDialogController = await MainWindow.Instance.ShowProgressAsync(title, message);
            });
        }

        /// <summary>
        /// Close UI progress dialog messages.
        /// </summary>
        public void CloseProgressMessage()
        {
            if (progressDialogController != null)
            {
                MainWindow.Instance.Dispatcher.Invoke(async () =>
                {
                    await progressDialogController.CloseAsync();
                });
                progressDialogController = null;
            }
        }

        #endregion

    }
}
