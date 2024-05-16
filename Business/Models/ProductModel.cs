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

        [Required]
        public int UserId { get; set; }

    }
}

