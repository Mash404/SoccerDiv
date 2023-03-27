using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SoccerDiv.Models
{
    public class TempCustomer
    {
        [Required(ErrorMessage = "Enter your name!!")]
        [Display(Name = "Name")]
        [StringLength(25, ErrorMessage = "Maximum 40 character")]
        public string Customer_Name { get; set; }

        [Required(ErrorMessage = "Enter your Email")]
        [EmailAddress]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Customer_Email { get; set; }

        [Required(ErrorMessage = "Enter your Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Customer_Password { get; set; }
    }
}