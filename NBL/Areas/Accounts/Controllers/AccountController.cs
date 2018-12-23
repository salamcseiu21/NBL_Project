using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Net.Mail;
using NBL.Areas.Accounts.Models;
using NBL.Areas.Accounts.BLL;
using NBL.BLL;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Accounts.Controllers
{
    [Authorize(Roles ="Accounts")]
    public class AccountController : Controller
    {
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly AccountsManager _accountsManager = new AccountsManager();
        private readonly ClientManager _clientManager = new ClientManager();
        // GET: Accounts/Account

        [HttpGet]
        public ActionResult Receivable()
        {
            Session["Payments"] = null;
            var model =
                new ViewReceivableCreateModel
                {
                    PaymentTypes = _commonGateway.GetAllPaymentTypes(),
                    TransactionTypes =_commonGateway.GetAllTransactionTypes()
                };

            return View(model);
        }
        [HttpPost]
        public ActionResult Receivable(FormCollection collection)
        {
             var model =
                new ViewReceivableCreateModel
                {
                    PaymentTypes = _commonGateway.GetAllPaymentTypes(),
                    TransactionTypes = _commonGateway.GetAllTransactionTypes()
                };
            try
            {
                
                Payment aPayment = new Payment();
                int paymentTypeId = Convert.ToInt32(collection["PaymentTypeId"]);
                var bankBranchName = collection["SourceBankName"];
                var chequeNo = collection["ChequeNo"];
                var amount = Convert.ToDecimal(collection["Amount"]);
                var date = Convert.ToDateTime(collection["Date"]);
                aPayment.ChequeAmount = amount;
                aPayment.BankBranchName = bankBranchName;
                aPayment.ChequeNo = chequeNo;
                aPayment.ChequeDate = date;
                aPayment.PaymentTypeId = paymentTypeId;
                aPayment.BankAccountNo = collection["BankAccountNo"];
                aPayment.SourceBankName = collection["SourceBankName"];
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
                return View(model);
            }
            catch (Exception exception)
            {
                TempData["Error"] = $"{exception.Message} <br>System Error : {exception.InnerException?.Message}";
                return View(model);
            }
  
        }

        [HttpPost]
        public JsonResult SaveReceivable(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                
                var anUser = (ViewUser)Session["user"];
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
                    //---------Send Mail ----------------
                    var aClient = _clientManager.GetClientById(Convert.ToInt32(collection["ClientId"]));
                    var body = $"Dear {aClient.ClientName}, a receivable is create to your account! thanks and regards Accounts Departments NBL.";
                    var subject = $"New Receiable Create at {DateTime.Now}";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(aClient.Email));  // replace with valid value 
                    message.Subject = subject;
                    message.Body = string.Format(body);
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        smtp.Send(message);
                    }
                    //------------End Send Mail-------------
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