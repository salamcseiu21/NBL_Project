using System;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;
using NBL.Areas.Admin.BLL;

namespace NBL.Areas.Manager.Controllers
{
    [Authorize(Roles = "Distributor")]
    public class HomeController : Controller
    {
        // GET: Manager/Home
        readonly ClientManager _clientManager=new ClientManager();
        readonly OrderManager _orderManager =new OrderManager();
        readonly  BranchManager _branchManager=new BranchManager();
        readonly InventoryManager _inventoryManager=new InventoryManager();
        readonly InvoiceManager _invoiceManager=new InvoiceManager();
        public ActionResult Home() 
        {
            SummaryModel model=new SummaryModel();
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var invoicedOrders = _invoiceManager.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            var clients = _clientManager.GetAllClientDetailsByBranchId(branchId);
            model.Clients = clients;
            model.InvoicedOrderList = invoicedOrders;
            model.Orders = _orderManager.GetOrdersByBranchAndCompnayId(branchId, companyId);
            model.Products = products;
            return View(model);
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

        public PartialViewResult Stock() 
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