using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class Scenario : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string Version { get; set; }
        [Required]
        public string ExecutionPath { get; set; }
        [Required]
        public string MinimapPath { get; set; }
        [Required]
        public double ZeroX { get; set; }
        [Required]
        public double ZeroY { get; set; }
        [Required]
        public double Scale { get; set; }


        public virtual Collection<Test> Tests { get; set; }
    }
}