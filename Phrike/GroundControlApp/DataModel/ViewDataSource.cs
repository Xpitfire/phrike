using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

// Das von dieser Datei definierte Datenmodell dient als repräsentatives Beispiel für ein Modell
// unterstützt.  Die gewählten Eigenschaftennamen stimmen mit Datenbindungen in den Standardelementvorlagen überein.
//
// Anwendungen können dieses Modell als Startpunkt verwenden und darauf aufbauen. Es kann jedoch auch komplett verworfen und
// durch ein anderes den Anforderungen entsprechendes Modell ersetzt werden Bei Verwendung dieses Modells verbessern Sie möglicherweise 
// Reaktionsfähigkeit durch Initiieren der Datenladeaufgabe im hinteren Code für App.xaml, wenn die App 
// zuerst gestartet wird.

namespace Phrike.GroundControl.DataModel
{
    /// <summary>
    /// Generisches Elementdatenmodell
    /// </summary>
    public class ViewDataItem
    {
        public ViewDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Content = content;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public string Content { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generisches Gruppendatenmodell
    /// </summary>
    public class ViewDataGroup
    {
        public ViewDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.Description = description;
            this.ImagePath = imagePath;
            this.Items = new ObservableCollection<ViewDataItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Description { get; private set; }
        public string ImagePath { get; private set; }
        public ObservableCollection<ViewDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Erstellt eine Auflistung von Gruppen und Elementen mit Inhalten, die aus einer statischen JSON-Datei gelesen werden.
    /// 
    /// ViewDataSource wird mit Daten initialisiert, die aus einer statischen JSON-Datei gelesen werden, die 
    /// Projekt.  Dadurch werden Beispieldaten zur Entwurfszeit und zur Laufzeit bereitgestellt.
    /// </summary>
    public sealed class ViewDataSource
    {
        private static ViewDataSource _viewDataSource = new ViewDataSource();

        private ObservableCollection<ViewDataGroup> _groups = new ObservableCollection<ViewDataGroup>();
        public ObservableCollection<ViewDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<ViewDataGroup>> GetGroupsAsync()
        {
            await _viewDataSource.GetSampleDataAsync();

            return _viewDataSource.Groups;
        }

        public static async Task<ViewDataGroup> GetGroupAsync(string uniqueId)
        {
            await _viewDataSource.GetSampleDataAsync();
            // Einfache lineare Suche ist bei kleinen DataSets akzeptabel.
            var matches = _viewDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<ViewDataItem> GetItemAsync(string uniqueId)
        {
            await _viewDataSource.GetSampleDataAsync();
            // Einfache lineare Suche ist bei kleinen DataSets akzeptabel.
            var matches = _viewDataSource.Groups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetSampleDataAsync()
        {
            if (this._groups.Count != 0)
                return;

            Uri dataUri = new Uri("ms-appx:///DataModel/ViewData.json");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Groups"].GetArray();

            foreach (JsonValue groupValue in jsonArray)
            {
                JsonObject groupObject = groupValue.GetObject();
                ViewDataGroup group = new ViewDataGroup(groupObject["UniqueId"].GetString(),
                                                            groupObject["Title"].GetString(),
                                                            groupObject["Subtitle"].GetString(),
                                                            groupObject["ImagePath"].GetString(),
                                                            groupObject["Description"].GetString());

                foreach (JsonValue itemValue in groupObject["Items"].GetArray())
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new ViewDataItem(itemObject["UniqueId"].GetString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["Subtitle"].GetString(),
                                                       itemObject["ImagePath"].GetString(),
                                                       itemObject["Description"].GetString(),
                                                       itemObject["Content"].GetString()));
                }
                this.Groups.Add(group);
            }
        }
    }
}