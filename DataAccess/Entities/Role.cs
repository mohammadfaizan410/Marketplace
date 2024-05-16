using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Records;
namespace DataAccess.Entities
{
    public class Role : Record
    {
        [Required]
        [StringLength(5, MinimumLength = 4)]
        public string Name { get; set; } 
        public List<User> Users { get; set; }
    }
}
