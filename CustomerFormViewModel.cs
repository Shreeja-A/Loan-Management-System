using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Loan_Management.ViewModels
{
    public class CustomerFormViewModel
    {
        public int LoanId { get; set; }
        public string CustomerId { get; set; }

        [MinLength(4)]
        [MaxLength(50)]
        [Required(ErrorMessage = "Name should range between 4 and 50 letters")]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }

        public string EmailId { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[789]\d{9}$", ErrorMessage = "Please enter a valid Contact Number")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "Please enter a valid Aadhar number")]
        [Display(Name = "Aadhar Number")]
        public long Aadhar_number { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Please enter a valid PAN Number")]
        [Display(Name ="Permanent Account Number")]
        public string Pan_number { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }

        
        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a valid PinCode")]
        public int Pincode { get; set; }

        [Required]
        [Display(Name = "Type of Collateral")]
        public string CollateralType { get; set; }

       
        [Display(Name = "FD Account Number")]
        public int Fd_AccountNumber { get; set; }

       
        [Display(Name = "Name of the Bank")]
        public string Fd_BankName { get; set; }

        [Display(Name = "Amount Of FD")]
        //[Range(0.01, 999999999, ErrorMessage = "Amount must be greater than 0")]
        public decimal FdAmount { get; set; }

        
        [Display(Name = "Address of the property")]
        public string Property_Address { get; set; }

        
        //[Range(0.01, 999999999, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Evaluation of the property")]
        public decimal Property_Evaluation { get; set; }

        [Required]
        [Display(Name = "Type of Loan")]
        public string LoanType { get; set; }

        [Required]
        [Range(0.01, 999999999, ErrorMessage = "Amount must be greater than 0")]
        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }

        public List <HttpPostedFileBase> Files { get; set; }



    }
}
