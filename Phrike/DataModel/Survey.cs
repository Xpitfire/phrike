using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Survey : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Collection<SurveyQuestion> Questions { get; set; }
    }
}
