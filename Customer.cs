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

        public long Aadhar_number{get;set;}

        public string Pan_number { get; set; }
        public string Address { get; set; }

        public string City { get; set; }

        public int Pincode { get; set; }

        public string AadharUpload { get; set; }

        public string PanUpload { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[a-zA-Z]).{6,}$", ErrorMessage = "The Password should be of minimum 6 letters with special characters included")]
        public string Password { get; set; }

       

       // public List<HttpPostedFileBase> Files { get; set; }
    }
}
