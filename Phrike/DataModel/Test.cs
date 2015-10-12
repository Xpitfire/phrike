using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Test : BaseEntity
    {
        public Propositus Propositus { get; set; }
        public virtual ICollection<PositionData> PositionData { get; set; }
        public virtual ICollection<Video> Video { get; set; }
        public virtual ICollection<SurveyResult> SurveyResult { get; set; }
        public string Scenario { get; set; }
    }
}
