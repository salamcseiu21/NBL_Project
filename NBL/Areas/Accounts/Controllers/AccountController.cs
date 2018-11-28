
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NBL.Areas.Accounts.Models;
using NBL.Areas.Accounts.BLL;

namespace NBL.Areas.Accounts.Controllers
{
    [Authorize(Roles ="Accounts")]
    public class AccountController : Controller
    {
        readonly BranchManager _branchManager = new BranchManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly AccountsManager _accountsManager = new AccountsManager();
        // GET: Accounts/Account

        [HttpGet]
        public ActionResult Receivable()
        {
            
            Session["Payments"] = null;
            var paymentTypes = _commonGateway.GetAllPaymentTypes().ToList();
            ViewBag.PaymentTypes = paymentTypes;
            return View();
        }
        [HttpPost]
        public ActionResult Receivable(FormCollection collection)
        {
            var paymentTypes = _commonGateway.GetAllPaymentTypes().ToList();
            var branches = _branchManager.GetAll().ToList();
            try
            {
                
                Payment aPayment = new Payment();
                int paymentTypeId = Convert.ToInt32(collection["PaymentTypeId"]);
                var bankBranchName = collection["SourceBankName"].ToString();
                var chequeNo = collection["ChequeNo"].ToString();
                var amount = Convert.ToDecimal(collection["Amount"]);
                var date = Convert.ToDateTime(collection["Date"]);
                aPayment.ChequeAmount = amount;
                aPayment.BankBranchName = bankBranchName;
                aPayment.ChequeNo = chequeNo;
                aPayment.ChequeDate = date;
                aPayment.PaymentTypeId = paymentTypeId;
                aPayment.BankAccountNo = collection["BankAccountNo"];
                aPayment.SourceBankName = collection["SourceBankName"].ToString();
                List<Payment> payments = (List<Payment>)Session["Payments"];
                if(payments!=null)
                {
                    payments.Add(aPayment);
                }
                else
                {
                    payments = new List<Payment> { aPayment };
                    Session["Payments"] = payments;
                    ViewBag.Payments = payments;
                }

              
                ViewBag.PaymentTypes = paymentTypes;
                ViewBag.Branches = branches;
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message + "<br>System Error :" + exception.InnerException.Message;
                ViewBag.PaymentTypes = paymentTypes;
                ViewBag.Branches = branches;
                return View();
            }
  
        }

        [HttpPost]
        public JsonResult SaveReceivable(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                
                User anUser = (User)Session["user"];
                Receivable receivable = new Receivable();
                List<Payment> payments = (List<Payment>)Session["Payments"];
                receivable.Payments = payments;
                receivable.ReceivableDateTime = DateTime.Now;
                receivable.UserId = anUser.UserId;
                receivable.ClientId = Convert.ToInt32(collection["ClientId"]);
                receivable.BranchId = Convert.ToInt32(Session["BranchId"]);
                receivable.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                receivable.TransactionTypeId = Convert.ToInt32(collection["TransactionTypeId"]);
                string inRef= collection["InvoiceRef"];
               
                if (inRef.StartsWith("IN00")){
                    receivable.InvoiceRef = DateTime.Now.Year.ToString().Substring(2, 2) + inRef;
                }
                    
                else
                {
                    receivable.InvoiceRef = inRef;
                }
                
                receivable.Remarks = collection["Remarks"];

                int rowAffected = _accountsManager.SaveReceivable(receivable);
                if (rowAffected > 0)
                {
                    Session["Payments"] = null;
                    aModel.Message = "<p class='text-green'>Saved receivable successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save receivable!</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex+"</p>";
               
            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Remove(FormCollection collection)
        {
            List<Payment> payments = (List<Payment>)Session["Payments"];
            string chequeNo = Convert.ToString(collection["chequeNoToRemove"]);

            var payment = payments.Find(n => n.ChequeNo == chequeNo);
            payments.Remove(payment);
            Session["Payments"] = payments;
            ViewBag.Payments = payments;
        }

        [HttpGet]
        public ActionResult Payable()
        {
            var paymentTypes = _commonGateway.GetAllPaymentTypes().ToList();
            ViewBag.PaymentTypes = paymentTypes;
            return View();
        }

        public JsonResult GetTempChequeInformation()
        {
            if (Session["Payments"] != null)
            {
                IEnumerable<Payment> payments = ((List<Payment>)Session["Payments"]).ToList();
                return Json(payments, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Payment>(), JsonRequestBehavior.AllowGet);
        }


   

    }
}