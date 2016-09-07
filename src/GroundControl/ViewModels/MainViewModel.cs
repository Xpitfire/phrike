// <summary></summary>
// -----------------------------------------------------------------------
// Copyright (c) 2016 University of Applied Sciences Upper-Austria
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private static MainViewModel instance;

        private readonly Stack<object> viewStack = new Stack<object>();

        private object currentViewModel = AppOverviewViewModel.Instance;

        private MainViewModel()
        {
            PushViewModel(AppOverviewViewModel.Instance);
        }

        public static MainViewModel Instance
            => instance ?? (instance = new MainViewModel());

        public object CurrentViewModel
        {
            get { return this.currentViewModel; }
            private set
            {
                if (Equals(value, this.currentViewModel))
                {
                    return;
                }
                this.currentViewModel = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void PushViewModel(object vm)
        {
            viewStack.Push(vm);
            CurrentViewModel = vm;
        }

        public void PopCurrentViewModel()
        {
            if (viewStack.Any())
            {
                viewStack.Pop();
            }
            CurrentViewModel = viewStack.Any()
                                   ? viewStack.Peek()
                                   : AppOverviewViewModel.Instance;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}