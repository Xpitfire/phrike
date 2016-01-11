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
using System.Windows.Data;
using System.Globalization;
using System.Windows.Input;

namespace Phrike.GroundControl.ViewModels
{

    public class EnumBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? parameter : Binding.DoNothing;
        }
    }

    public enum FilterType
    {
        Subject,
        Scenario,
        Date,
        All
    };

    public delegate void FilterChangedEvent();

    class TestArchiveViewModel : INotifyPropertyChanged
    {
        //Initialize FilterDateTime with a default value
        private DateTime _filterDateTime = DateTime.Now;
        private FilterType filterType = FilterType.All;
        private ObservableCollection<TestVM> tests;
        private SubjectVM selectedSubject;
        private ScenarioVM selectedScenario;
        private string filterString = "";
        private RelayCommand detailCmd;
        private RelayCommand deleteCmd;

        public FilterChangedEvent FilterChanged;


        #region FilterVisibilities
        private void UpdateVisibilities()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterType)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubjVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScenVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateVisibility)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tests)));
            FilterChanged.Invoke();
        }
        public FilterType FilterType
        {
            get { return filterType; }
            set
            {
                if (filterType != value)
                {
                    filterType = value;
                    UpdateVisibilities();
                }
            }
        }
        public Visibility SubjVisibility { get { return FilterType == FilterType.Subject ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility ScenVisibility { get { return FilterType == FilterType.Scenario ? Visibility.Visible : Visibility.Hidden; } }
        public Visibility DateVisibility { get { return FilterType == FilterType.Date ? Visibility.Visible : Visibility.Hidden; } }
        #endregion

        public TestArchiveViewModel()
        {
            tests = new ObservableCollection<TestVM>();
            ScenarioList = new ObservableCollection<ScenarioVM>();
            SubjectList = new ObservableCollection<SubjectVM>();
            if (DataLoadHelper.IsLoadDataActive())
            {
                LoadTests();
                LoadSubjects();
                LoadScenarios();
            }
        }

        private async void LoadScenarios()
        {
            ScenarioList.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Scenario> enu = x.ScenarioRepository.Get(includeProperties: "Tests").Where(sc => sc.Tests.Count > 0).GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    ScenarioList.Add(new ScenarioVM(enu.Current));
                }
            }

            SelectedScenario = ScenarioList.FirstOrDefault();
        }

        private async void LoadSubjects()
        {
            SubjectList.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Subject> enu = x.SubjectRepository.Get(includeProperties: "Tests").Where(su => su.Tests.Count > 0).GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    SubjectList.Add(new SubjectVM(enu.Current));
                }
            }

            SelectedSubject = SubjectList.FirstOrDefault();
        }

        private async void LoadTests()
        {
            Tests.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Test> enu = x.TestRepository.Get(includeProperties: "Subject,Scenario").GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Tests.Add(new TestVM(enu.Current));
                }
            }
        }

        public DateTime FilterDateTime
        {
            get { return _filterDateTime; }
            set
            {
                _filterDateTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterDateTime)));
                FilterChanged?.Invoke();
            }
        }
        public SubjectVM SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                if (selectedSubject != value)
                {
                    selectedSubject = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSubject)));
                    FilterChanged?.Invoke();
                }
            }
        }
        public ScenarioVM SelectedScenario
        {
            get
            {
                return selectedScenario;
            }
            set
            {
                if (selectedScenario != value)
                {
                    selectedScenario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedScenario)));
                    FilterChanged?.Invoke();
                }
            }
        }
        public string FilterString
        {
            get { return filterString; }
            set
            {
                if (filterString != value)
                {
                    filterString = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FilterString)));
                    FilterChanged?.Invoke();
                }
            }
        }
        public TestVM SelectedTest { get; set; }
        public ObservableCollection<TestVM> Tests
        {
            get
            {
                return tests;
            }
            private set
            {
                if (tests != value)
                {
                    tests = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tests)));
                }
            }
        }
        public ObservableCollection<ScenarioVM> ScenarioList { get; private set; }
        public ObservableCollection<SubjectVM> SubjectList { get; private set; }


        public event PropertyChangedEventHandler PropertyChanged;

        private bool DateEquals(DateTime dt1, DateTime dt2)
        {
            return dt1.Day == dt2.Day && dt1.Month == dt2.Month && dt1.Year == dt2.Year;
        }

        public bool Filter(object o)
        {
            if (!(o is TestVM))
            {
                return false;
            }

            if (!((o as TestVM).Title.ToLower().Contains(FilterString.ToLower())))
            {
                return false;
            }

            switch (FilterType)
            {
                case FilterType.All:
                    return true;
                case FilterType.Date:
                    return DateEquals((o as TestVM).Date, FilterDateTime);
                case FilterType.Scenario:
                    return SelectedScenario.Id == (o as TestVM).Scenario.Id;
                case FilterType.Subject:
                    return SelectedSubject.Id == (o as TestVM).Subject.Id;
                default:
                    return false;
            }
        }

        public ICommand DetailCommand
        {
            get
            {
                if (detailCmd == null)
                {
                    detailCmd = new RelayCommand((a) => ShowDetails());
                }
                return detailCmd;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (deleteCmd == null)
                {
                    deleteCmd = new RelayCommand((a) => Delete(a));
                }
                return deleteCmd;
            }
        }

        public void Delete(object param)
        {
            if (!SelectedTest.Delete())
            {
                DialogHelper.ShowErrorDialog("Löschen Fehlgeschlagen!");
            }

            LoadTests();
        }

        public void ShowDetails()
        {
            if (SelectedTest != null)
            {
                MainViewModel.Instance.PushViewModel(new AnalysisViewModel(SelectedTest.Id));
            }
            else
            {
                DialogHelper.ShowErrorDialog("Bitte eine wählen Sie aus!");
            }
        }
    }
}