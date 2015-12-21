using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows;

namespace Phrike.GroundControl.Helper
{
    public class DialogHelper
    {
        public async void ShowDialogAsync(string title, string message, MessageDialogStyle dialogStyle = MessageDialogStyle.Affirmative)
        {
            await ShowDialog(title, message, dialogStyle);
        }

        private async Task<MessageDialogResult> ShowDialog(string title, string message, MessageDialogStyle dialogStyle)
        {
            var metroWindow = (Application.Current.MainWindow as MetroWindow);
            metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            return await metroWindow.ShowMessageAsync(title, message, dialogStyle, metroWindow.MetroDialogOptions);
        }
    }
}
