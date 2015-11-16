using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public class Subject : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string ServiceRank { get; set; }
        public string Function { get; set; }
        public string Conditions { get; set; }
        public BloodType BloodType { get; set; }
        public RhFactor RhFactor { get; set; }
        public string AvatarPath { get; set; }

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
