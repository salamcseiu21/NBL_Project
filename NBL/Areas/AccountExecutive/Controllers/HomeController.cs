using NBL.Areas.Accounts.BLL;
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;

namespace NBL.Areas.AccountExecutive.Controllers
{
    [Authorize(Roles = "AccountExecutive")]
    public class HomeController : Controller
    {
        readonly IClientManager _iClientManager;
        readonly IBranchManager _iBranchManager;
        readonly IEmployeeManager _iEmployeeManager;
        readonly ProductManager _productManager = new ProductManager();
       
        readonly AccountsManager _accountsManager = new AccountsManager();

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IEmployeeManager iEmployeeManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iEmployeeManager = iEmployeeManager;
        }
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
            var clients = _iClientManager.GetClientByBranchId(branchId);
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

        public PartialViewResult ViewProduct()
        {
            var products = _productManager.GetAll.ToList();
            return PartialView("_ViewProductPartialPage",products);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
    }
}