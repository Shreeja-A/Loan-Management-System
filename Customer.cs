using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loan_Management.Models
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Key]
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

        [Required(ErrorMessage = "Please enter a valid Date of birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime Dob { get; set; }

        public string Address { get; set; }

        public int Pincode { get; set; }

        public string AadharUpload { get; set; }

        public string PanUpload { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[a-zA-Z]).{6,}$", ErrorMessage = "The Password should be of minimum 6 letters with special characters included")]
        public string Password { get; set; }

        [Display(Name ="Type of Collateral")]
        public string CollateralType { get; set; }

        [Display(Name = "FD Account Number")]
        public int Fd_AccountNumber { get; set; }

        [Display(Name = "Name of the Bank")]
        public string Fd_BankName { get; set; }

        [Display(Name = "Amount Of FD")]
        public decimal FdAmount { get; set; }

        [Display(Name = "Address of the property")]
        public string Property_Address { get; set; }

        [Display(Name = "Evaluation of the property")]
        public decimal Property_Evaluation { get; set; }

        [Display(Name ="Type of Loan")]
        public string LoanType { get; set; }

        [Display(Name ="Loan Amount")]
        public decimal LoanAmount { get; set; }


        public string status { get; set; }

       // public List<HttpPostedFileBase> Files { get; set; }
    }
}
