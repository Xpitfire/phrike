using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Phrike.GroundControl.Helper
{
    static class DefaultDataProvider
    {
        private static string CreateResourceString(string file)
        {
            return $@"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{file}";
        }

        public static string PrepareDefaultSubjectIcon()
        {
            return CreateResourceString("Resources/user.png");
        }

        public static string PrepareDefaultScenarioIcon()
        {
            return CreateResourceString("Resources/scenario.png");
        }
    }
}
