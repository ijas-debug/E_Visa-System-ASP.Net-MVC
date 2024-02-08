using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace FinalProject.Models
{
    public class VisaApplication
    {

        [Key]
        public int ApplicationID { get; set; }


        [Required(ErrorMessage = "Enter First Name!")]
        [Display(Name = "First name:")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Enter last name!")]
        [Display(Name = "Last name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Select date of birth!")]
        [Display(Name = "Date of birth:")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        


        [Required(ErrorMessage = "Please enter the email address !")] 
        [Display(Name = "Email id:")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address format.")]
        public string EmailID { get; set; }


        

        [Required(ErrorMessage = "Enter Phone Number!")]
        [Display(Name = "Phone number:")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Enter Address!")]
        [Display(Name = "Address:")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Select Expected Date of Arrival!")]
        [Display(Name = "Expected date of arrival:")]
        [DataType(DataType.Date)]
        public DateTime ExpectedDateOfArrival { get; set; }

        [Required(ErrorMessage = "Select Expected Date of Departure!")]
        [Display(Name = "Expected date of departure:")]
        [DataType(DataType.Date)]
        public DateTime ExpectedDateOfDeparture { get; set; }


        [Required(ErrorMessage = "Select Visa Service!")]
        [Display(Name = "Visa service:")]
        public string VisaService { get; set; }


        [Required(ErrorMessage = "Select Gender!")]
        [Display(Name = "Gender:")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Enter Town/City of Birth!")]
        [Display(Name = "Town/City of birth:")]
        public string TownCityOfBirth { get; set; }


        [Required(ErrorMessage = "Enter Country of Birth!")]
        [Display(Name = "Country of birth:")]
        public string CountryOfBirth { get; set; }


        [Required(ErrorMessage = "Enter Citizenship/National ID No.!")]
        [Display(Name = "Citizenship/National id no.:")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
        public string CitizenshipNationalIdNo { get; set; }


        [Required(ErrorMessage = "Enter Religion!")]
        [Display(Name = "Religion:")]
        public string Religion { get; set; }


        [Required(ErrorMessage = "Enter Educational Qualification!")]
        [Display(Name = "Educational qualification:")]
        public string EducationalQualification { get; set; }


        [Required(ErrorMessage = "Enter Passport Type!")]
        [Display(Name = "Passport type:")]
        public string PassportType { get; set; }


        [Required(ErrorMessage = "Enter Nationality!")]
        [Display(Name = "Nationality:")]
        public string Nationality { get; set; }


        [Required(ErrorMessage = "Enter Passport Number!")]
        [Display(Name = "Passport number:")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
        public string PassportNumber { get; set; }


        [Required(ErrorMessage = "Enter Place of Issue!")]
        [Display(Name = "Place of issue:")]
        public string PlaceOfIssue { get; set; }


        [Required(ErrorMessage = "Select Date of Issue!")]
        [Display(Name = "Date of issue:")]
        [DataType(DataType.Date)]
        public DateTime DateOfIssue { get; set; }

        [Required(ErrorMessage = "Select Date of Expiry!")]
        [Display(Name = "Date of expiry:")]
        [DataType(DataType.Date)]
        public DateTime DateOfExpiry { get; set; }

        

        [Display(Name = "Passport or identity certificate number:")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
        public string PassportOrICNo { get; set; }

        [Display(Name = "Port of arrival:")]
        public string PortOfArrival { get; set; }

        [Display(Name = "Reference name in India:")]
        public string ReferenceNameInIndia { get; set; }

        [Display(Name = "Reference address in India:")]
        public string ReferenceAddressInIndia { get; set; }

        [Display(Name = "Reference phone:")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "The field must contain exactly 10 numbers.")]
        public string ReferencePhone { get; set; }

        [Required(ErrorMessage = "Upload Photo")]
        [Display(Name = "Photo :")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "jpg,jpeg,gif", ErrorMessage = "Only JPEG and GIF images are allowed.")]
        public string Photo { get; set; }


        
        public string Status { get; set; }

       

    }
}
