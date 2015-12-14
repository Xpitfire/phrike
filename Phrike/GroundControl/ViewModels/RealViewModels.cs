using DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DataAccess;
using OxyPlot;
using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    class SubjectCollectionVM : INotifyPropertyChanged
    {
        private SubjectVM currentSubject;
        public ObservableCollection<SubjectVM> Subjects { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public SubjectCollectionVM()
        {
            Subjects = new ObservableCollection<SubjectVM>();
            currentSubject = new SubjectVM();
            LoadSubjects();
        }


        public SubjectVM CurrentSubject
        {
            get { return this.currentSubject; }
            set
            {
                if (currentSubject != value)
                {
                    this.currentSubject = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSubject)));
                }
            }
        }

        public async void LoadSubjects()
        {
            Subjects.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Subject> enu = x.SubjectRepository.Get().GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Subjects.Add(new SubjectVM(enu.Current));
                }
            }
        }

    }


    public class SubjectVM : INotifyPropertyChanged
    {
        private Subject subject;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool insertsDone = true;

        public SubjectVM(Subject subject)
        {
            this.subject = subject;
        }

        public SubjectVM()
        {
            subject = new Subject();
        }

        internal bool Submit(out string message)
        {
            using (var x = new UnitOfWork())
            {
                try
                {
                    if (subject.Id == default(int))
                    {
                        string path = subject.AvatarPath;
                        subject.AvatarPath = null;
                        x.SubjectRepository.Insert(subject);
                        x.Save();

                        FileStorageHelper.SetSubjectAvatar(path, subject, x);
                    }
                    else
                    {
                        if (AvatarPathChanged)
                        {
                            string path = subject.AvatarPath;
                            subject.AvatarPath = null;
                            x.SubjectRepository.Update(subject);
                            x.Save();

                            FileStorageHelper.SetSubjectAvatar(path, subject, x);
                        }
                        else
                        {
                            x.SubjectRepository.Update(subject);
                            x.Save();
                        }
                    }
                    InsertsDone = true;
                    message = "";
                    return true;
                }
                catch (DbEntityValidationException ex)
                {
                    message = "Fehler beim erstellen eines neuen Benutzers:\n";
                    foreach (var error in ex.EntityValidationErrors.FirstOrDefault()?.ValidationErrors)
                    {
                        message += $"{error.ErrorMessage}\n";
                    }
                    return false;
                }
                catch (Exception end)
                {
                    message = "Unbekannter Fehler:\n";
                    message += end.Message;
                    return false;
                }
            }
        }

        internal void Cancel()
        {
            InsertsDone = true;
        }

        internal void Flush()
        {
            LastName = "";
            FirstName = "";
            DateOfBirth = DateTime.MinValue;
            Gender = default(Gender);
            CountryCode = "";
            City = "";
            PostalCode = "";
            Street = "";
            ServiceRank = "";
            Function = "";
            Conditions = "";
            BloodType = default(BloodType);
            RhFactor = default(RhFactor);
            AvatarPath = "";
        }

        public bool UseDefaultIcon { get { return String.IsNullOrEmpty(AvatarPath); } }

        public String ImagePath
        {
            get
            {
                if (UseDefaultIcon)
                {
                    return DefaultDataProvider.PrepareDefaultSubjectIcon();
                }
                else
                {
                    return System.IO.Path.Combine(PathHelper.PhrikePicture, subject.AvatarPath);
                }
            }
        }

        public String FullName
        {
            get { return $"{subject.ServiceRank} {subject.FirstName} {subject.LastName}"; }
        }

        public bool InsertsDone
        {
            get { return insertsDone; }
            set
            {
                if (insertsDone != value)
                {
                    insertsDone = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(InsertsDone)));
                }
            }
        }

        private bool AvatarPathChanged { get; set; }

        public IEnumerable<Gender> AvailableGenders => (Gender[])Enum.GetValues(typeof(Gender));
        public IEnumerable<String> AvailableCountries => (new List<string>() { "AT", "DE", "CH" });
        public IEnumerable<RhFactor> AvailableRhFactors => (RhFactor[])Enum.GetValues(typeof(RhFactor));
        public IEnumerable<BloodType> AvailableBloodTypes => (BloodType[])Enum.GetValues(typeof(BloodType));
        public IEnumerable<String> AvailableServiceRanks => (new List<string>() { "Rekrut", "Gefreiter", "Korporal", "Zugsführer", "Wachtmeister", "Oberwachtmeister", "Stabswachtmeister", "Oberstabswachtmeister", "Offiziersstellvertreter", "Vizeleutnant", "Fähnrich", "Leutnant", "Oberleutnant", "Hauptmann", "Major", "Oberstleutnant", "Oberst", "Brigardier", "Generalmajor", "Generalleutnant", "General" });
        #region Property Propagation

        public String LastName
        {
            get { return subject.LastName; }
            set
            {
                if (subject.LastName != value)
                {
                    subject.LastName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastName)));
                }
            }
        }
        public String FirstName
        {
            get { return subject.FirstName; }
            set
            {
                if (subject.FirstName != value)
                {
                    subject.FirstName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
                }
            }
        }
        public DateTime DateOfBirth
        {
            get { return subject.DateOfBirth; }
            set
            {
                if (value == DateTime.MinValue)
                {
                    subject.DateOfBirth = subject.DateOfBirth = DateTime.Today.Subtract(new TimeSpan(18 * 365, 0, 0, 0));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
                }
                else if (subject.DateOfBirth != value)
                {
                    subject.DateOfBirth = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateOfBirth)));
                }
            }
        }
        public Gender Gender
        {
            get { return subject.Gender; }
            set
            {
                if (subject.Gender != value)
                {
                    subject.Gender = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Gender)));
                }
            }
        }
        public string CountryCode
        {
            get { return subject.CountryCode; }
            set
            {
                if (subject.CountryCode != value)
                {
                    subject.CountryCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CountryCode)));
                }
            }
        }
        public string City
        {
            get { return subject.City; }
            set
            {
                if (subject.City != value)
                {
                    subject.City = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(City)));
                }
            }
        }
        public string PostalCode
        {
            get { return subject.PostalCode; }
            set
            {
                if (subject.PostalCode != value)
                {
                    subject.PostalCode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PostalCode)));
                }
            }
        }
        public string Street
        {
            get { return subject.Street; }
            set
            {
                if (subject.Street != value)
                {
                    subject.Street = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Street)));
                }
            }
        }
        public string ServiceRank
        {
            get { return subject.ServiceRank; }
            set
            {
                if (subject.ServiceRank != value)
                {
                    subject.ServiceRank = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServiceRank)));
                }
            }
        }
        public string Function
        {
            get { return subject.Function; }
            set
            {
                if (subject.Function != value)
                {
                    subject.Function = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Function)));
                }
            }
        }
        public string Conditions
        {
            get { return subject.Conditions; }
            set
            {
                if (subject.Conditions != value)
                {
                    subject.Conditions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Conditions)));
                }
            }
        }
        public BloodType BloodType
        {
            get { return subject.BloodType; }
            set
            {
                if (subject.BloodType != value)
                {
                    subject.BloodType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BloodType)));
                }
            }
        }
        public RhFactor RhFactor
        {
            get { return subject.RhFactor; }
            set
            {
                if (subject.RhFactor != value)
                {
                    subject.RhFactor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RhFactor)));
                }
            }
        }
        public String AvatarPath
        {
            get
            {
                //return String.IsNullOrEmpty(subject.AvatarPath) ? "" : System.IO.Path.Combine(PathHelper.PhrikePicture, subject.AvatarPath);
                return subject.AvatarPath;
            }
            set
            {
                if (subject.AvatarPath != value)
                {
                    subject.AvatarPath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvatarPath)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
                    AvatarPathChanged = true;
                }
            }
        }

        #endregion
    }

    class ScenarioCollectionVM : INotifyPropertyChanged
    {
        private ScenarioVM currentScenario;
        public ObservableCollection<ScenarioVM> Scenarios { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public ScenarioCollectionVM()
        {
            this.Scenarios = new ObservableCollection<ScenarioVM>();

            LoadScenarios();
        }

        private async void LoadScenarios()
        {
            Scenarios.Clear();

            using (var x = new UnitOfWork())
            {
                IEnumerator<Scenario> enu = x.ScenarioRepository.Get().GetEnumerator();
                while (await Task.Factory.StartNew(() => enu.MoveNext()))
                {
                    Scenarios.Add(new ScenarioVM(enu.Current));
                }
            }
        }

        public ScenarioVM CurrentScenario
        {
            get { return this.currentScenario; }
            set
            {
                if (currentScenario != value)
                {
                    this.currentScenario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScenario)));
                }
            }
        }
    }

    class ScenarioVM : INotifyPropertyChanged
    {
        private Scenario scenario;
        public event PropertyChangedEventHandler PropertyChanged;

        public ScenarioVM(Scenario scenario)
        {
            this.scenario = scenario;
        }

        public String Icon
        {
            get
            {
                if (scenario.ThumbnailPath == null || scenario.ThumbnailPath == String.Empty)
                {
                    return DefaultDataProvider.PrepareDefaultScenarioIcon();
                }
                else
                {
                    return System.IO.Path.Combine(PathHelper.PhrikePicture, scenario.ThumbnailPath);
                }
            }
        }

        public String Name { get { return scenario.Name; } }

        public String Description { get { return scenario.Description; } }
    }

    class OverviewVM : INotifyPropertyChanged
    {
        private ScenarioVM currentScenario;
        private SubjectVM currentSubject;

        public event PropertyChangedEventHandler PropertyChanged;


        public ScenarioVM CurrentScenario
        {
            get { return currentScenario; }
            set
            {
                if (currentScenario != value)
                {
                    currentScenario = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentScenario)));
                }
            }
        }

        public SubjectVM CurrentSubject
        {
            get
            {
                return currentSubject;
            }
            set
            {
                if (currentSubject != value)
                {
                    currentSubject = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(currentSubject)));
                }
            }
        }
    }
}
