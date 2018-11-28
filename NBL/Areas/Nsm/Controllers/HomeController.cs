
using System;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

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
            SummaryModel summary = new SummaryModel
            {
                BranchId = branchId,
                CompanyId = companyId,
                Orders = orders,
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

        public PartialViewResult ViewProduct()
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
    }
}