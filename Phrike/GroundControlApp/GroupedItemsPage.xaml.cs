using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Phrike.GroundControl.Common;
using Phrike.GroundControl.DataModel;

// Die Elementvorlage für die Seite "Gruppierte Elemente" ist unter http://go.microsoft.com/fwlink/?LinkId=234231 dokumentiert.

namespace GroundControlApp
{
    /// <summary>
    /// Eine Seite, auf der eine gruppierte Auflistung von Elementen angezeigt wird.
    /// </summary>
    public sealed partial class GroupedItemsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// NavigationHelper wird auf jeder Seite zur Unterstützung bei der Navigation verwendet und 
        /// Verwaltung der Prozesslebensdauer
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Dies kann in ein stark typisiertes Anzeigemodell geändert werden.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public GroupedItemsPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Füllt die Seite mit Inhalt auf, der bei der Navigation übergeben wird.  Gespeicherte Zustände werden ebenfalls
        /// bereitgestellt, wenn eine Seite aus einer vorherigen Sitzung neu erstellt wird.
        /// </summary>
        /// <param name="sender">
        /// Die Quelle des Ereignisses, normalerweise <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Ereignisdaten, die die Navigationsparameter bereitstellen, die an
        /// <see cref="Frame.Navigate(Type, Object)"/> als diese Seite ursprünglich angefordert wurde und
        /// ein Wörterbuch des Zustands, der von dieser Seite während einer früheren
        /// beibehalten wurde.  Der Zustand ist beim ersten Aufrufen einer Seite NULL.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Ein geeignetes Datenmodell für die problematische Domäne erstellen, um die Beispieldaten auszutauschen
            var sampleDataGroups = await ViewDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Wird aufgerufen, wenn auf einen Gruppenkopf geklickt wird.
        /// </summary>
        /// <param name="sender">Die Schaltfläche, die als Gruppenkopf für die ausgewählte Gruppe verwendet wird.</param>
        /// <param name="e">Ereignisdaten, die beschreiben, wie der Klick initiiert wurde.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Ermitteln, welche Gruppe die Schaltflächeninstanz darstellt
            var group = (sender as FrameworkElement).DataContext;

            // Zur entsprechenden Zielseite navigieren und die neue Seite konfigurieren,
            // indem die erforderlichen Informationen als Navigationsparameter übergeben werden
            this.Frame.Navigate(typeof(GroupDetailPage), ((ViewDataGroup)group).UniqueId);
        }

        /// <summary>
        /// Wird aufgerufen, wenn auf ein Element innerhalb einer Gruppe geklickt wird.
        /// </summary>
        /// <param name="sender">GridView (oder ListView, wenn die Anwendung angedockt ist),
        /// die das angeklickte Element anzeigt.</param>
        /// <param name="e">Ereignisdaten, die das angeklickte Element beschreiben.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Zur entsprechenden Zielseite navigieren und die neue Seite konfigurieren,
            // indem die erforderlichen Informationen als Navigationsparameter übergeben werden
            var itemId = ((ViewDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        #region NavigationHelper-Registrierung

        /// Die in diesem Abschnitt bereitgestellten Methoden werden einfach verwendet, um
        /// damit NavigationHelper auf die Navigationsmethoden der Seite reagieren kann.
        /// 
        /// Platzieren Sie seitenspezifische Logik in Ereignishandlern für  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// und <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// Der Navigationsparameter ist in der LoadState-Methode verfügbar 
        /// zusätzlich zum Seitenzustand, der während einer früheren Sitzung beibehalten wurde.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}