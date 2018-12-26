﻿
using System;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;

namespace NBL.Areas.Nsm.Controllers
{
    [Authorize(Roles ="Nsm")]
    public class HomeController : Controller
    {
        // GET: Nsm/Home
        readonly IClientManager _iClientManager;
        readonly IEmployeeManager _iEmployeeManager;
        readonly IOrderManager _iOrderManager;
        readonly IBranchManager _iBranchManager;
        readonly InventoryManager _inventoryManager = new InventoryManager();

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IEmployeeManager iEmployeeManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iEmployeeManager = iEmployeeManager;
        }
        public ActionResult Home() 
        {
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId).ToList().FindAll(n => n.Status == 4);
            var delayedOrders = _iOrderManager.GetDelayedOrdersToNsmByBranchAndCompanyId(branchId, companyId);
            var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId).ToList();
            var pendingorders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0).ToList();
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var verifiedOrders = _iOrderManager.GetVerifiedOrdersByBranchAndCompanyId(branchId,companyId);

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
            var clients = _iClientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }

        public PartialViewResult ViewClientProfile(int id)
        {
            var aClient = _iClientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage",aClient);

        }

        public PartialViewResult ViewEmployee()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfoByBranchId(branchId).ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }

        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _iEmployeeManager.GetEmployeeById(id);
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
            var branches = _iBranchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
    }
}