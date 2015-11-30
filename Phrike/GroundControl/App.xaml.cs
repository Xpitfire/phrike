using NLog;
using NLog.Targets;
using Phrike.GroundControl.Helper;
using System;
using System.IO;
using System.Windows;

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
        }
    }
}
