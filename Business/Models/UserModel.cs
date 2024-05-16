using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Records;

namespace Business.Models
{

    public class UserModel : Record
    {
        #region Properties copied from the related entity
        [DisplayName("User Name")] 
        [Required(ErrorMessage = "{0} is required!")] 
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }
        [DisplayName("Role")]
        [Required(ErrorMessage = "{0} is required!")]
        public int? RoleId { get; set; }
        #endregion
        public string? RoleOutput { get; set; }
    }
}
