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
using Phrike.GroundControl.ViewModels;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for UserSelect.xaml
    /// </summary>
    public partial class UserSelect : UserControl
    {
        public static readonly RoutedEvent UserSelectedEvent = EventManager.RegisterRoutedEvent(
            "UserSelectedEvent",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(UserSelect));

        public event RoutedEventHandler UserSelected
        {
            add { AddHandler(UserSelectedEvent, value); }
            remove { RemoveHandler(UserSelectedEvent, value); }
        }

        public string Filter { get; set; }

        public UserSelect()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.DataContext = new SubjectCollectionVM();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(spUser.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
                view.GroupDescriptions.Add(new PropertyGroupDescription("LastName[0]"));
                view.Filter = FilterSubjects;
            };
        }

        private bool FilterSubjects(object o)
        {
            return Filter == null ? true : o is SubjectVM ? ((SubjectVM)o).LastName.ToLower().Contains(Filter) : false;
        }

        private void TbxSearch_OnKeyDown(object sender, TextChangedEventArgs e)
        {
            this.Filter = tbxSearch.Text.ToLower();
            CollectionViewSource.GetDefaultView(spUser.ItemsSource).Refresh();
        }

        private void SpUser_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(UserSelect.UserSelectedEvent));
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            grdSelect.IsEnabled = false;
            grdSelect.Visibility = Visibility.Hidden;
            ucAdd.Visibility = Visibility.Visible;
            ucAdd.IsEnabled = true;
        }
    }
}
