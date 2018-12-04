using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;
using NBL.Areas.Admin.BLL;
using NBL.Areas.Manager.BLL;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
     
        readonly  OrderManager _orderManager=new OrderManager();
        readonly InvoiceManager _invoiceManager = new InvoiceManager();
        readonly DeliveryManager _deliveryManager = new DeliveryManager(); 
        readonly ClientManager _clientManager = new ClientManager();
        public PartialViewResult All()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }
        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }
        public ActionResult PendingOrder()
        {
            //------------- Status=1 means the order is approved by NSM and it is pending at admin  stage----------
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 1).ToList();
            return View(orders);

        }

        // GET: Accounts/Order/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        //---Approved order by Accounts/Admin
        public ActionResult Approve(int id) 
        {
            var order = _orderManager.GetOrderByOrderId(id);
            List<OrderDetails> orders = _orderManager.GetOrderDetailsByOrderId(id).ToList();
            ViewBag.AOrders = orders;
            return View(order);

        }
        [HttpPost]
        public ActionResult Approve(int id, FormCollection collection) 
        {
            try
            {

                int branchId = Convert.ToInt32(Session["BranchId"]);
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                var anUser = (ViewUser)Session["user"];
                var order = _orderManager.GetOrderByOrderId(id);
                Client client = _clientManager.GetClientById(order.ClientId);
                //List<OrderDetails> orders = _orderManager.GetOrderDetailsByOrderId(id).ToList();
                decimal specialDiscount = Convert.ToDecimal(collection["Discount"]);
                Invoice anInvoice = new Invoice
                {
                    InvoiceDateTime = DateTime.Now,
                    CompanyId = companyId,
                    BranchId = branchId,
                    ClientId=order.ClientId,
                    Amounts=order.Amounts,
                    Discount=order.Discount,
                    SpecialDiscount=specialDiscount,
                    InvoiceByUserId = anUser.UserId,
                    TransactionRef = order.OrederRef,
                    ClientAccountCode = client.SubSubSubAccountCode,
                    Explanation = "Credit sale by " + anUser.UserId,
                    DiscountAccountCode = _orderManager.GetDiscountAccountCodeByClintTypeId(client.ClientTypeId)
                };
                string invoice = _invoiceManager.Save(order.OrderItems, anInvoice);
                //---------- Status=2 means approved by Admin
                order.Status = 2;
                order.SpecialDiscount = specialDiscount;
                order.AdminUserId = anUser.UserId;
                string result = _orderManager.ApproveOrderByAdmin(order);
                ViewBag.Message = result;
                return RedirectToAction("PendingOrder");
            }
            catch(Exception exception)
            {
                string messge = exception.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Invoice(int id)
        {

            var invocedOrder = _invoiceManager.GetInvoicedOrderByInvoiceId(id);
            var orderInfo = _orderManager.GetOrderInfoByTransactionRef(invocedOrder.TransactionRef);
            IEnumerable<InvoiceDetails> details = _invoiceManager.GetInvoicedOrderDetailsByInvoiceId(id);
            var client = _clientManager.GetClientDeailsById(orderInfo.ClientId);

            ViewInvoiceModel model=new ViewInvoiceModel
            {
                Client = client,
                Order = orderInfo,
                Invoice = invocedOrder,
                InvoiceDetailses = details

            };
            return View(model);

        }

        public ActionResult ViewAllDeliveredOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var orders = _deliveryManager.GetAllDeliveredOrders().ToList().FindAll(n => n.ToBranchId == branchId).ToList().DistinctBy(n => n.TransactionRef).ToList();
            return View(orders);
        }

        public ActionResult ViewTodaysDeliverdOrders() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var orders = _deliveryManager.GetAllDeliveredOrders().ToList().FindAll(n => n.ToBranchId == branchId).ToList();
            return View(orders);
        }


        public ActionResult InvoicedOrderList()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _invoiceManager.GetAllInvoicedOrdersByBranchCompanyAndUserId(branchId,companyId,user.UserId).ToList().FindAll(n => n.InvoiceDateTime.Date.Equals(DateTime.Now.Date));
            return View(orders);
        }

        public ActionResult Cancel(int id)
        {
            var order = _orderManager.GetOrderByOrderId(id);
            return View(order);
        }
        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {

            //---------Status=7 means order cancel by Admin------------------

            var user = (ViewUser)Session["user"];
            int orderId = Convert.ToInt32(collection["OrderId"]);
            var order = _orderManager.GetOrderByOrderId(orderId);
            order.ResonOfCancel = collection["Reason"];
            order.CancelByUserId = user.UserId;
            order.Status = 7;
            var status = _orderManager.CancelOrder(order);
            return status ? RedirectToAction("All") : RedirectToAction("Cancel", orderId);

        }
    }
}