using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Admin.BLL;
using NBL.Areas.Manager.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
     
        readonly  IOrderManager _iOrderManager;
        readonly InvoiceManager _invoiceManager = new InvoiceManager();
        readonly IDeliveryManager _iDeliveryManager; 
        readonly IClientManager _iClientManager;

        public OrderController(IOrderManager iOrderManager ,IClientManager iClientManager,IDeliveryManager iDeliveryManager)
        {
            _iOrderManager = iOrderManager;
            _iClientManager = iClientManager;
            _iDeliveryManager = iDeliveryManager;
        }
        public PartialViewResult All()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }
        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage", orders);
        }
        public ActionResult PendingOrder()
        {
            //------------- Status=1 means the order is approved by NSM and it is pending at admin  stage----------
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 1).ToList();
            return View(orders);

        }

        public ActionResult DelayedOrders()
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetDelayedOrdersToAdminByBranchAndCompanyId(branchId, companyId);
            return View(orders);
        }
        //---Approved order by Accounts/Admin
        public ActionResult Approve(int id) 
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
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
                var order = _iOrderManager.GetOrderByOrderId(id);
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
                    ClientAccountCode = order.Client.SubSubSubAccountCode,
                    Explanation = "Credit sale by " + anUser.UserId,
                    DiscountAccountCode = _iOrderManager.GetDiscountAccountCodeByClintTypeId(order.Client.ClientTypeId)
                };
                string invoice = _invoiceManager.Save(order.OrderItems, anInvoice);
                //---------- Status=2 means approved by Admin
                order.Status = 2;
                order.SpecialDiscount = specialDiscount;
                order.AdminUserId = anUser.UserId;
                string result = _iOrderManager.ApproveOrderByAdmin(order);
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
            var orderInfo = _iOrderManager.GetOrderInfoByTransactionRef(invocedOrder.TransactionRef);
            IEnumerable<InvoiceDetails> details = _invoiceManager.GetInvoicedOrderDetailsByInvoiceId(id);
            var client = _iClientManager.GetClientDeailsById(orderInfo.ClientId);

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
            var orders = _iDeliveryManager.GetAllDeliveredOrders().ToList().FindAll(n => n.ToBranchId == branchId).ToList().DistinctBy(n => n.TransactionRef).ToList();
            return View(orders);
        }

        public ActionResult ViewTodaysDeliverdOrders() 
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var orders = _iDeliveryManager.GetAllDeliveredOrders().ToList().FindAll(n => n.ToBranchId == branchId).ToList();
            return View(orders);
        }


        public ActionResult InvoicedOrderList()
        {
            var user = (ViewUser)Session["user"];
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _invoiceManager.GetAllInvoicedOrdersByBranchCompanyAndUserId(branchId,companyId,user.UserId).ToList();
            return View(orders);
        }

        public ActionResult Cancel(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            return View(order);
        }
        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {

            //---------Status=7 means order cancel by Admin------------------

            var user = (ViewUser)Session["user"];
            int orderId = Convert.ToInt32(collection["OrderId"]);
            var order = _iOrderManager.GetOrderByOrderId(orderId);
            order.ResonOfCancel = collection["Reason"];
            order.CancelByUserId = user.UserId;
            order.Status = 7;
            var status = _iOrderManager.CancelOrder(order);
            return status ? RedirectToAction("All") : RedirectToAction("Cancel", new {id= orderId });

        }

        public ActionResult Verify(int id)
        {
            var order = _iOrderManager.GetOrderByOrderId(id);
            return View(order);
        }
        [HttpPost]
        public ActionResult Verify(FormCollection collection)
        {
            int orderId = Convert.ToInt32(collection["OrderId"]);
            string notes = collection["VerificationNote"];
            var user = (ViewUser)Session["user"];
            bool result = _iOrderManager.UpdateVerificationStatus(orderId,notes,user.UserId);
            if (result)
            {
                return RedirectToAction("PendingOrder");
            }
           return RedirectToAction("PendingOrder");
            
        }
    }
}