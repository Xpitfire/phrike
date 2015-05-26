using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Phrike.GroundControl.Commands
{
    public class MenuStartCommand
    {

        private static RoutedUICommand _start;

        public static RoutedUICommand Start {
            get
            {
                return _start; 
                
            }
        }

        static MenuStartCommand()
        {
            _start = new RoutedUICommand("Start Command", "Start", typeof(MenuStartCommand));
        }

    }
}
