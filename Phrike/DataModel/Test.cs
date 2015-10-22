using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Test : BaseEntity
    {
        public string Title { get; set; }
        public Subject Subject { get; set; }
        public DateTime Time { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<PositionData> PositionData { get; set; }
        public virtual ICollection<Video> Video { get; set; }
        public virtual ICollection<SurveyResult> SurveyResult { get; set; }
        public Scenario Scenario { get; set; }

        public Test()
        {
            this.PositionData = new List<PositionData>();
            this.Video = new List<Video>();
            this.SurveyResult = new List<SurveyResult>();
        }
    }
}
