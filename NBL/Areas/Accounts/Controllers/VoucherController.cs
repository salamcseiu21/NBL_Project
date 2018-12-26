﻿
using NBL.Areas.Accounts.BLL;
using NBL.Areas.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Accounts.Controllers
{
    [Authorize(Roles = "Accounts")]
    public class VoucherController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        readonly AccountsManager _accountsManager = new AccountsManager();

        public VoucherController(ICommonManager iCommonManager)
        {
            _iCommonManager = iCommonManager;
        }
        [HttpGet]
        public ActionResult CreditVoucher()
        {
            Session["PurposeList"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult CreditVoucher(FormCollection collection)
        {

            try
            {

                Purpose aPurpose = new Purpose
                {
                    PurposeCode = collection["PurposeCode"],
                    Amounts = Convert.ToDecimal(collection["PurposeAmounts"]),
                    PurposeName = collection["PurposeName"],
                    Remarks = collection["Remarks"],
                    DebitOrCredit = "Dr"
                };
                List<Purpose> purposeList = (List<Purpose>)Session["PurposeList"];
               
                if (purposeList != null)
                {
                    purposeList.Add(aPurpose);
                }
                else
                {
                    purposeList = new List<Purpose> { aPurpose };
                    Session["PurposeList"] = purposeList;
                    ViewBag.Payments = purposeList;
                }

                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                return View();
            }
        }
        [HttpPost]
        public void RemoveCreditPurpose(FormCollection collection)
        {
            List<Purpose> purposeList = (List<Purpose>)Session["PurposeList"];
            string purposeCode = Convert.ToString(collection["purposeCodeToRemove"]);
            var purpose = purposeList.Find(n => n.PurposeCode.Equals(purposeCode));
            purposeList.Remove(purpose);
            Session["PurposeList"] = purposeList;
        }

        [HttpPost]
        public JsonResult SaveCreditVoucher(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                List<Purpose> purposeList = (List<Purpose>)Session["PurposeList"];
                Voucher voucher = new Voucher();
                var transacitonTypeId = Convert.ToInt32(collection["TransactionTypeId"]);
                var amount = purposeList.Sum(n=>n.Amounts);
                string accontCode=collection["AccountCode"];
                var aPurpose = new Purpose
                {
                    PurposeCode = accontCode,
                    Amounts =amount,
                    DebitOrCredit = "Cr",
                   
                };

                var anUser = (ViewUser)Session["user"];
                purposeList.Add(aPurpose);
                voucher.PurposeList = purposeList;
                voucher.Remarks = collection["Remarks"];
                voucher.VoucherType = 1;
                voucher.VoucherName = "Credit Voucher";
                voucher.VoucherDate = Convert.ToDateTime(collection["Date"]);
                voucher.Amounts = amount;
                voucher.VoucherByUserId = anUser.UserId;
                voucher.BranchId = Convert.ToInt32(Session["BranchId"]);
                voucher.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                voucher.TransactionTypeId = transacitonTypeId;
                voucher.AccountCode = accontCode;
                int rowAffected = _accountsManager.SaveVoucher(voucher);
                if (rowAffected > 0)
                {
                    Session["PurposeList"] = null;
                    aModel.Message = "<p class='text-green'>Saved credit voucher successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save credit voucher info !</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception?.Message;
                aModel.Message = "<p style='color:red'>" + ex + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DebitVoucher()
        {
            Session["DebitPurposeList"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult DebitVoucher(FormCollection collection)
        {
            try
            {

                Purpose aPurpose = new Purpose
                {
                    PurposeCode = collection["PurposeCode"],
                    Amounts = Convert.ToDecimal(collection["PurposeAmounts"]),
                    PurposeName = collection["PurposeName"],
                    DebitOrCredit = "Cr",
                    Remarks = collection["Remarks"]
                };
                List<Purpose> purposeList = (List<Purpose>)Session["DebitPurposeList"];
                if (purposeList != null)
                {
                    purposeList.Add(aPurpose);
                }
                else
                {
                    purposeList = new List<Purpose> { aPurpose };
                    Session["DebitPurposeList"] = purposeList;
                    ViewBag.Payments = purposeList;
                }

                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                return View();
            }
        }

        [HttpPost]
        public void RemoveDebitPurpose(FormCollection collection) 
        {
            List<Purpose> purposeList = (List<Purpose>)Session["DebitPurposeList"];
            string purposeCode = Convert.ToString(collection["purposeCodeToRemove"]);
            var purpose = purposeList.Find(n => n.PurposeCode.Equals(purposeCode));
            purposeList.Remove(purpose);
            Session["DebitPurposeList"] = purposeList;
        }

        [HttpPost]
        public JsonResult SaveDebitVoucher(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                List<Purpose> purposeList = (List<Purpose>)Session["DebitPurposeList"];
                Voucher voucher = new Voucher();
                int transacitonTypeId = Convert.ToInt32(collection["TransactionTypeId"]);
                var amount = purposeList.Sum(n => n.Amounts);
                var date = Convert.ToDateTime(collection["Date"]);
                string accontCode = collection["AccountCode"];
                Purpose aPurpose = new Purpose
                {
                    PurposeCode = accontCode,
                    Amounts =amount,
                    DebitOrCredit = "Dr"
                };

                var anUser = (ViewUser)Session["user"];
               
                purposeList.Add(aPurpose);
               
                //-------------Voucher type=2 Debit voucher----------
                voucher.PurposeList = purposeList;
                voucher.Remarks = collection["Remarks"];
                voucher.VoucherType = 2;
                voucher.VoucherName = "Debit Voucher";
                voucher.VoucherDate = date;
                voucher.Amounts = amount;
                voucher.VoucherByUserId = anUser.UserId;
                voucher.BranchId = Convert.ToInt32(Session["BranchId"]);
                voucher.CompanyId = Convert.ToInt32(Session["CompanyId"]);
                voucher.TransactionTypeId = transacitonTypeId;
                voucher.AccountCode = accontCode;
                
                int rowAffected = _accountsManager.SaveVoucher(voucher); 
                if (rowAffected > 0)
                {
                    Session["DebitPurposeList"] = null;
                    aModel.Message = "<p class='text-green'>Saved Debit voucher successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save Debit voucher info !</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ChequePaymentVoucher()
        {
            return View();
        } 
        [HttpPost]
        public JsonResult ChequePaymentVoucher(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                List<Purpose> purposeList = new List<Purpose>();
                Voucher voucher = new Voucher();
                var amount = Convert.ToDecimal(collection["Amount"]);
                var date = Convert.ToDateTime(collection["Date"]);
                string bankCode = collection["BankCode"];
                string purposeCode= collection["PurposeCode"];
                
                Purpose debitPurpose = new Purpose
                {
                    PurposeCode = bankCode,
                    Amounts = amount,
                    DebitOrCredit = "Dr"
                };
                purposeList.Add(debitPurpose);
                var anUser = (ViewUser)Session["user"];
             
             
                Purpose creditPurpose = new Purpose
                {
                    PurposeCode = purposeCode,
                    Amounts =amount,
                    DebitOrCredit = "Cr"
                };
                purposeList.Add(creditPurpose);
                //-------------Voucher type 3 = Cheque payment voucher,transcation type 2 = Bank
                voucher.PurposeList = purposeList;
                voucher.Remarks = collection["Remarks"];
                voucher.VoucherType = 3;
                voucher.VoucherName = "Cheque Payment Voucher";
                voucher.TransactionTypeId = 2;
                voucher.VoucherDate = date;
                voucher.Amounts = amount;
                voucher.VoucherByUserId = anUser.UserId;
                voucher.BranchId = Convert.ToInt32(Session["BranchId"]);
                voucher.CompanyId = Convert.ToInt32(Session["CompanyId"]);

                int rowAffected = _accountsManager.SaveVoucher(voucher);
                if (rowAffected > 0)
                {
                    aModel.Message = "<p class='text-green'>Saved Cheque payment voucher successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save Cheque payment voucher info !</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ChequeReceiveVoucher()
        {
            ViewBag.PaymentTypes = _iCommonManager.GetAllPaymentTypes().ToList();
            return View();
        }
        [HttpPost]
        public JsonResult ChequeReceiveVoucher(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                List<Purpose> purposeList = new List<Purpose>();
                Voucher voucher = new Voucher();
                var amount = Convert.ToDecimal(collection["Amount"]);
                string bankCode = collection["BankCode"];
                string purposeCode = collection["PurposeCode"];
                Purpose creditPurpose = new Purpose
                {
                    PurposeCode = bankCode,
                    Amounts = amount,
                    DebitOrCredit = "Cr"
                };
                purposeList.Add(creditPurpose);
                var anUser = (ViewUser)Session["user"];


                Purpose debitPurpose  = new Purpose
                {
                    PurposeCode = purposeCode,
                    Amounts =amount,
                    DebitOrCredit = "Dr"
                };
                purposeList.Add(debitPurpose);
                //-------------Voucher type  = Cheque receive voucher,transcation type 2 = Bank
                voucher.PurposeList = purposeList;
                voucher.Remarks = collection["Remarks"];
                voucher.VoucherType = 4;
                voucher.VoucherName = "Cheque Receive Voucher";
                voucher.TransactionTypeId = 2;
                voucher.VoucherDate = Convert.ToDateTime(collection["Date"]);
                voucher.Amounts = amount;
                voucher.VoucherByUserId = anUser.UserId;
                voucher.BranchId = Convert.ToInt32(Session["BranchId"]);
                voucher.CompanyId = Convert.ToInt32(Session["CompanyId"]);

                int rowAffected = _accountsManager.SaveVoucher(voucher);
                if (rowAffected > 0)
                {
                    aModel.Message = "<p class='text-green'>Saved Cheque Receive voucher successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save Cheque receive voucher info !</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult JournalVoucher() 
        {

            Session["Journals"] = null;
            ViewBag.PaymentTypes = _iCommonManager.GetAllPaymentTypes().ToList();
            return View();
        }

        [HttpPost]
        public ActionResult JournalVoucher(FormCollection collection) 
        {
            var paymentTypes = _iCommonManager.GetAllPaymentTypes().ToList();
            try
            {

               
                JournalVoucher aJournal = new JournalVoucher();
                var purposeName = collection["PurposeName"];
                var purposeCode = collection["PurposeCode"];
                var remarks = collection["Remarks"];
                var amount = Convert.ToDecimal(collection["Amount"]);
                var transactionType = collection["TransactionType"];
                var date = Convert.ToDateTime(collection["Date"]);
                aJournal.DebitOrCredit = transactionType;
                if (transactionType.Equals("Cr"))
                {
                    aJournal.Amounts = amount * -1;
                }
                else
                {
                    aJournal.Amounts = amount;
                }

                aJournal.PurposeName = purposeName;
                aJournal.PurposeCode = purposeCode;
                aJournal.VoucherDate = date;
                aJournal.Remarks = remarks;
                List<JournalVoucher> journals = (List<JournalVoucher>)Session["Journals"]; 
                if (journals != null)
                {
                    journals.Add(aJournal);
                }
                else
                {
                    journals = new List<JournalVoucher> { aJournal };
                    Session["Journals"] = journals;
                    ViewBag.Payments = journals;
                }


                ViewBag.PaymentTypes = paymentTypes;
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message + "<br>System Error :" + exception?.InnerException?.Message;
                ViewBag.PaymentTypes = paymentTypes;
                return View();
            }
        }
        [HttpPost]
        public void RemoveJournalVoucher(FormCollection collection)
        {
            List<JournalVoucher> journals = (List<JournalVoucher>)Session["Journals"];
            string purposeCode = Convert.ToString(collection["JournalInfoToRemove"]);
            var purpose = journals.Find(n => n.PurposeCode.Equals(purposeCode));
            journals.Remove(purpose);
            Session["Journals"] = journals; 
        }
        //------------Save journal information into database--------//
        [HttpPost]
        public JsonResult SaveJournalVoucher(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {

                var anUser = (ViewUser)Session["user"];
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                JournalVoucher aJournal = new JournalVoucher
                {
                    VoucherDate = Convert.ToDateTime(collection["Date"]),
                    SysDateTime = DateTime.Now,
                    BranchId = branchId,
                    CompanyId = companyId,
                    VoucherByUserId = anUser.UserId
                };
                List<JournalVoucher> journals = (List<JournalVoucher>)Session["Journals"];
                //---------------insert code should be write here---
                int rowAffected = _accountsManager.SaveJournalVoucher(aJournal,journals);
                if (rowAffected > 0)
                {
                    Session["Journals"] = null;
                    aModel.Message = "<p class='text-green'>Saved Journal Information successfully!</p>";
                }
                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to save Journal Information!</p>";
                }

            }
            catch (Exception exception)
            {

                var ex = exception.Message;
                aModel.Message = "<p style='color:red'>" + ex + "</p>";

            }
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
        //----------------Get temp Journal Information ------------------
        public JsonResult GetTempJournalInformation()
        {
            if (Session["Journals"] != null)
            {
                IEnumerable<JournalVoucher> journals = ((List<JournalVoucher>)Session["Journals"]).ToList();
                return Json(journals, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<JournalVoucher>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewJournal()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var journals = _accountsManager.GetAllJournalVouchersByBranchAndCompanyId(branchId,companyId);
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


        public ActionResult EditJournalVoucher(int id) 
        {
            var voucher = _accountsManager.GetJournalVoucherById(id);
            var voucherDetails = _accountsManager.GetJournalVoucherDetailsById(id);
            ViewBag.JournalDetails = voucherDetails;
            return View(voucher);
        }
        [HttpPost]
        public ActionResult EditJournalVoucher(int id, FormCollection collection)
        {
            var user = (ViewUser)Session["user"];
            var voucher = _accountsManager.GetJournalVoucherById(id);
            voucher.UpdatedByUserId = user.UserId;
            var voucherDetails = _accountsManager.GetJournalVoucherDetailsById(id);
          
            foreach (JournalDetails detail in voucherDetails)
            {
                detail.Amount = Convert.ToDecimal(collection["amount_of_" + detail.JournalDetailsId]);
            }
           var cr= voucherDetails.ToList().FindAll(n => n.DebitOrCredit.Equals("Cr")).Sum(n => n.Amount);
           var dr = voucherDetails.ToList().FindAll(n => n.DebitOrCredit.Equals("Dr")).Sum(n => n.Amount);
            if (dr == cr)
            {
                voucher.Amounts = voucherDetails.ToList().FindAll(n => n.DebitOrCredit.Equals("Cr")).Sum(n => n.Amount);
                bool result = _accountsManager.UpdateJournalVoucher(voucher, voucherDetails.ToList());
                if (result)
                {
                    return RedirectToAction("ViewJournal");
                }
                ViewBag.JournalDetails = voucherDetails;
                return View(voucher);
            }
            ViewBag.ErrorMssage = "Debit and Credit amount not same !!";
            ViewBag.JournalDetails = voucherDetails;
            return View(voucher);

        }

        //----------------Get temp credit Purpose Information ------------------
        public JsonResult GetTempCreditPurposeInformation()
        {
            if (Session["PurposeList"] != null)
            {
                IEnumerable<Purpose> purposeList = ((List<Purpose>)Session["PurposeList"]).ToList();
                return Json(purposeList, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Payment>(), JsonRequestBehavior.AllowGet);
        }

     

        //----------------Get temp debit Purpose Information ------------------
        public JsonResult GetTempDebitPurposeInformation()
        {
            if (Session["DebitPurposeList"] != null)
            {
                
                IEnumerable<Purpose> purposeList = ((List<Purpose>)Session["DebitPurposeList"]).ToList();
                return Json(purposeList, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Payment>(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCreditVoucher()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _accountsManager.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, 1);
            return View(vouchers);

        }
        public ActionResult ViewDebitVoucher()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _accountsManager.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, 2);
            return View(vouchers);
        }

        public ActionResult ViewChequePaymentVoucher() 
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _accountsManager.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, 3);
            return View(vouchers);
        }
        public ActionResult ViewChequeReceiveVoucher()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _accountsManager.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, 4);
            return View(vouchers);
        }

        public PartialViewResult Vouchers()
        {
             var vouchers = _accountsManager.GetVoucherList();
            ViewBag.VoucherName = "All Vouchers";
            return PartialView("_VoucherPartialPage",vouchers);
        }

        public PartialViewResult PendingVouchers()
        {
            ViewBag.VoucherName = "Pending Vouchers";
            var vouchers = _accountsManager.GetPendingVoucherList();
            return PartialView("_VoucherPartialPage", vouchers);
        }
        public ActionResult VoucherDetails(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id); 
            return View(voucher);
        }
        //----------------Edit Voucher-----------------
        public ActionResult EditVoucher(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }
        [HttpPost]
        public ActionResult EditVoucher(int id,FormCollection collection)
        {
            var user = (ViewUser) Session["user"];

            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            voucher.UpdatedByUserId = user.UserId;
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(id);
            foreach (var detail in voucherDetails)
            {
                detail.Amounts = Convert.ToDecimal(collection["amount_of_" + detail.VoucherDetailsId]);
            }
            voucher.Amounts = Convert.ToDecimal(collection["Amount"]);
            bool result = _accountsManager.UpdateVoucher(voucher,voucherDetails.ToList());
            if (result)
            {
                return RedirectToAction("Vouchers");
            }
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }

        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {
            int voucherId = Convert.ToInt32(collection["VoucherId"]);
            string reason = collection["Reason"];
            var anUser = (ViewUser)Session["user"];
            bool result=_accountsManager.CancelVoucher(voucherId,reason,anUser.UserId);
            if (result)
            {
                return RedirectToAction("Vouchers");
            }
            var voucher = _accountsManager.GetVoucherByVoucherId(voucherId);
            return RedirectToAction("VoucherDetails","Voucher", voucher);
        }

        public ActionResult Approve(FormCollection collection)
        {
            int voucherId = Convert.ToInt32(collection["VoucherIdToApprove"]);
            Voucher aVoucher=_accountsManager.GetVoucherByVoucherId(voucherId);
            var anUser = (ViewUser)Session["user"];
            var voucherDetails=_accountsManager.GetVoucherDetailsByVoucherId(voucherId).ToList();
            bool result = _accountsManager.ApproveVoucher(aVoucher,voucherDetails,anUser.UserId);
            return result ? RedirectToAction("Vouchers") : RedirectToAction("VoucherDetails", "Voucher", aVoucher);
        }

        public ActionResult VoucherPreview(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }
        public ActionResult JournalPreview(int id)
        {
            var voucher = _accountsManager.GetJournalVoucherById(id);
            var voucherDetails = _accountsManager.GetJournalVoucherDetailsById(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }
    }
}