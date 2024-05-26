using DataAccess.Records;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
   public class ProductModel : Record
    {
        [Required]
        [StringLength(10)]
        public string ProductName { get; set; }

        [Required]
        [StringLength(20)]
        public string ProductDescription { get; set; }

        [DisplayName("Categories")]
        [Required(ErrorMessage = "At least one category must be selected!")]
        public List<int>? CategoryIdsInput { get; set; }

        [DisplayName("Categories")]
        public List<String>? CategoryNamesOutput { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        public string PriceOutput { get; set; }

        [Required(ErrorMessage = "Please enter a value for the stock amount!")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock amount must be greater than 0.")]
        public int StockAmount { get; set; }

        public string Availibility {  get; set; } 

        [Required]
        public int UserId { get; set; }

    }
}

