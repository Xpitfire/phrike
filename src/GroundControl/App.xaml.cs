using NLog;
using NLog.Targets;
using Phrike.GroundControl.Helper;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace Phrike.GroundControl
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var target = (FileTarget)LogManager.Configuration.FindTargetByName("phrikeFileLogger");
            target.FileName = Path.Combine(PathHelper.PhrikeLog, "GroundControl_${shortdate}.log");
            LogManager.ReconfigExistingLoggers();


            AppDomain.CurrentDomain.SetData("DataDirectory", PathHelper.PhrikeData);


            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}
