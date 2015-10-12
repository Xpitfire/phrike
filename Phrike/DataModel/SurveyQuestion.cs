using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DataModel
{
    public class SurveyQuestion : BaseEntity
    {
        public Survey Survey { get; set; }
        [Required]
        public string Question { get; set; }
    }
}
