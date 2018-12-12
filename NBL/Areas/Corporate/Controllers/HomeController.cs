
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;
using NBL.Areas.Accounts.BLL;
using NBL.Areas.Admin.BLL;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize(Roles = "Corporate")]
    public class HomeController : Controller
    {

        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly OrderManager _orderManager = new OrderManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        readonly AccountsManager _accountsManager=new AccountsManager();
        readonly DepartmentManager _departmentManager=new DepartmentManager();
        readonly VatManager _vatManager=new VatManager();
        readonly DiscountManager _discountManager=new DiscountManager();
        readonly InvoiceManager _invoiceManager=new InvoiceManager();
        readonly ReportManager _reportManager=new ReportManager();

        // GET: Corporate/Home
        public ActionResult Home() 
        {
           
            Session["BranchId"] = null;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _branchManager.GetAll();
            ViewTotalOrder totalOrder = _reportManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByCompanyId(companyId) * -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByCompanyId(companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByCompanyId(companyId);
            var clients = _reportManager.GetTopClients().ToList();
            var batteries = _reportManager.GetPopularBatteries().ToList();
            SummaryModel summary = new SummaryModel
            {
                Branches = branches.ToList(),
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TotalSale = sales,
                TotalCollection = collection,
                OrderedAmount = orderedAmount,
                Clients = clients,
                Products = batteries

            };
            return View(summary);
        }
        public PartialViewResult BranchWiseSummary() 
        {
            var branches = _branchManager.GetAll().ToList().FindAll(n => n.BranchId != 9).ToList();
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _invoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList(); 
            SummaryModel model=new SummaryModel 
            {
                Branches = branches,
                InvoicedOrderList = invoicedOrders
            };
            return PartialView("_ViewOrderSummaryPartialPage", model);
        }
        public PartialViewResult OrderSummary(int branchId)
        {
            List<Order> model = _orderManager.GetOrdersByBranchId(branchId).ToList();
            return PartialView("_ViewBranchWishOrderSummayPartialPage", model);
        }
        /// <summary>
        /// Sales reports 
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesSummary()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel ();
            ViewBag.BranchId=_branchManager.GetBranchSelectList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SalesSummary(ViewOrderSearchModel model)
        {
            model.Orders = model.Orders.ToList().FindAll(n => n.BranchId == model.BranchId);
            ViewBag.BranchId = _branchManager.GetBranchSelectList();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOrders(int? branchId)  
        {
            var orders = _orderManager.GetOrdersByBranchId(Convert.ToInt32(branchId)).ToList();
            return Json(new { data = orders }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            ViewOrderSearchModel model=new ViewOrderSearchModel();
            ViewBag.BranchId = _branchManager.GetBranchSelectList();
            return View(model);
        }
        /// <summary>
        /// Get All Orders 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult All()
        {
            List<Order> model = _orderManager.GetAll.ToList();
            return PartialView("_ViewBranchWishOrderSummayPartialPage", model);
        }

        /// <summary>
        /// Get All Client 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewClient()
        {
            var clients = _clientManager.GetAll.ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }
        /// <summary>
        /// Get client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult ViewClientProfile(int id)
        {
            var viewClient = _clientManager.GetClientDeailsById(id); 
            return PartialView("_ViewClientProfilePartialPage", viewClient); 

        }
        public PartialViewResult ViewEmployee()
        {
            var employees = _employeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }
        /// <summary>
        /// Get client by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _employeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }
        public PartialViewResult AllOrders()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetOrdersByCompanyId(companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }
        public PartialViewResult LatestOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetLatestOrdersByCompanyId(companyId).ToList().OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public PartialViewResult CurrentMonthOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderWithClientInformationByCompanyId(companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
            ViewBag.Heading = $"Current Month Orders ({DateTime.Now:MMMM})";
            return PartialView("_ViewOrdersPartialPage", orders);
        }
        /// <summary>
        /// Get order Details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult OrderDetails(int id)
        {
            var order = _orderManager.GetOrderByOrderId(id);
            return View(order);
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
        public PartialViewResult ViewDivision()
        {
            var divisions = _divisionGateway.GetAll.ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);

        }
        public PartialViewResult ViewRegion()
        {
            var regions = _regionManager.GetAllRegion().ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            var territories = _territoryManager.GetAllTerritory().ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);

        }
        public PartialViewResult Supplier()
        {
            var suppliers = _commonGateway.GetAllSupplier().ToList();
            return PartialView("_ViewSupplierPartialPage",suppliers);
        }

        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var journals = _accountsManager.GetAllJournalVouchersByCompanyId(companyId).ToList();
            return PartialView("_ViewJournalPartialPage",journals);
        }

        public ActionResult ViewDepartment()
        {
            var departments = _departmentManager.GetAll.ToList();
            return View(departments);
        }

        public PartialViewResult Vouchers()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var vouchers = _accountsManager.GetVoucherListByCompanyId(companyId);
            return PartialView("_ViewVouchersPartialPage",vouchers);
        }

        public ActionResult VoucherPreview(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return View(voucher);
        }

        public PartialViewResult ProductWishVat()
        {
            IEnumerable<Vat> vats = _vatManager.GetProductWishVat();
            return PartialView("_ViewProductWishVatPartialPage",vats);
        }

        public PartialViewResult ViewDiscount()
        {
            var discounts = _discountManager.GetAllDiscounts().ToList();
            return PartialView("_ViewDiscountPartialPage", discounts);
        }

        public ActionResult BusinessArea()
        {
            var branches = _branchManager.GetAll().ToList().Where(i => !i.BranchName.Contains("Corporate"));
            return View(branches);
        }

    }
}