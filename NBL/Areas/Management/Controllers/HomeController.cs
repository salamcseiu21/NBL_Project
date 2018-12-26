﻿
using Microsoft.Ajax.Utilities;
using NBL.Areas.Accounts.BLL;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Management.Controllers
{

    [Authorize(Roles = "Management")]
    public class HomeController : Controller
    {
        readonly IBranchManager _iBranchManager;
        readonly IClientManager _iClientManager;
        readonly IOrderManager _iOrderManager;
        readonly InventoryManager _inventoryManager = new InventoryManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        readonly AccountsManager _accountsManager = new AccountsManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly  IReportManager _iReportManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IReportManager iReportManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
        }
        // GET: Management/Home
        public ActionResult Home()
        {
           
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var topClients = _iReportManager.GetTopClientsByBranchId(branchId).ToList();
            var topProducts = _iReportManager.GetPopularBatteriesByBranchAndCompanyId(branchId,companyId).ToList();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrderByBranchIdCompanyIdAndYear(branchId,companyId,DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(branchId, companyId) * -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId);
            var orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId);
            var pendingOrders = _iOrderManager.GetPendingOrdersByBranchAndCompanyId(branchId,companyId).ToList();
            var employees = _employeeManager.GetAllEmployeeWithFullInfoByBranchId(branchId);
            var branches = _iBranchManager.GetAll();
            SummaryModel aModel = new SummaryModel
            {
                Branches = branches.ToList(),
                BranchId = branchId,
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TotalSale = sales,
                TotalCollection = collection,
                OrderedAmount = orderedAmount,
                TopClients = topClients,
                TopProducts = topProducts,
                Clients = clients,
                Products = products,
                Orders = orders,
                PendingOrders = pendingOrders,
                Employees = employees

            };
            return View(aModel);
        }

        /// <summary>
        /// Test method for exporting data from database
        /// </summary>

        public void Export()
        {
            var clients= (from e in _iReportManager.GetTopClients()
             
                select new ViewClientModel
                {
                    ClientName = e.ClientName,
                    CommercialName =e.CommercialName,
                    Transaction =e.TotalDebitAmount
                }).ToList();
            var gv = new GridView
            {

                DataSource = clients,
            };
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Top_Clients.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);


        }
        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _iClientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage",clients);
        }
        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _iClientManager.GetClientDeailsById(id);
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
            var orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId,companyId).ToList();
            foreach (var viewOrder in orders)
            {
                viewOrder.Client = _iClientManager.GetClientById(viewOrder.ClientId);
            }
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public PartialViewResult CurrentMonthOrders() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
            ViewBag.Heading = $"Current Month Orders ({DateTime.Now:MMMM})";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            return View(order);
        }

        public PartialViewResult Stock() 
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

        public PartialViewResult PendingOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            ViewBag.Heading = "Pending Orders";
            var orders = _iOrderManager.GetPendingOrdersByBranchAndCompanyId(branchId, companyId);
            return PartialView("_ViewOrdersPartialPage", orders);
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
                string message = exception.InnerException?.Message?? "N/A";
                ViewBag.ErrorMessage = message;
                return View();
            }
           
        }

        public ActionResult ClientSummary()
        {
           var summary=_iClientManager.GetClientSummary();
            return View(summary);
        }

        public ActionResult OrderSummary()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel();
            ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }
    }
}