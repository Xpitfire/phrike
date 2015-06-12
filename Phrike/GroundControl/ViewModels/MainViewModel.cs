using System.ComponentModel;
using System.Runtime.CompilerServices;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{

    class MainViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static MainViewModel Instance { get; private set; }

        private int selectedTab;

        public MainViewModel()
        {
            Instance = this;
        }

        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                OnPropertyChanged();
            }
        }

        public void SelectTabSettings()
        {
            SelectedTab = 3;
        }

        public void SelectTabNewStresstest()
        {
            SelectedTab = 1;
        }

        public void SelectTabAnalysis()
        {
            SelectedTab = 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ShowDialogMessage(string tile, string message)
        {
            MainWindow.Instance.Dispatcher.Invoke(() => MainWindow.Instance.ShowMessageAsync(tile, message));
        }

        private ProgressDialogController progressDialogController;

        public void ShowProgressMessage(string title, string message)
        {
            MainWindow.Instance.Dispatcher.Invoke(async () =>
            {
                progressDialogController = await MainWindow.Instance.ShowProgressAsync(title, message);
            });
        }

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

    }
}
