using DataModel;
using Phrike.GroundControl.ViewModels;
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

namespace Phrike.GroundControl.Views
{
    /// <summary>
    /// Interaction logic for ScenarioSelect.xaml
    /// </summary>
    public partial class ScenarioSelect : UserControl
    {
        public string Filter { get; set; }
     
        public ScenarioSelect()
        {
            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                this.DataContext = new ScenarioCollectionVM();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(spUser.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("Description", ListSortDirection.Ascending));
                view.GroupDescriptions.Add(new PropertyGroupDescription("Name[0]"));
                view.Filter = FilterScenarios;
            };
        }

        public bool FilterScenarios(object o)
        {
            return Filter == null ? true : o is ScenarioVM ? ((ScenarioVM)o).Name.ToLower().Contains(Filter) : false;
        }

        private void TbxSearch_OnKeyDown(object sender, TextChangedEventArgs e)
        {
            this.Filter = tbxSearch.Text.ToLower();
            CollectionViewSource.GetDefaultView(spUser.ItemsSource).Refresh();
        }
    }
}
