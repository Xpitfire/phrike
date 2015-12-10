using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Phrike.GroundControl.ViewModels
{
    class TestResultViewModel : INotifyPropertyChanged
    {
        //Initialize FilterDateTime with a default value
        private DateTime _filterDateTime = DateTime.Now;

        private List<SubjectVM> _subjectList = new List<SubjectVM>();

        public TestResultViewModel()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var subjects = unitOfWork.SubjectRepository.Get();
                _subjectList.Clear();
                _subjectList.AddRange(subjects.Select(x => new SubjectVM(x)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubjectList)));
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

        public List<SubjectVM> SubjectList
        {
            get { return _subjectList; }
            set
            {
                _subjectList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubjectList)));
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
