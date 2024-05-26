using DataAccess.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Product : Record
    {
        [Required]
        [StringLength(10)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string ProductDescription { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public int StockAmount {  get; set; }

        public User User { get; set; }

        //has one to many relation user being the one side
        public int Userid { get; set; }

        public Product()
        {
            ProductCategories = new List<ProductCategory>();
        }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
