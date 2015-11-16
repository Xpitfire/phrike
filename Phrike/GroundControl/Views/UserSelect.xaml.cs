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
using DataModel;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for UserSelect.xaml
    /// </summary>
    public partial class UserSelect : UserControl
    {
        public List<Subject> Subjects { get; private set; }

        public UserSelect()
        {
            InitializeComponent();
            var x = (ViewModels.UserSelectViewModel) this.FindResource("UserSelectViewModel");
            this.Subjects= new List<Subject>();
            foreach (var subj in x.Subjects)
            {
                //var child = BuildChildItem(subj);

                this.Subjects.Add(subj);
            }
        }

        private UIElement BuildChildItem(Subject subj)
        {
            StackPanel stp = new StackPanel();
            stp.Orientation = Orientation.Vertical;

            Label lbl = new Label();
            lbl.Content = $"{subj.FirstName} {subj.LastName}";

            stp.Children.Add(lbl);

            Image img = new Image();
            img.Source = new BitmapImage(new Uri(@"C:\OperationPhrike\Git\Phrike\GroundControl\bin\Debug\user1.png", UriKind.Absolute));
            stp.Children.Add(img);

            return stp;
        }
    }
}
