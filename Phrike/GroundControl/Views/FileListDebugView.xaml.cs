using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DataAccess;

using DataModel;

using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl {
    /// <summary>
    /// Interaction logic for FileListDebugView.xaml
    /// </summary>
    public partial class FileListDebugView : UserControl {
        public FileListDebugView()
        {
            InitializeComponent();
            GotFocus += (s, e) =>
            {
                using (var db = new UnitOfWork())
                {
                    DataContext =
                        new AuxiliaryDataListViewModel(
                            db.TestRepository.Get(includeProperties: nameof(AuxilaryData)).FirstOrDefault());
                }
            };
        }
    }
}
