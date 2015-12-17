using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataAccess;
using DataModel;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    class TestArchiveViewModel : INotifyPropertyChanged
    {
        //Initialize FilterDateTime with a default value
        private DateTime _filterDateTime = DateTime.Now;

        private bool filterAll = true;
        private bool filterSubject;
        private bool filterDate;
        private bool filterScenario;
        private DateTime dateFilter;

        ///TODO: http://stackoverflow.com/questions/9212873/binding-radiobuttons-group-to-a-property-in-wpf

        private void UpdateVisibilities()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubjVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScenVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateVisibility)));
        }

        public bool FilterAll
        {
            get
            {
                return filterAll;
            }
            set
            {
                if (filterAll != value)
                {
                    filterAll = value;
                    if (value)
                    {
                        FilterDate = false;
                        FilterSubject = false;
                        FilterScenario = false;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterAll)));
                    UpdateVisibilities();
                }
            }
        }
        public bool FilterSubject
        {
            get
            {
                return filterSubject;
            }
            set
            {
                if (filterSubject != value)
                {
                    filterSubject = value;
                    if (value)
                    {
                        FilterDate = false;
                        FilterAll = false;
                        FilterScenario = false;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterSubject)));
                    UpdateVisibilities();
                }
            }
        }
        public bool FilterDate
        {
            get
            {
                return filterDate;
            }
            set
            {
                if (filterDate != value)
                {
                    filterDate = value;
                    if (value)
                    {
                        FilterAll = false;
                        FilterSubject = false;
                        FilterScenario = false;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterDate)));
                    UpdateVisibilities();
                }
            }
        }
        public bool FilterScenario
        {
            get
            {
                return filterScenario;
            }
            set
            {
                if (filterAll != value)
                {
                    filterScenario = value;
                    if (value)
                    {
                        FilterDate = false;
                        FilterSubject = false;
                        FilterAll = false;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterScenario)));
                    UpdateVisibilities();
                }
            }
        }

        public Visibility SubjVisibility { get { return FilterSubject ? Visibility.Visible : Visibility.Hidden;} }
        public Visibility ScenVisibility { get { return FilterScenario ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility DateVisibility { get { return  FilterScenario ? Visibility.Visible : Visibility.Hidden; } }


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

        public bool Filter(object o)
        {
            if (!(o is TestVM))
            {
                return false;
            }

            if (FilterAll)
            {
                return true;
            }
            else if (FilterDate)
            {
                return true;
            }
            else if (FilterScenario)
            {
                return true;
            }
            else if (FilterSubject)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
