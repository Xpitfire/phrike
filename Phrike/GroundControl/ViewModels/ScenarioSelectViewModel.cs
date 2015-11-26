using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Phrike.GroundControl.ViewModels
{
    class ScenarioSelectViewModel
    {
        public List<Scenario> Scenarios { get; set; }
        public string Filter { get; set; }

        public ScenarioSelectViewModel()
        {
            using (var x = new UnitOfWork())
            {
                this.Scenarios = x.ScenarioRepository.Get().ToList();
            }
        }

        public bool FilterScenarios(object o)
        {
            return Filter == null ? true : o is Scenario ? ((Scenario)o).Name.Contains(Filter) : false;
        }
    }
}
