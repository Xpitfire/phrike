using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DataModel
{
    public class Scenario : BaseEntity
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string ExecutionPath { get; set; }
        public string MinimapPath { get; set; }
        public double ZeroX { get; set; }
        public double ZeroY { get; set; }
        public double Scale { get; set; }


        public virtual Collection<Test> Tests { get; set; }
    }
}