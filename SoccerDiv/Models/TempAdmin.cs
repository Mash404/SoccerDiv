using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoccerDiv.Models
{
    public class TempAdmin
    {
        [Required(ErrorMessage = "Enter admin name!!")]
        [Display(Name = "Admin Name")]
        public string Admin_Name { get; set; }

        [Required(ErrorMessage = "Enter admin email!!")]
        [EmailAddress]
        [Display(Name = "Admin Email")]
        [DataType(DataType.EmailAddress)]
        public string Admin_Email { get; set; }

        [Required(ErrorMessage = "Enter admin password!!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Admin_Password { get; set; }
    }
}