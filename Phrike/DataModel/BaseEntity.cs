using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel
{
    public abstract class BaseEntity
    {
        [Key]
        [Required]
        public int Id { get; set; }
    }
}
