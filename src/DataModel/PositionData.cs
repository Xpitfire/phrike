using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class PositionData : BaseEntity
    {
        [Required]
        public Test Test { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public float X { get; set; }
        [Required]
        public float Y { get; set; }
        [Required]
        public float Z { get; set; }


        [Required]
        public float Roll { get; set; }
        [Required]
        public float Pitch { get; set; }
        [Required]
        public float Yaw { get; set; }
    }
}
