﻿
using Microsoft.Ajax.Utilities;
using NBL.Areas.Accounts.BLL;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NBL.Areas.Management.Controllers
{

    [Authorize(Roles = "Management")]
    public class HomeController : Controller
    {
        readonly BranchManager _branchManager = new BranchManager();
        readonly ClientManager _clientManager = new ClientManager();
        readonly OrderManager _orderManager = new OrderManager();
        readonly InventoryManager _inventoryManager = new InventoryManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        readonly AccountsManager _accountsManager = new AccountsManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly  ReportManager _reportManager=new ReportManager();
        // GET: Management/Home
        public ActionResult Home()
        {
           
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _reportManager.GetTopClients().ToList();
            var batteries = _reportManager.GetPopularBatteries().ToList();
            ViewTotalOrder totalOrder = _reportManager.GetTotalOrderByBranchIdCompanyIdAndYear(branchId,companyId,DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(branchId, companyId) * -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var branches = _branchManager.GetAll();
            SummaryModel aModel = new SummaryModel
            {
                Branches = branches.ToList(),
                BranchId = branchId,
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TotalSale = sales,
                TotalCollection = collection,
                OrderedAmount = orderedAmount,
                Clients = clients,
                Products = batteries

            };

            return View(aModel);
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _clientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage",clients);
        }
        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _clientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage",client);

        }

        public PartialViewResult ViewEmployee()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var employees = _employeeManager.GetAllEmployeeWithFullInfoByBranchId(branchId).ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }
        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _employeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }
        public PartialViewResult AllOrders()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var orders = _orderManager.GetOrdersByBranchAndCompnayId(branchId,companyId).ToList();
            foreach (var viewOrder in orders)
            {
                viewOrder.Client = _clientManager.GetClientById(viewOrder.ClientId);
            }
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public PartialViewResult CurrentMonthOrders() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
            ViewBag.Heading = "Current Month Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _orderManager.GetOrderByOrderId(id);
            return View(order);
        }

        public PartialViewResult ProductInStock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var stockProducts = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage",stockProducts);
        }
        public PartialViewResult Supplier()
        {
            var suppliers = _commonGateway.GetAllSupplier().ToList();
            return PartialView("_ViewSupplierPartialPage",suppliers);
        }
        public PartialViewResult ViewDivision()
        {
            var divisions = _divisionGateway.GetAll.ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);
        }

        public PartialViewResult ViewRegion()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var regions = _regionManager.GetAssignedRegionListToBranchByBranchId(branchId).ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var territories = _territoryManager.GetTerritoryListByBranchId(branchId).ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);
        }

        public ActionResult BusinessArea()
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var regions = _regionManager.GetAssignedRegionListToBranchByBranchId(branchId).ToList();
            return View(regions); 
        }


        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var journals = _accountsManager.GetAllJournalVouchersByBranchAndCompanyId(branchId,companyId).ToList();
            return PartialView("_ViewJournalPartialPage",journals);
        }

        public PartialViewResult Vouchers()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var vouchers = _accountsManager.GetVoucherListByBranchAndCompanyId(branchId,companyId);
            return PartialView("_ViewVouchersPartialPage",vouchers);
        }

        public ActionResult VoucherPreview(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }

        public ActionResult SendMail()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMail(FormCollection collection,HttpPostedFileBase attachment)
        {
            try
            {
                var body = collection["MessageBody"];
                var email = collection["ToEmail"];
                var subject = collection["Subject"];
                var message = new MailMessage();
                message.To.Add(new MailAddress(email));  // replace with valid value 
                message.Subject = subject;
                message.Body = string.Format(body);
                message.IsBodyHtml = true;
                if (attachment != null && attachment.ContentLength > 0)
                {
                    message.Attachments.Add(new Attachment(attachment.InputStream, Path.GetFileName(attachment.FileName)));
                }
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(message);
                    ViewBag.SuccessMessage = "Mail Send Successfully!";
                    return View();
                }
            }
            catch(Exception exception)
            {
                string message = exception.Message?? "N/A";
                ViewBag.ErrorMessage = message;
                return View();
            }
           
        }
    }
}