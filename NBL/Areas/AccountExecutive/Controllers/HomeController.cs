using NBL.Areas.Accounts.BLL;
using System;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;

namespace NBL.Areas.AccountExecutive.Controllers
{
    [Authorize(Roles = "AccountExecutive")]
    public class HomeController : Controller
    {
        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly ProductManager _productManager = new ProductManager();
        readonly OrderManager _orderManager = new OrderManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly AccountsManager _accountsManager = new AccountsManager();
        // GET: AccountExecutive/Home
        public ActionResult Home() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var receivableCheques = _accountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId);
            return View(receivableCheques);
        }
        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _clientManager.GetClientByBranchId(branchId);
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

        public PartialViewResult ViewProduct()
        {
            var products = _productManager.GetAll.ToList();
            return PartialView("_ViewProductPartialPage",products);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
    }
}