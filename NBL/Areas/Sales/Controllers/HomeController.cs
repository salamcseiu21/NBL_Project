using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;


namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles ="User")]
    public class HomeController : Controller
    {
        readonly OrderManager _orderManager = new OrderManager();
        readonly ClientManager _clientManager = new ClientManager();
        readonly BranchManager _branchManager = new BranchManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly CommonGateway _commonGateway=new CommonGateway();
        readonly InventoryManager _inventoryManager=new InventoryManager();
        // GET: User/Home
        public ActionResult Home()
        {

            SummaryModel model=new SummaryModel();
            var user = (ViewUser)Session["user"];
            var branchId = Convert.ToInt32(Session["BranchId"]);
            var companyId = Convert.ToInt32(Session["CompanyId"]); 
            var delayedOrders = _orderManager.GetDelayedOrdersToSalesPersonByBranchAndCompanyId(branchId, companyId);
            model.DelayedOrders = delayedOrders;
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            model.Products = products;
            var clients = _clientManager.GetAllClientDetailsByBranchId(branchId).ToList();
            model.Clients = clients;
            model.PendingOrders = _orderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.UserId == user.UserId);
            model.Orders = orders;
            return View(model);

        }

        public PartialViewResult ViewClient()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var clients = _clientManager.GetClientByBranchId(branchId).ToList();
            return PartialView("_ViewClientPartialPage", clients);

        }


        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _clientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage", client);
        }

        public JsonResult GetClients()
        {


            List<Client> clients = _clientManager.GetAll.ToList();
            foreach (Client client in clients.ToList())
            {
                clients.Add(new Client
                {
                    ClientId = client.ClientId,
                    Address = client.Address,
                    ClientName = client.ClientName,
                    ClientImage = client.ClientImage,
                    Phone = client.Phone,
                    Email = client.Email,
                    AlternatePhone = client.AlternatePhone,
                    SubSubSubAccountCode = client.SubSubSubAccountCode
                });
            }

            return Json(clients, JsonRequestBehavior.AllowGet);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
        public ActionResult ViewEmployeeProfile(int id)
        {
            var employee = _employeeManager.GetEmployeeById(id);
            return View(employee);
        }

        public PartialViewResult All()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n=>n.Status==4).ToList().FindAll(n=>n.UserId== user.UserId);
            return PartialView("_OrdersSummaryPartialPage", orders);
        }

        public PartialViewResult CurrentMonthsOrder()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList().FindAll(n => n.Status == 4).FindAll(n=>n.OrderDate.Month.Equals(DateTime.Now.Month)).ToList().FindAll(n => n.UserId == user.UserId);
            return PartialView("_OrdersSummaryPartialPage", orders);
        }

        public ActionResult Test()
        {
            var list = _commonGateway.TestMethod();
            string message = "";
            foreach (dynamic item in list)
            {
                message += $"Product Name: {item.ProductName}, Vat {item.Vat} <br/>";
            }
            ViewBag.Data = message;
            Task.Run(
                DownloadPageAsync);
            return View();
        }

        public async Task<int?> DownloadPageAsync()
        {
            // ... Target page.
            string page = "http://en.wikipedia.org/";

            // ... Use HttpClient.
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
     
            return result.Length;
        }
    }
}