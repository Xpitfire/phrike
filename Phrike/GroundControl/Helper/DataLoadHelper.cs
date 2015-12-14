using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Phrike.GroundControl.Helper
{
    public static class DataLoadHelper
    {
        public static bool IsLoadDataActive()
        {
#if DEBUG
            return !DesignerProperties.GetIsInDesignMode(new DependencyObject());
#else
            return true;
#endif
        }
    }
}
