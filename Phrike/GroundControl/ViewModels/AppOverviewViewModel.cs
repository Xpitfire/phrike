using System.ComponentModel;
using System.Runtime.CompilerServices;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{

    public class AppOverviewViewModel : INotifyPropertyChanged
    {
        public static AppOverviewViewModel Instance { get; private set; }

        public AppOverviewViewModel()
        {
            Instance = this;
        }               

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
