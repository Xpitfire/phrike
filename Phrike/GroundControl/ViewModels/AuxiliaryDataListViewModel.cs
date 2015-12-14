// <summary></summary>
// -----------------------------------------------------------------------
// Copyright (c) 2015 University of Applied Sciences Upper-Austria
// Project OperationPhrike
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using DataModel;

using Microsoft.Win32;

using Phrike.GroundControl.Helper;

namespace Phrike.GroundControl.ViewModels
{
    public class AuxiliaryDataListViewModel
    {
        private readonly Test parentTest;

        private ICommand addFileCmd;

        private ICommand deleteFileCmd;

        public AuxiliaryDataListViewModel(Test parentTest)
        {
            this.parentTest = parentTest;
            AuxiliaryData = new ObservableCollection<AuxiliaryDataViewModel>(
                parentTest.AuxilaryData.Select(d => new AuxiliaryDataViewModel(d)));
        }

        public ObservableCollection<AuxiliaryDataViewModel> AuxiliaryData { get; }

        public ICommand DeleteFile 
            => deleteFileCmd ?? (deleteFileCmd = new RelayCommand(DoDeleteFile));
        public ICommand AddFile
            => addFileCmd ?? (addFileCmd = new RelayCommand(DoAddFile));

        private void DoDeleteFile(object rawAuxVm)
        {
            var auxVm = (AuxiliaryDataViewModel)rawAuxVm;
            FileStorageHelper.DeleteFile(auxVm.Model.Id);
            AuxiliaryData.Remove(auxVm);
        }

        private void DoAddFile(object obj)
        {
            string filterString = string.Join(
                ";",
                AuxiliaryDataMimeTypes.FileExtMimeTypes.Keys.Select(e => "*." + e));
            var dlg = new OpenFileDialog
            {
                Filter = "Video oder Sensoraufzeichung|" + filterString
            };
            if (dlg.ShowDialog() != true)
            {
                return;
            }

            AuxilaryData data = FileStorageHelper.ImportFile(
                dlg.FileName,
                AuxiliaryDataMimeTypes.GetMimeTypeForPath(dlg.FileName),
                parentTest.Id);

            AuxiliaryData.Add(new AuxiliaryDataViewModel(data));
        }
    }

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