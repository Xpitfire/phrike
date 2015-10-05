using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class PositionData : BaseEntity
    {
        public DateTime Time { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }


        // TODO: RIchtig anordnen
        public float Pitch { get; set; }
        public float Yaw { get; set; }
        public float Roll { get; set; }
    }
}
