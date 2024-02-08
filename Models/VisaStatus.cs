using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FinalProject.Models
{
    public class VisaStatus
    {
        [Required(ErrorMessage = "Enter Email Id!")]
        [Display(Name = "Email id:")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "Enter passport number!")]
        [Display(Name = "Passport number:")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
        public string PassportNumber { get; set; }
    }
}