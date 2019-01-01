
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.BLL.Contracts;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Corporate.Controllers
{
    [Authorize(Roles = "Corporate")]
    public class HomeController : Controller
    {

        private readonly IEmployeeManager _iEmployeeManager;
        private readonly ICommonManager _iCommonManager;
        private readonly IDivisionGateway _iDivisionGateway;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly IAccountsManager _iAccountsManager;
        private readonly IDepartmentManager _iDepartmentManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IReportManager _iReportManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IVatManager _iVatManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;

        public HomeController(IVatManager iVatManager,IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IReportManager iReportManager,IDepartmentManager iDepartmentManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IDiscountManager iDiscountManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IAccountsManager iAccountsManager,IInvoiceManager iInvoiceManager,IDivisionGateway iDivisionGateway)
        {
            _iVatManager = iVatManager;
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iReportManager = iReportManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iAccountsManager = iAccountsManager;
            _iInvoiceManager = iInvoiceManager;
            _iDivisionGateway = iDivisionGateway;
        }

        // GET: Corporate/Home
        public ActionResult Home() 
        {
           
            Session["BranchId"] = null;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _iBranchManager.GetAllBranches();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            var sales = _iAccountsManager.GetTotalSaleValueOfCurrentMonthByCompanyId(companyId) * -1;
            var collection = _iAccountsManager.GetTotalCollectionOfCurrentMonthByCompanyId(companyId);
            var orderedAmount = _iAccountsManager.GetTotalOrderedAmountOfCurrentMonthByCompanyId(companyId);
            var products = _iInventoryManager.GetStockProductByCompanyId(companyId);
            var orders = _iOrderManager.GetOrdersByCompanyId(companyId).ToList();
            var topClients = _iReportManager.GetTopClientsByYear(DateTime.Now.Year).ToList();
            var clients = _iClientManager.GetAllClientDetails();
            var topProducts = _iReportManager.GetPopularBatteriesByYear(DateTime.Now.Year).ToList(); 
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
            var branches = _iBranchManager.GetAllBranches().ToList().FindAll(n => n.BranchId != 9).ToList();
            foreach (ViewBranch branch in branches)
            {
                branch.Orders = _iOrderManager.GetOrdersByBranchId(branch.BranchId).ToList();
            }
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList(); 
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
            foreach (Order order in model)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
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
            foreach (ViewOrder order in orders)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }
        public PartialViewResult LatestOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByCompanyId(companyId).ToList().OrderByDescending(n => n.OrderId).ToList();
            foreach (ViewOrder order in orders)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }

        public PartialViewResult CurrentMonthOrders()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderWithClientInformationByCompanyId(companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n => n.OrderDate.Month.Equals(DateTime.Now.Month));
            foreach (ViewOrder order in orders)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
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
            order.Client = _iClientManager.GetById(order.ClientId);
            return View(order);
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
        public PartialViewResult ViewDivision()
        {
            var divisions = _iDivisionGateway.GetAll().ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);

        }
        public PartialViewResult ViewRegion()
        {
            var regions = _iRegionManager.GetAll().ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            var territories = _iTerritoryManager.GetAll().ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);

        }
        public PartialViewResult Supplier()
        {
            var suppliers = _iCommonManager.GetAllSupplier().ToList();
            return PartialView("_ViewSupplierPartialPage",suppliers);
        }

        public PartialViewResult ViewJournal()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var journals = _iAccountsManager.GetAllJournalVouchersByCompanyId(companyId).ToList();
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
            var vouchers = _iAccountsManager.GetVoucherListByCompanyId(companyId);
            return PartialView("_ViewVouchersPartialPage",vouchers);
        }

        public ActionResult VoucherPreview(int id)
        {
            var voucher = _iAccountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _iAccountsManager.GetVoucherDetailsByVoucherId(id);
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
            var discounts = _iDiscountManager.GetAll().ToList();
            return PartialView("_ViewDiscountPartialPage", discounts);
        }

        public ActionResult BusinessArea()
        {
            var branches = _iBranchManager.GetAllBranches().ToList().Where(i => !i.BranchName.Contains("Corporate"));
            foreach (var branch in branches)
            {
                branch.RegionList = _iRegionManager.GetAssignedRegionListToBranchByBranchId(branch.BranchId);
            }
            return View(branches);
        }

    }
}