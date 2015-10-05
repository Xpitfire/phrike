using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Video : BaseEntity
    {
        public DateTime Timestamp { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }
}
