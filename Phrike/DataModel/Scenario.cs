using System.Collections.ObjectModel;

namespace DataModel
{
    public class Scenario : BaseEntity
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string ExecutionPath { get; set; }
        public string MinimapPath { get; set; }
        public object MinimapLocationRatio { get; set; }


        public virtual Collection<Test> Tests { get; set; }
    }
}