using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Admin.BLL;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;

namespace NBL.Areas.Manager.Controllers
{
    [Authorize(Roles = "Distributor")]
    public class HomeController : Controller
    {
        // GET: Manager/Home
       private readonly IClientManager _iClientManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly InvoiceManager _invoiceManager=new InvoiceManager();

        public HomeController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IInventoryManager iInventoryManager)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iInventoryManager = iInventoryManager;
        }
        public ActionResult Home() 
        {
            SummaryModel model=new SummaryModel();
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var invoicedOrders = _invoiceManager.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            var clients = _iClientManager.GetAllClientDetailsByBranchId(branchId);
            model.Clients = clients;
            model.InvoicedOrderList = invoicedOrders;
            model.Orders = _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId);
            model.Products = products;
            return View(model);
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

        public PartialViewResult Stock() 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage",products);
        }
        public PartialViewResult ViewBranch()
        {
            var branches = _iBranchManager.GetAllBranches().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }

    }
}