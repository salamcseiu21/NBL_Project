
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.BLL;
using NBL.BLL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Accounts.Controllers
{
    [Authorize(Roles = "Accounts")]
    public class HomeController : Controller
    {
        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly ProductManager _productManager = new ProductManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly ReportManager _reportManager=new ReportManager();
        readonly AccountsManager _accountsManager=new AccountsManager();
        // GET: Accounts/Home
        public ActionResult Home()
        {

            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _reportManager.GetTopClients().ToList();
            var batteries = _reportManager.GetPopularBatteries().ToList();
            ViewTotalOrder totalOrder = _reportManager.GetTotalOrderByBranchIdCompanyIdAndYear(branchId, companyId, DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(branchId, companyId) * -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var branches = _branchManager.GetAll();
            SummaryModel aModel = new SummaryModel
            {
                Branches = branches.ToList(),
                BranchId = branchId,
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TotalSale = sales,
                TotalCollection = collection,
                OrderedAmount = orderedAmount,
                Clients = clients,
                Products = batteries

            };

            return View(aModel);
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