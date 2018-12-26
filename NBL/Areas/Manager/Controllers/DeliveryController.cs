using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Areas.Manager.BLL;
using NBL.Areas.Admin.BLL;
using System.Collections.Generic;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Manager.Controllers
{
    [Authorize(Roles = "Distributor")]
    public class DeliveryController : Controller
    {

        private readonly InvoiceManager _invoiceManager = new InvoiceManager();
        private readonly IInventoryManager _iInventoryManager;
        private readonly ProductManager _productManager = new ProductManager();
        private readonly IDeliveryManager _iDeliveryManager;

        public DeliveryController(IDeliveryManager iDeliveryManager,IInventoryManager iInventoryManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
        }
        public ActionResult OrderList()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _invoiceManager.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            return View(invoicedOrders);
        }
       
        [HttpGet]
        public ActionResult Delivery(int id)
        {
            var invoice = _invoiceManager.GetInvoicedOrderByInvoiceId(id);
            var invoicedOrders = _invoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
            ViewBag.Invoice = invoice;
            return View(invoicedOrders);
        }

        [HttpPost]
        public ActionResult Delivery(FormCollection collection)
        {
            try
            {


                int deliverebyUserId = ((ViewUser)Session["user"]).UserId;
                int branchId = Convert.ToInt32(Session["BranchId"]);
                int invoiceId = Convert.ToInt32(collection["InvoiceId"]);
                var invoice = _invoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                var invoicedOrders = _invoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                int invoiceQty = invoicedOrders.Sum(n => n.InvoicedQuantity);

                List<InvoiceDetails> invoiceList = new List<InvoiceDetails>();

                foreach (var item in invoicedOrders)
                {
                    ProductDetails aProduct = _productManager.GetProductDetailsByProductId(item.ProductId);
                    item.UnitPrice = aProduct.UnitPrice;
                    string qtyOf = "QtyOf_" + item.ProductId;
                    int qty = Convert.ToInt32(collection[qtyOf]);
                    if (qty > 0)
                    {
                        item.Quantity = qty;
                        item.StockQuantity = _iInventoryManager.GetStockQtyByBranchAndProductId(branchId, item.ProductId) - item.Quantity;
                        invoiceList.Add(item);
                    }

                }
                int diliveryQty = invoicedOrders.Sum(n => n.DeliveredQuantity) + invoiceList.Sum(n => n.Quantity);
                int invoiceStatus = 1;
                int orderStatus = 3;
                if (invoiceQty == diliveryQty)
                {
                    invoiceStatus = 2;
                    orderStatus = 4;
                }

                Delivery aDelivery = new Delivery
                {
                    TransactionRef = invoice.TransactionRef,
                    InvoiceRef = invoice.InvoiceRef,
                    DeliveredByUserId = deliverebyUserId,
                    Transportation = collection["Transportation"],
                    DriverName = collection["DriverName"],
                    DriverPhone = collection["DriverPhone"],
                    TransportationCost = Convert.ToDecimal(collection["TransportationCost"]),
                    VehicleNo = collection["VehicleNo"],
                    DeliveryDate = Convert.ToDateTime(collection["DeliveryDate"]).Date,
                    CompanyId = invoice.CompanyId,
                    ToBranchId = invoice.BranchId,
                    FromBranchId = invoice.BranchId
                };
                string result = _iInventoryManager.Save(invoiceList, aDelivery, invoiceStatus,orderStatus);
                if (result.StartsWith("S"))
                {
                    return RedirectToAction("OrderList");
                }
                return View();
            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message;
                //return View("Delivery");
                throw new Exception();
            }
        }

        public ActionResult Calan(int id)
        {
            var chalan = _iDeliveryManager.GetChalanByDeliveryId(id);
            return View(chalan);

        }
        public ActionResult DeleveredOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var user = (ViewUser) Session["user"];
            var orders = _iDeliveryManager.GetAllDeliveredOrdersByBranchCompanyAndUserId(branchId,companyId,user.UserId).ToList();
            return View(orders);
        }
    }
}