using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class SurveyQuestion : BaseEntity
    {
        public Survey Survey { get; set; }
        public virtual Collection<SurveyResult> SurveyResults { get; set; }

        [Required]
        public string Question { get; set; }
    }
}
