using Phrike.GroundControl.Helper;
using System;
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
            AppDomain.CurrentDomain.SetData("DataDirectory", PathHelper.PhrikeData);
        }
    }
}
