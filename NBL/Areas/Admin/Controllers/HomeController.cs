
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Admin.BLL;
using NBL.BLL.Contracts;
using NBL.Models;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {

        readonly IClientManager _iClientManager;
        readonly IEmployeeManager _iEmployeeManager;
        readonly IOrderManager _iOrderManager;
        readonly IBranchManager _iBranchManager;
        readonly InvoiceManager _invoiceManager = new InvoiceManager();
        readonly IInventoryManager _iInventoryManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IEmployeeManager iEmployeeManager,IInventoryManager iInventoryManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
            _iInventoryManager = iInventoryManager;
        }
        // GET: Admin/Home
        public ActionResult Home() 
        {
            
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _invoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList().FindAll(n=>n.BranchId==branchId).ToList();
            var pendingOrders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList().FindAll(n => n.Status == 1).ToList();
            var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var delayedOrders = _iOrderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
            var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId,companyId);
            SummaryModel model = new SummaryModel
            {
                InvoicedOrderList = orders,
                PendingOrders = pendingOrders,
                Clients = clients,
                Products = products,
                DelayedOrders = delayedOrders,
                VerifiedOrders = verifiedOrders
            };
            return View(model);
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
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }

        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _iEmployeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }

        public PartialViewResult Stock() 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage",products);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }

        public PartialViewResult VerifyingOrders()
        {
            int branchId = Convert.ToInt32(Session["branchId"]);
            int companyId = Convert.ToInt32(Session["companyId"]);
            var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
            return PartialView("_ViewVerifyingOrdersPartialPage",verifiedOrders);
        }
    }
}