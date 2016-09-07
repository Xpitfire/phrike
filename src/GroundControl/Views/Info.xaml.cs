using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phrike.GroundControl.Views
{
    class Developer : IComparable<Developer>
    {
        public string Name { get; set; }
        public string Avatar { get; set; }

        public int CompareTo(Developer obj)
        {
            return this.Name.CompareTo(obj.Name);
        }
    }

    class Designer : Developer
    {
    }

    class DataSource
    {
        public List<Developer> devs
        {
            get
            {
                var ret = new List<Developer>();

                ret.Add(new Developer() { Avatar = "../Images/dinu_flat_small.png", Name = "Marius-Constantin Dinu" });
                ret.Add(new Developer() { Avatar = "../Images/frauscher_flat_small.png", Name = "Josef Frauscher" });
                ret.Add(new Developer() { Avatar = "../Images/juttla_flat_small.png", Name = "Gurpret Juttla" });
                ret.Add(new Developer() { Avatar = "../Images/kohlendorfer_flat_small.png", Name = "Roland Kohlendorfer" });
                ret.Add(new Developer() { Avatar = "../Images/neumair_flat_small.png", Name = "Tom Neumair" });
                ret.Add(new Developer() { Avatar = "../Images/neumüller_flat_small.png", Name = "Christian Neumüller" });
                ret.Add(new Developer() { Avatar = "../Images/pham_flat_small.png", Name = "Christian Pham" });
                ret.Add(new Developer() { Avatar = "../Images/sandra_flat_small.png", Name = "Sandra Horner" });
                ret.Add(new Developer() { Avatar = "../Images/steinke_flat_small.png", Name = "Alexander Steinke" });
                ret.Add(new Developer() { Avatar = "../Images/wollvieh_flat_small.png", Name = "Wolfgang Mayr" });

                ret.Sort();
                return ret;
            }
        }

        public List<Designer> des
        {
            get
            {
                var ret = new List<Designer>();

                ret.Add(new Designer() { Avatar = "../Images/hahn_flat_small.png", Name = "Andreas Hahn" });
                ret.Add(new Designer() { Avatar = "../Images/hamm_flat_small.png", Name = "Michael Hammerer" });
                ret.Add(new Designer() { Avatar = "../Images/schr_flat_small.png", Name = "Rafael Schrenk" });

                ret.Sort();
                return ret;
            }
        }

        public List<Developer> prof
        {
            get
            {
                var ret = new List<Developer>();
                ret.Add(new Developer() { Avatar = "../Images/prof_flat_small.png", Name = "Prof. Gerald Zwettler" });
                return ret;
            }
        }

    }

    /// <summary>
    /// Interaction logic for Info.xaml
    /// </summary>
    public partial class Info : UserControl
    {
        public Info()
        {
            InitializeComponent();

            this.Loaded += (sender, args) => this.DataContext = new DataSource();
        }
    }
}
