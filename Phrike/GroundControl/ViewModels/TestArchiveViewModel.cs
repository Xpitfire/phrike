using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataModel;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    class TestArchiveViewModel : INotifyPropertyChanged
    {
        //Initialize FilterDateTime with a default value
        private DateTime _filterDateTime = DateTime.Now;

        public TestArchiveViewModel()
        {
            if (DataLoadHelper.IsLoadDataActive())
            {
                TestVM = new TestCollectionVM();
                TestList = TestVM.Tests;
            }
        }

        public DateTime FilterDateTime
        {
            get { return _filterDateTime; }
            set
            {
                _filterDateTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterDateTime)));
            }
        }

        public TestCollectionVM TestVM { get; }

        public ObservableCollection<TestVM> TestList { get; } 

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
