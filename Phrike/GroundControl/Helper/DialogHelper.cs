using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Phrike.GroundControl.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace Phrike.GroundControl.Helper
{
    public static class DialogHelper
    {
        public static void ShowErrorDialog(string message)
        {
            ShowErrorDialogAsync(message);
        }

        public static void ShowDialogAsync(string title, string message, MessageDialogStyle dialogStyle = MessageDialogStyle.Affirmative)
        {
            ShowDialog(title, message, dialogStyle);
        }



        public static void ShowErrorDialogAsync(string message)
        {
            ShowDialog("Fehler", message, MessageDialogStyle.Affirmative);
        }

        private static Task<MessageDialogResult> ShowDialog(string title, string message, MessageDialogStyle dialogStyle)
        {
            //if (Application.Current.)

            Task<MessageDialogResult> messageDialogResult = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var metroWindow = (Application.Current.MainWindow as MetroWindow);
                metroWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
                messageDialogResult = metroWindow.ShowMessageAsync(title, message, dialogStyle, metroWindow.MetroDialogOptions);
            });
            return messageDialogResult;
        }
    }
}
