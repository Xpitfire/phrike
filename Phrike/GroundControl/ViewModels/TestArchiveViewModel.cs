using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataModel;

namespace Phrike.GroundControl.ViewModels
{
    class TestArchiveViewModel : INotifyPropertyChanged
    {
        //Initialize FilterDateTime with a default value
        private DateTime _filterDateTime = DateTime.Now;

        private List<Subject> _subjectList = new List<Subject>();

        public TestArchiveViewModel()
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var subjects = unitOfWork.SubjectRepository.Get();
                SubjectList = subjects.ToList();
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

        public List<Subject> SubjectList
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
