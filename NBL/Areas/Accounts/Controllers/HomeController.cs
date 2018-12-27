
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.BLL;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Accounts.Controllers
{
    [Authorize(Roles = "Accounts")]
    public class HomeController : Controller
    {
        private readonly IClientManager _iClientManager;
        readonly IEmployeeManager _iEmployeeManager;
        readonly ProductManager _productManager = new ProductManager();
        readonly IBranchManager _iBranchManager;
        readonly IReportManager _iReportManager;
        readonly AccountsManager _accountsManager=new AccountsManager();

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IReportManager iReportManager,IEmployeeManager iEmployeeManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iReportManager = iReportManager;
            _iEmployeeManager = iEmployeeManager;
        }
        // GET: Accounts/Home
        public ActionResult Home()
        {

            var companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _iReportManager.GetTopClients().ToList();
            var batteries = _iReportManager.GetPopularBatteries().ToList();
            ViewTotalOrder totalOrder = _iReportManager.GetTotalOrderByBranchIdCompanyIdAndYear(branchId, companyId, DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(branchId, companyId) * -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(branchId, companyId);
            var branches = _iBranchManager.GetAllBranches();
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