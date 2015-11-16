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
using DataAccess;

namespace Phrike.GroundControl.ViewModels
{
    public class UserSelectViewModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public UserSelectViewModel()
        {
            using (var x = new UnitOfWork())
            {
                this.Subjects = x.SubjectRepository.Get().ToList();
            }
        }

        public List<DataModel.Subject> Subjects { get; set; }
    }
}