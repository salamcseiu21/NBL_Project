﻿
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Accounts.BLL.Contracts;

using NBL.BLL.Contracts;

namespace NBL.Areas.AccountExecutive.Controllers
{
    [Authorize(Roles = "AccountExecutive")]
    public class HomeController : Controller
    {
        private readonly IClientManager _iClientManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IProductManager _iProductManager;
        private readonly IAccountsManager _iAccountsManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IEmployeeManager iEmployeeManager,IProductManager iProductManager,IAccountsManager iAccountsManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iEmployeeManager = iEmployeeManager;
            _iProductManager = iProductManager;
            _iAccountsManager = iAccountsManager;
        }
        // GET: AccountExecutive/Home
        public ActionResult Home() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var receivableCheques = _iAccountsManager.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId);
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
            var products = _iProductManager.GetAll().ToList();
            return PartialView("_ViewProductPartialPage",products);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
    }
}