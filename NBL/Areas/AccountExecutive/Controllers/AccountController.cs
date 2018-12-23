using NBL.Areas.Accounts.BLL;
using NBL.Areas.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.AccountExecutive.Controllers
{
    [Authorize(Roles = "AccountExecutive")]
    public class AccountController : Controller
    {

        readonly AccountsManager _accountsManager = new AccountsManager();
        readonly ClientManager _clientManager = new ClientManager();
        private readonly VatManager _vatManager = new VatManager();
        private readonly DiscountManager _discountManager = new DiscountManager();
        // GET: AccountExecutive/Account
        [HttpGet]
        public ActionResult ActiveReceivable() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var receivableCheques = _accountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId);
            return View(receivableCheques);
        }
        [HttpGet]
        public ActionResult ReceivableDetails(int id)
        {
            var receivableCheques = _accountsManager.GetReceivableChequeByDetailsId(id);
            return View(receivableCheques);
        }
        [HttpPost]
        public ActionResult ReceivableDetails(FormCollection collection,int id)
        {
            try
            {
                var anUser = (ViewUser)Session["user"];
                var chequeDetails = _accountsManager.GetReceivableChequeByDetailsId(id);
                Client aClient = _clientManager.GetClientById(chequeDetails.ClientId);
                DateTime date = Convert.ToDateTime(collection["ReceiveDate"]);
                string bankCode = collection["BankCode"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);

                Receivable aReceivable = new Receivable
                {
                    TransactionRef = chequeDetails.ReceivableRef,
                    SubSubSubAccountCode = bankCode,
                    ReceivableDateTime = date,
                    BranchId = branchId,
                    CompanyId = companyId,
                    UserId = anUser.UserId,
                    Paymode = 'B',
                    Remarks = "Active receivable by " + anUser.UserId
                };

                bool result = _accountsManager.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
                if (result)
                {
                    //---------Send Mail ----------------
                    var body = $"Dear {aClient.ClientName}, your receivalbe amount is receive by NBL. thanks and regards Accounts Departments NBL.";
                    var subject = $"Receiable Confirm at {DateTime.Now}";
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
                }
                return RedirectToAction("ActiveReceivable");
            }
            catch (Exception exception)
            {
                var chequeDetails = _accountsManager.GetReceivableChequeByDetailsId(id);
                TempData["Error"] = exception.Message;
                return View(chequeDetails);
            }
        }

       [HttpPost]
        public JsonResult ApproveCashAmount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                
                var anUser = (ViewUser)Session["user"];
                int detailsId = Convert.ToInt32(collection["ChequeDetailsId"]);
                var chequeDetails = _accountsManager.GetReceivableChequeByDetailsId(detailsId);
                Client aClient = _clientManager.GetClientById(chequeDetails.ClientId);
                DateTime date = DateTime.Now;
                string bankCode = "3308011";
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                Receivable aReceivable = new Receivable
                {
                    TransactionRef = chequeDetails.ReceivableRef,
                    SubSubSubAccountCode = bankCode,
                    ReceivableDateTime = date,
                    BranchId = branchId,
                    CompanyId = companyId,
                    Paymode = 'C',
                    UserId = anUser.UserId,
                    Remarks = "Active receivable by " + anUser.UserId
                };
                bool result = _accountsManager.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
                if (result)
                {
                    //---------Send Mail ----------------
                    var body = $"Dear {aClient.ClientName}, your receivalbe amount is receive by NBL. thanks and regards Accounts Departments NBL.";
                    var subject = $"Receiable Confirm at {DateTime.Now}";
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
                }
                aModel.Message = result ? "<p class='text-green'> Cash Amount Approved Successfully!</p>" : "<p class='text-danger'> Failed to  Approve Cash Amount! </p>";
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                aModel.Message = " <p style='color:red'>"+message+"</p>";
                
            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult Vouchers()
        {
            var vouchers = _accountsManager.GetPendingVoucherList().ToList();
            return PartialView("_VoucherPartialPage", vouchers);
        }
        public ActionResult VoucherDetails(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            return View(voucher);
        }

        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {
            int voucherId = Convert.ToInt32(collection["VoucherId"]);
            string reason = collection["Reason"];
            var anUser = (ViewUser)Session["user"];
            bool result = _accountsManager.CancelVoucher(voucherId, reason, anUser.UserId);
            if (result)
            {
                return RedirectToAction("Vouchers");
            }
            var voucher = _accountsManager.GetVoucherByVoucherId(voucherId);
            return RedirectToAction("VoucherDetails", "Voucher", voucher);
        }

        public ActionResult Approve(FormCollection collection)
        {
            int voucherId = Convert.ToInt32(collection["VoucherIdToApprove"]);
            Voucher aVoucher = _accountsManager.GetVoucherByVoucherId(voucherId);
            var anUser = (ViewUser)Session["user"];
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(voucherId).ToList();
            bool result = _accountsManager.ApproveVoucher(aVoucher, voucherDetails, anUser.UserId);
            return result ? RedirectToAction("Vouchers") : RedirectToAction("VoucherDetails", "Account", aVoucher);
        }


        public ActionResult ViewJournal()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            List<JournalVoucher> journals = _accountsManager.GetAllPendingJournalVoucherByBranchAndCompanyId(branchId, companyId);
            return View(journals);
        }

        public ActionResult JournalDetails(int id)
        {
            JournalVoucher journal = _accountsManager.GetJournalVoucherById(id);
            List<JournalDetails> vouchers = _accountsManager.GetJournalVoucherDetailsById(id);
            ViewBag.JournalDetails = vouchers;
            return View(journal);

        }
        [HttpPost]
        public ActionResult CancelJournalVoucher(FormCollection collection)
        {
            int voucherId = Convert.ToInt32(collection["VoucherId"]);
            string reason = collection["Reason"];
            var anUser = (ViewUser)Session["user"];
            bool result = _accountsManager.CancelJournalVoucher(voucherId, reason, anUser.UserId);
            if (result)
            {
                return RedirectToAction("ViewJournal");
            }
            var voucher = _accountsManager.GetJournalVoucherById(voucherId);
            return RedirectToAction("JournalDetails", "Voucher", voucher);
        }

        [HttpPost]
        public ActionResult ApproveJournalVoucher(FormCollection collection)
        {
            int voucherId = Convert.ToInt32(collection["JournalVoucherIdToApprove"]);
            JournalVoucher aVoucher = _accountsManager.GetJournalVoucherById(voucherId);
            var anUser = (ViewUser)Session["user"];
            var voucherDetails = _accountsManager.GetJournalVoucherDetailsById(voucherId).ToList();
            bool result = _accountsManager.ApproveJournalVoucher(aVoucher, voucherDetails, anUser.UserId);
            return result ? RedirectToAction("ViewJournal") : RedirectToAction("JournalDetails", "Account", aVoucher);
        }

        public ActionResult Vats()
        {
            var vats = _vatManager.GetAllPendingVats();
            return View(vats);
        }
        [HttpPost]
        public JsonResult ApproveVat(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int vatId = Convert.ToInt32(collection["VatIdToApprove"]);
                var vat = _vatManager.GetAllPendingVats().ToList().Find(n => n.VatId == vatId);
                var anUser = (ViewUser)Session["user"];
                vat.ApprovedByUserId = anUser.UserId;
                bool result = _accountsManager.ApproveVat(vat);
                aModel.Message = result ? "<p class='text-green'>Vat info approved Successfully!!</p>" : "<p class='text-danger'>Failed to Approve!!</p>";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Discounts() 
        {
            IEnumerable<Discount> discounts = _discountManager.GetAllPendingDiscounts(); 
            return View(discounts);
        }

        [HttpPost]
        public JsonResult ApproveDiscount(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int discountId = Convert.ToInt32(collection["DiscountIdToApprove"]);
                var discount = _discountManager.GetAllPendingDiscounts().ToList().Find(n => n.DiscountId == discountId);
                var anUser = (ViewUser)Session["user"];
                discount.ApprovedByUserId = anUser.UserId;
                bool result = _accountsManager.ApproveDiscount(discount);
                aModel.Message = result ? "<p class='text-green'>Discount info approved Successfully!!</p>" : "<p class='text-danger'>Failed to Approve!!</p>";
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }
    }
}