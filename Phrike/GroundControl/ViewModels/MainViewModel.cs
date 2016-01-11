using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object currentViewModel = AppOverviewViewModel.Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        private static MainViewModel instance;

        public static MainViewModel Instance => instance ?? (instance = new MainViewModel());

        public object CurrentViewModel 
        {
            get { return this.currentViewModel; }
            set
            {
                if (Equals(value, this.currentViewModel))
                {
                    return;
                }
                this.currentViewModel = value;
                this.OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            CurrentViewModel = new AppOverviewViewModel();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
