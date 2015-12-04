// <summary>Implementation of the RelayCommand class.</summary>
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

using System;
using System.Windows.Input;

using Phrike.GroundControl.Annotations;

namespace Phrike.GroundControl
{
    /// <summary>
    /// An implementation of <see cref="ICommand"/> that invokes delegates as
    /// handlers for <see cref="Execute"/> and <see cref="CanExecute"/>
    /// </summary>
    /// <remarks>
    /// Adapted from http://stackoverflow.com/questions/1468791/wpf-icommand-mvvm-implementation.
    /// </remarks>
    public class RelayCommand : ICommand
    {
        public RelayCommand(
            [NotNull] Action<object> executeHandler,
            [CanBeNull] Predicate<object> canExecuteHandler = null)
        {
            if (executeHandler == null)
            {
                throw new ArgumentNullException(nameof(executeHandler));
            }
            ExecuteHandler = executeHandler;
            CanExecuteHandler = canExecuteHandler;
        }

        [NotNull]
        public Action<object> ExecuteHandler { get; set; }

        [CanBeNull]
        public Predicate<object> CanExecuteHandler { get; set; }

        public bool CanExecute(object parameter)
        {
            return CanExecuteHandler?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            ExecuteHandler(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}