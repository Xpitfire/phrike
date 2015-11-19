using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        //public ICollectionView ItemsView { get; set; }
        public string Filter { get; set; }

        public UserSelect()
        {
            InitializeComponent();
            var x = (ViewModels.UserSelectViewModel) this.FindResource("UserSelectViewModel");
            this.Subjects= new List<Subject>();

            foreach (var subj in x.Subjects)
            {
                //var child = BuildChildItem(subj);
                if (subj.AvatarPath == null) subj.AvatarPath = @"C:\public\user.png";
                this.Subjects.Add(subj);
            }
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(spUser.ItemsSource);
            view.Filter = FilterSubjects;
        }

        private bool FilterSubjects(object o)
        {
            return Filter == null ? true : o is Subject ? ((Subject)o).LastName.Contains(Filter) : false;
        }

        private void TbxSearch_OnKeyDown(object sender, TextChangedEventArgs e)
        {
            this.Filter = tbxSearch.Text;
            CollectionViewSource.GetDefaultView(spUser.ItemsSource).Refresh();
        }
    }
}
