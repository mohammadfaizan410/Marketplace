using DataAccess.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Category:Record
    {
        [Required]
        [StringLength(30)]
        public string CategoryName { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
