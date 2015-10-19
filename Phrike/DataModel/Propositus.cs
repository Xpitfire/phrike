using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Propositus : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Sex Sex { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}
