using Loan_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Loan_Management
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
  
        public DbSet<Loan> Loans { get; set; }
      

    }
}
