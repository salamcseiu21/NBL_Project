﻿using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class HomeController : Controller
    {

        readonly IClientManager _iClientManager;
        readonly IEmployeeManager _iEmployeeManager;
        readonly ProductManager _productManager = new ProductManager();
        readonly IDepartmentManager _iDepartmentManager;
        readonly IBranchManager _iBranchManager;
        readonly IRegionManager _iRegionManager;
        readonly ITerritoryManager _iTerritoryManager;

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IDepartmentManager iDepartmentManager,IEmployeeManager iEmployeeManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iDepartmentManager = iDepartmentManager;
            _iEmployeeManager = iEmployeeManager;
            _iRegionManager = iRegionManager;
            _iTerritoryManager = iTerritoryManager;
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
            var products = _productManager.GetAll.ToList();
            return View(products);

        }

        public ActionResult BusinessArea()
        {
            return View();
        }
    }
}