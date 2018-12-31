﻿using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class HomeController : Controller
    {

        private readonly IClientManager _iClientManager;
        private readonly IEmployeeManager _iEmployeeManager;
        private readonly IProductManager _iProductManager;
        private readonly IDepartmentManager _iDepartmentManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IDepartmentManager iDepartmentManager,IEmployeeManager iEmployeeManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IProductManager iProductManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
            _iProductManager = iProductManager;
        }
        // GET: Editor/Home
        public ActionResult Home() 
        {
            SummaryModel model = new SummaryModel
            {
                Clients = _iClientManager.GetAllClientDetails().ToList(),
                Employees = _iEmployeeManager.GetAllEmployeeWithFullInfo(),
                Departments = _iDepartmentManager.GetAll(),
                Branches = _iBranchManager.GetAllBranches(),
                Regions = _iRegionManager.GetAll(),
                Territories = _iTerritoryManager.GetAll()
            };
            return View(model);
        }

        public ActionResult ViewClient()
        {
            var clients = _iClientManager.GetAll().ToList();
            return View(clients);

        }

        public ActionResult ViewEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo().ToList();
            return View(employees);

        }

        public ActionResult ViewProduct()
        {
            var products = _iProductManager.GetAll().ToList();
            return View(products);

        }

        public ActionResult BusinessArea()
        {
            return View();
        }
    }
}