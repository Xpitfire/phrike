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
using MahApps.Metro.Controls.Dialogs;

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for UserSelect.xaml
    /// </summary>
    public partial class UserSelect : UserControl
    {
        private SubjectCollectionVM context;
        private enum ViewState
        {
            Select,
            Add
        };
        private static ViewState state = ViewState.Select;


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
                this.DataContext = context = new SubjectCollectionVM();
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
            if (spUser.SelectedItem != null)
            { RaiseEvent(new RoutedEventArgs(UserSelect.UserSelectedEvent)); }
        }

        private void BtnAdd_OnClick(object sender, RoutedEventArgs e)
        {
            //grdSelect.IsEnabled = false;
            //grdSelect.Visibility = Visibility.Hidden;
            //ucAdd.Visibility = Visibility.Visible;
            //ucAdd.IsEnabled = true;
            //context.CurrentSubject.InsertsDone = false;

            grdSelect.Visibility = Visibility.Hidden;
            grdAdd.Visibility = Visibility.Visible;
            context.CurrentSubject.Flush();
            //spUser.SelectedItem = new SubjectVM();
        }

        private void BtnSubmit_OnClick(object sender, RoutedEventArgs e)
        {
            string message;
            if (context.CurrentSubject.Add(out message))
            {
                grdSelect.Visibility = Visibility.Visible;
                grdAdd.Visibility = Visibility.Hidden;
                ((SubjectCollectionVM)this.DataContext).LoadSubjects();
            }
            else
            {
                var x = message.Split('\n');
                string title = x[0];
                string msg = "";
                for (int i = 1; i < x.Length; i++) msg += $"{x[i]}\n";
                MainWindow.Instance.Dispatcher.Invoke(() => MainWindow.Instance.ShowMessageAsync(title, msg));
            }
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            //context.CurrentSubject = context.Subjects.FirstOrDefault();
            grdSelect.Visibility = Visibility.Visible;
            grdAdd.Visibility = Visibility.Hidden;
        }
    }
}
