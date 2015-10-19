using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Subject : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Conditions { get; set; }
        public BloodType BloodType { get; set; }
        public RhFactor RhFactor { get; set; }
        
        public string CountryCode { get; set; }

        public virtual Collection<Test> Tests { get; set; }
    }

    public enum RhFactor
    {
        Positive,
        Negative
    }

    public enum BloodType
    {
        A,
        B,
        AB,
        Zero
    }

    public enum Gender
    {
        Male,
        Female
    }
}
