using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Video : BaseEntity
    {
        [Required]
        public Test Test { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
        [Required]
        public string FilePath { get; set; }
        public string Description { get; set; }
    }
}
