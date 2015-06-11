using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{

    class MainViewModel : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private int selectedTab;

        public int SelectedTab
        {
            get { return selectedTab; }
            set
            {
                selectedTab = value;
                OnPropertyChanged("SelectedTab");
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
    }
}
