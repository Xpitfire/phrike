using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using NLog;
using Phrike.GroundControl.Annotations;
using Phrike.GroundControl.Controller;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using DataAccess;
using DataModel;

namespace Phrike.GroundControl.ViewModels
{
    public class UserSelectViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public string Filter { get; set; }

        public CollectionViewSource Source = new CollectionViewSource();

        public UserSelectViewModel()
        {
         //   List<DataModel.Subject> Subjects;
            using (var x = new UnitOfWork())
            {
                Subjects = x.SubjectRepository.Get().ToList();
            }

            //foreach (var subj in Subjects)
            //{
            //    //var child = BuildChildItem(subj);
            //    if (subj.AvatarPath == null) subj.AvatarPath = @"C:\public\user.png";
            //    Subjects.Add(subj);
            //}

            Source.Source = Subjects;
            Source.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
            Source.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
            Source.GroupDescriptions.Add(new PropertyGroupDescription("LastName[0]"));
        }

        private bool FilterSubjects(object o)
        {
            return Filter == null ? true : o is Subject ? ((Subject)o).LastName.Contains(Filter) : false;
        }

        public List<DataModel.Subject> Subjects { get; set; }
    }
}