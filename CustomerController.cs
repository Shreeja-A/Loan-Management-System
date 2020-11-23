using Loan_Management.Models;
using Loan_Management.ViewModels;
using Scrypt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Loan_Management.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        CustomerContext context = new CustomerContext();
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewmodel user)

        {
            if (ModelState.IsValid)
            {
                Customer cust = new Customer();
                cust = context.Customers.Where(t => t.EmailId == user.EmailId).FirstOrDefault();
                if (cust == null)
                {
                    ViewBag.Message = "Your details are submitted successfully";
                    Customer c = new Customer();
                    c.CustomerName = user.CustomerName;
                    var id = context.Customers.Max(u => u.Id);
                    id = id + 1;
                    string Id = "LMS" + DateTime.Now.Year + id.ToString();
                    c.CustomerId = Id;
                    c.EmailId = user.EmailId;
                    c.ContactNumber = user.ContactNumber;
                    ScryptEncoder scrypt = new ScryptEncoder();  
                    c.Password = scrypt.Encode(user.Password);
                    context.Customers.Add(c);
                    context.SaveChanges();
                    return RedirectToAction("Login", "Customer");
                }
                else
                {
                    ModelState.AddModelError("", "User already exists! ");
                    return View("Register");
                }
            }
            ModelState.AddModelError("", "Please update the Highlighted mandatory fields");
            return View("Register");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel user)

        {
            if (ModelState.IsValid)
            {
                ScryptEncoder encrpt = new ScryptEncoder();

                var currentUser = context.Customers.Where(t => t.EmailId.ToLower().Equals(user.EmailId.ToLower())).FirstOrDefault();

                if (currentUser != null && encrpt.Compare(user.Password, currentUser.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.EmailId, false);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email Id or Password");
                    return View();
                }

            };

            return View("Login");
        }

        [Authorize]
        [HttpGet]
        public ActionResult CustomerForm()
        {
            Customer c = new Customer();
            var id = User.Identity.Name;
            c = context.Customers.Where(t => t.EmailId == id).FirstOrDefault();
            Loan loanc = context.Loans.Where(t => t.CustomerId == c.CustomerId).FirstOrDefault();
            CustomerFormViewModel u = new CustomerFormViewModel();
            u.CustomerId = c.CustomerId;
            u.CustomerName = c.CustomerName;
            u.EmailId = c.EmailId;
            u.ContactNumber = c.ContactNumber;
            u.Address = c.Address;
            u.City = c.City;
            u.Aadhar_number = c.Aadhar_number;
            u.Pan_number = c.Pan_number;
            u.Pincode = c.Pincode;
            return View(u);
        }

        [HttpPost]
        public ActionResult CustomerForm(CustomerFormViewModel cust, string command)
        {
            if (command == "Submit")
            {
                Customer c = new Customer();
                c = context.Customers.Where(t => t.CustomerId == cust.CustomerId).FirstOrDefault();
                if (cust.Address != null && cust.City != null && cust.Pincode != 0 && cust.Aadhar_number != 0 && cust.Pan_number != null)
                {
                    c.City = cust.City;
                    c.Address = cust.Address;
                    c.Pincode = cust.Pincode;
                    c.Aadhar_number = cust.Aadhar_number;
                    c.Pan_number = cust.Pan_number;
                }
                else
                {
                    ModelState.AddModelError("", "Enter the Customer Basic Details");
                    return View();
                }
                Loan l = new Loan();
                l.status = "Submitted";
                l.CustomerId = cust.CustomerId;
                if (cust.CollateralType == null)
                {
                    ModelState.AddModelError("", "Enter the Collateral details");
                    return View();
                }
                else
                {
                    l.CollateralType = cust.CollateralType;
                    if (l.CollateralType == "Fixed Deposit")
                    {
                        if (cust.Property_Address != null || cust.Property_Evaluation != 0)
                        {
                            ModelState.AddModelError("", "Enter only the Fixed Deposit details ");
                            return View();

                        }
                        else if (cust.Fd_AccountNumber != 0 && cust.Fd_BankName != null && cust.FdAmount != 0)
                        {
                            l.Fd_AccountNumber = cust.Fd_AccountNumber;
                            l.Fd_BankName = cust.Fd_BankName;
                            l.FdAmount = cust.FdAmount;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Enter the Fixed Deposit details");
                            return View();

                        }
                    }
                    else
                    {
                        if (cust.Fd_AccountNumber != 0 || cust.Fd_BankName != null || cust.FdAmount != 0)
                        {
                            ModelState.AddModelError("", "Enter only the Property details");
                            return View();

                        }
                        else if (cust.Property_Address != null && cust.Property_Evaluation != 0)
                        {
                            l.Property_Address = cust.Property_Address;
                            l.Property_Evaluation = cust.Property_Evaluation;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Enter the Property details");
                            return View();

                        }

                    }
                }
                if (cust.LoanType != null && cust.LoanAmount != 0)
                {
                    l.LoanType = cust.LoanType;
                    l.LoanAmount = cust.LoanAmount;
                }
                else
                {
                    ModelState.AddModelError("", "Enter the Loan details");
                    return View();
                }

                if (cust.Files[0] != null && cust.Files[1] != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(cust.Files[0].FileName);
                    string FileExtension = Path.GetExtension(cust.Files[0].FileName);
                    FileName = c.CustomerId + "-" + "Aadhar" + FileName.Trim() + FileExtension;
                    string UploadPath = ConfigurationManager.AppSettings["UserPath"].ToString();
                    c.AadharUpload = UploadPath + FileName;
                    cust.Files[0].SaveAs(c.AadharUpload);

                    string FileName1 = Path.GetFileNameWithoutExtension(cust.Files[1].FileName);
                    string FileExtension1 = Path.GetExtension(cust.Files[1].FileName);
                    FileName1 = c.CustomerId + "-" + "PAN" + FileName1.Trim() + FileExtension1;
                    string UploadPath1 = ConfigurationManager.AppSettings["UserPath"].ToString();
                    c.PanUpload = UploadPath1 + FileName1;
                    cust.Files[1].SaveAs(c.PanUpload);
                }
                else
                {
                    ModelState.AddModelError("", "Please upload the required Documents");
                    return View();
                }

                context.Loans.Add(l);
                context.SaveChanges();
                return View("SubmitSuccess") ;
            }
            else if(command=="Save")
            {
                Customer c = new Customer();
                c = context.Customers.Where(t => t.CustomerId == cust.CustomerId).FirstOrDefault();
                Loan l = new Loan();
                l.status = "Saved";
                l.CustomerId = cust.CustomerId;
                l.CollateralType = cust.CollateralType;
                l.Fd_AccountNumber = cust.Fd_AccountNumber;
                l.Fd_BankName = cust.Fd_BankName;
                l.FdAmount = cust.FdAmount;
                l.Property_Address = cust.Property_Address;
                l.Property_Evaluation = cust.Property_Evaluation;
                l.LoanType = cust.LoanType;
                l.LoanAmount = cust.LoanAmount;
                context.Loans.Add(l);
                context.SaveChanges();
                return RedirectToAction("MyRequests", "Customer");
            }      
            return RedirectToAction("CustomerForm");
        }

        [HttpGet]
        public ActionResult SavedCustomerForm(int id)
        {
            Customer c = new Customer();
            Loan loanc = context.Loans.Where(t => t.Id == id && t.status == "Saved").FirstOrDefault();
            c = context.Customers.Where(t => t.CustomerId == loanc.CustomerId).FirstOrDefault();
            CustomerFormViewModel u = new CustomerFormViewModel();
            u.LoanId = loanc.Id;
            u.CustomerId = c.CustomerId;
            u.CustomerName = c.CustomerName;
            u.EmailId = c.EmailId;
            u.ContactNumber = c.ContactNumber;
            u.Address = c.Address;
            u.City = c.City;
            u.Aadhar_number = c.Aadhar_number;
            u.Pan_number = c.Pan_number;
            u.Pincode = c.Pincode;
            u.CollateralType = loanc.CollateralType;
            u.Fd_AccountNumber = loanc.Fd_AccountNumber;
            u.FdAmount = loanc.FdAmount;
            u.Fd_BankName = loanc.Fd_BankName;
            u.Property_Address = loanc.Property_Address;
            u.Property_Evaluation = loanc.Property_Evaluation;
            u.LoanAmount = loanc.LoanAmount;
            u.LoanType = loanc.LoanType;

            return View("SavedCustomerForm", u);
        }
        [HttpPost]
        public ActionResult SavedCustomerForm(CustomerFormViewModel cust, string command)
        {
            if (command == "Submit")
            {
                Customer c = new Customer();
                c = context.Customers.Where(t => t.CustomerId == cust.CustomerId).FirstOrDefault();
                if (cust.Address != null && cust.City != null && cust.Pincode != 0 && cust.Aadhar_number != 0 && cust.Pan_number != null)
                {
                    c.City = cust.City;
                    c.Address = cust.Address;
                    c.Pincode = cust.Pincode;
                    c.Aadhar_number = cust.Aadhar_number;
                    c.Pan_number = cust.Pan_number;
                }
                else
                {
                    ModelState.AddModelError("", "Enter the Customer Basic Details");
                    return View();
                }
                Loan l = new Loan();
                l = context.Loans.Where(t => t.Id == cust.LoanId && t.status == "Saved").FirstOrDefault();
                l.status = "Submitted";
                l.CustomerId = cust.CustomerId;
                if (cust.CollateralType == null)
                {
                    ModelState.AddModelError("", "Enter the Collateral details");
                    return View();
                }
                else
                {
                    l.CollateralType = cust.CollateralType;
                    if (l.CollateralType == "Fixed Deposit")
                    {
                        if (cust.Property_Address != null || cust.Property_Evaluation != 0)
                        {
                            ModelState.AddModelError("", "Enter only the Fixed Deposit details ");
                            return View();

                        }
                        else if (cust.Fd_AccountNumber != 0 && cust.Fd_BankName != null && cust.FdAmount != 0)
                        {
                            l.Fd_AccountNumber = cust.Fd_AccountNumber;
                            l.Fd_BankName = cust.Fd_BankName;
                            l.FdAmount = cust.FdAmount;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Enter the Fixed Deposit details");
                            return View();

                        }
                    }
                    else
                    {
                        if (cust.Fd_AccountNumber != 0 || cust.Fd_BankName != null || cust.FdAmount != 0)
                        {
                            ModelState.AddModelError("", "Enter only the Property details");
                            return View();

                        }
                        else if (cust.Property_Address != null && cust.Property_Evaluation != 0)
                        {
                            l.Property_Address = cust.Property_Address;
                            l.Property_Evaluation = cust.Property_Evaluation;
                        }
                        else
                        {
                            ModelState.AddModelError("", "Enter the Property details");
                            return View();

                        }

                    }
                }
                if (cust.LoanType != null && cust.LoanAmount != 0)
                {
                    l.LoanType = cust.LoanType;
                    l.LoanAmount = cust.LoanAmount;
                }
                else
                {
                    ModelState.AddModelError("", "Enter the Loan details");
                    return View();
                }

                if (cust.Files[0] != null && cust.Files[1] != null)
                {
                    string FileName = Path.GetFileNameWithoutExtension(cust.Files[0].FileName);
                    string FileExtension = Path.GetExtension(cust.Files[0].FileName);
                    FileName = c.CustomerId + "-" + "Aadhar" + FileName.Trim() + FileExtension;
                    string UploadPath = ConfigurationManager.AppSettings["UserPath"].ToString();
                    c.AadharUpload = UploadPath + FileName;
                    cust.Files[0].SaveAs(c.AadharUpload);

                    string FileName1 = Path.GetFileNameWithoutExtension(cust.Files[1].FileName);
                    string FileExtension1 = Path.GetExtension(cust.Files[1].FileName);
                    FileName1 = c.CustomerId + "-" + "PAN" + FileName1.Trim() + FileExtension1;
                    string UploadPath1 = ConfigurationManager.AppSettings["UserPath"].ToString();
                    c.PanUpload = UploadPath1 + FileName1;
                    cust.Files[1].SaveAs(c.PanUpload);
                }
                else
                {
                    ModelState.AddModelError("", "Please upload the required Documents");
                    return View();
                }
                context.SaveChanges();
                return View("SubmitSuccess");
            }
            else if (command == "Save")
            {
                Customer c = new Customer();
                c = context.Customers.Where(t => t.CustomerId == cust.CustomerId).FirstOrDefault();
                Loan l = new Loan();
                l = context.Loans.Where(t => t.Id == cust.LoanId && t.status == "Saved").FirstOrDefault();
                l.status = "Saved";
                l.CustomerId = cust.CustomerId;
                l.CollateralType = cust.CollateralType;
                l.Fd_AccountNumber = cust.Fd_AccountNumber;
                l.Fd_BankName = cust.Fd_BankName;
                l.FdAmount = cust.FdAmount;
                l.Property_Address = cust.Property_Address;
                l.Property_Evaluation = cust.Property_Evaluation;
                l.LoanType = cust.LoanType;
                l.LoanAmount = cust.LoanAmount;
                context.SaveChanges();
                return RedirectToAction("MyRequests", "Customer");

            }
            return RedirectToAction("CustomerForm");
        }

        [HttpGet]
        public ActionResult SubmittedCustomerForm(int id)
        {
            SaveFormViewModel s = new SaveFormViewModel();
            Customer c = new Customer(); 
            Loan l = new Loan();
            l = context.Loans.Where(t => t.Id == id && t.status=="Submitted").FirstOrDefault();
            c = context.Customers.Where(t => t.CustomerId == l.CustomerId).FirstOrDefault();
            s.customer = c;
            s.loan = l;
            return View("SubmittedCustomerForm",s);
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyRequests()
        {
            List<Loan> l = new List<Loan>();
            Customer c = new Customer();
            var id = User.Identity.Name;
            c = context.Customers.Where(t => t.EmailId == id).FirstOrDefault();
            l = context.Loans.Where(t => t.CustomerId == c.CustomerId).ToList();
            return View(l);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Loan l = new Loan();
            l = context.Loans.Where(t => t.Id == id).FirstOrDefault();
            if(l.status=="Saved")
            {
                return RedirectToAction("SavedCustomerForm",new { id = l.Id });
            }
            else if(l.status=="Submitted")
            {
                return RedirectToAction("SubmittedCustomerForm",new { id = l.Id });
            }
            return RedirectToAction("MyRequests");
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
    }
