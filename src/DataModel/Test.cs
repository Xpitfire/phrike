using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Test : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public Subject Subject { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string Location { get; set; }
        public string Notes { get; set; }

        public ICollection<PositionData> PositionData { get; set; }
        public ICollection<AuxilaryData> AuxilaryData { get; set; }
        public ICollection<SurveyResult> SurveyResult { get; set; }
        [Required]
        public Scenario Scenario { get; set; }

        public Test()
        {
            this.PositionData = new List<PositionData>();
            this.AuxilaryData = new List<AuxilaryData>();
            this.SurveyResult = new List<SurveyResult>();
        }
    }
}
