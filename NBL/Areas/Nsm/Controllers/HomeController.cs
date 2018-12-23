
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models;

namespace NBL.Areas.Nsm.Controllers
{
    [Authorize(Roles ="Nsm")]
    public class HomeController : Controller
    {
        // GET: Nsm/Home
        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly OrderManager _orderManager = new OrderManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly InventoryManager _inventoryManager = new InventoryManager();
        public ActionResult Home() 
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetOrdersByBranchAndCompnayId(branchId, companyId).ToList().FindAll(n => n.Status == 4);
            var delayedOrders = _orderManager.GetDelayedOrdersToNsmByBranchAndCompanyId(branchId, companyId);
            var clients = _clientManager.GetAllClientDetailsByBranchId(branchId).ToList();
            var pendingorders = _orderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0).ToList();
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var verifiedOrders = _orderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId,companyId);

            SummaryModel summary = new SummaryModel
            {
                BranchId = branchId,
                CompanyId = companyId,
                Orders = orders,
                Clients =clients,
                DelayedOrders = delayedOrders,
                PendingOrders = pendingorders,
                Products = products,
                VerifiedOrders = verifiedOrders
            };
            return View(summary);
        }


        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _clientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }

        public PartialViewResult ViewClientProfile(int id)
        {
            var aClient = _clientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage",aClient);

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


        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var stocks = products;
            return PartialView("_ViewStockProductInBranchPartialPage", stocks);
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
    }
}