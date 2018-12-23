
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Admin.BLL;
using NBL.BLL;
using NBL.Models;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {

        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly OrderManager _orderManager = new OrderManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly InvoiceManager _invoiceManager = new InvoiceManager();
        readonly InventoryManager _inventoryManager=new InventoryManager();
        // GET: Admin/Home
        public ActionResult Home() 
        {
            
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _invoiceManager.GetAllInvoicedOrdersByCompanyId(companyId).ToList().FindAll(n=>n.BranchId==branchId).ToList();
            var pendingOrders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList().FindAll(n => n.Status == 1).ToList();
            var clients = _clientManager.GetAllClientDetailsByBranchId(branchId).ToList();
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var delayedOrders = _orderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
            var verifiedOrders = _orderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId,companyId);
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
            var employees = _employeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }

        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _employeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }

        public PartialViewResult Stock() 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage",products);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }

        public PartialViewResult VerifyingOrders()
        {
            int branchId = Convert.ToInt32(Session["branchId"]);
            int companyId = Convert.ToInt32(Session["companyId"]);
            var verifiedOrders = _orderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId, companyId);
            return PartialView("_ViewVerifyingOrdersPartialPage",verifiedOrders);
        }
    }
}