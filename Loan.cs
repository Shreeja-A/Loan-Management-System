using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loan_Management.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("customer")]
        public string CustomerId { get; set; }

        public Customer customer { get; set; }

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

        [Display(Name = "Type of Loan")]
        public string LoanType { get; set; }

        [Display(Name = "Loan Amount")]
        public decimal LoanAmount { get; set; }

        [Display(Name ="Status")]
        public string status { get; set; }
    }
}
