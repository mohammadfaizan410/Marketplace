using DataAccess.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class User : Record
    {

        [Required]
        [StringLength(10)]
        public string UserName { get; set; }

        [Required]
        [StringLength(8)]
        public string Password { get; set; }

        public Role Role { get; set; }

        // tables one to many relationship 
        public int RoleId { get; set; }

        public List<Product> Products { get; set; }
    }
}
