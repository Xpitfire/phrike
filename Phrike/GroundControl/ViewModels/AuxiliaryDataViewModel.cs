using DataModel;

namespace Phrike.GroundControl.ViewModels
{
    public class AuxiliaryDataViewModel
    {
        public AuxiliaryDataViewModel(AuxilaryData model)
        {
            this.Model = model;
        }

        public string DisplayName
            =>
                string.IsNullOrEmpty(Model.Description)
                    ? Model.FilePath
                    : Model.Description;

        public string FullInfo => "Datei: " + Model.FilePath + "\nTyp: " + Model.MimeType;

        public string CategoryName
            =>
                AuxiliaryDataMimeTypes.GetCategory(Model.MimeType)
                == AuxiliaryDataMimeTypes.Category.Video
                    ? "Video"
                    : "Sensoraufzeichnung";

        public AuxilaryData Model { get; }
    }
}