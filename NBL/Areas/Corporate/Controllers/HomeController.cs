
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.BLL;
using NBL.Areas.Admin.BLL;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize(Roles = "Corporate")]
    public class HomeController : Controller
    {

        
        
        readonly IEmployeeManager _iEmployeeManager;
       
    
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        readonly AccountsManager _accountsManager=new AccountsManager();
        readonly IDepartmentManager _iDepartmentManager;
        
        readonly DiscountManager _discountManager=new DiscountManager();
        readonly InvoiceManager _invoiceManager=new InvoiceManager();
        readonly IReportManager _iReportManager;
        readonly InventoryManager _inventoryManager=new InventoryManager();

        readonly IVatManager _iVatManager;
        readonly IBranchManager _iBranchManager;
        readonly IOrderManager _iOrderManager;
        readonly IClientManager _iClientManager;

        public HomeController(IVatManager iVatManager,IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IReportManager iReportManager,IDepartmentManager iDepartmentManager,IEmployeeManager iEmployeeManager)
        {
            _iVatManager = iVatManager;
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
        }

        // GET: Corporate/Home
        public ActionResult Home() 
        {
           
            Session["BranchId"] = null;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _iBranchManager.GetAll();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByCompanyId(companyId) * -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByCompanyId(companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByCompanyId(companyId);
            var products = _inventoryManager.GetStockProductByCompanyId(companyId);
            var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
            var topClients = _iReportManager.GetTopClients().ToList();
            var clients = _iClientManager.GetAllClientDetails();
            var topProducts = _iReportManager.GetPopularBatteries().ToList(); 
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            SummaryModel summary = new SummaryModel
            {
                Branches = branches.ToList(),
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TotalSale = sales,
                TotalCollection = collection,
                OrderedAmount = orderedAmount,
                TopClients = topClients,
                Orders = orders,
                TopProducts = topProducts,
                Clients = clients,
                Employees = employees,
                Products = products
               
            };
            return View(summary);
        }
        public PartialViewResult BranchWiseSummary() 
        {
            var branches = _iBranchManager.GetAll().ToList().FindAll(n => n.BranchId != 9).ToList();
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
            List<Order> model = _iOrderManager.GetOrdersByBranchId(branchId).ToList();
            return PartialView("_ViewBranchWishOrderSummayPartialPage", model);
        }
        /// <summary>
        /// Sales reports 
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesSummary()
        {
            ViewOrderSearchModel model = new ViewOrderSearchModel ();
            ViewBag.BranchId= _iBranchManager.GetBranchSelectList();
            return View(model);
        }
        [HttpPost]
        public ActionResult SalesSummary(ViewOrderSearchModel model)
        {
            model.Orders = model.Orders.ToList().FindAll(n => n.BranchId == model.BranchId);
            ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }

        [HttpGet]
        public JsonResult GetOrders(int? branchId)  
        {
            var orders = _iOrderManager.GetOrdersByBranchId(Convert.ToInt32(branchId)).ToList();
            return Json(new { data = orders }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search()
        {
            ViewOrderSearchModel model=new ViewOrderSearchModel();
            ViewBag.BranchId = _iBranchManager.GetBranchSelectList();
            return View(model);
        }
        /// <summary>
        /// Get All Orders 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult All()
        {
            List<Order> model = _iOrderManager.GetAll().ToList();
            return PartialView("_ViewBranchWishOrderSummayPartialPage", model);
        }

        /// <summary>
        /// Get All Client 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewClient()
        {
            var clients = _iClientManager.GetAll().ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }
        /// <summary>
        /// Get client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult ViewClientProfile(int id)
        {
            var viewClient = _iClientManager.GetClientDeailsById(id); 
            return PartialView("_ViewClientProfilePartialPage", viewClient); 

        }
        public PartialViewResult ViewEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }
        /// <summary>
        /// Get client by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _iEmployeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }
        public PartialViewResult AllOrders()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }
        public PartialViewResult LatestOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByCompanyId(companyId).ToList().OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public PartialViewResult CurrentMonthOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderWithClientInformationByCompanyId(companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
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
            var order = _iOrderManager.GetOrderByOrderId(id);
            return View(order);
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAll().ToList();
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
            var departments = _iDepartmentManager.GetAll().ToList();
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
            IEnumerable<Vat> vats = _iVatManager.GetProductWishVat();
            return PartialView("_ViewProductWishVatPartialPage",vats);
        }

        public PartialViewResult ViewDiscount()
        {
            var discounts = _discountManager.GetAllDiscounts().ToList();
            return PartialView("_ViewDiscountPartialPage", discounts);
        }

        public ActionResult BusinessArea()
        {
            var branches = _iBranchManager.GetAll().ToList().Where(i => !i.BranchName.Contains("Corporate"));
            return View(branches);
        }

    }
}