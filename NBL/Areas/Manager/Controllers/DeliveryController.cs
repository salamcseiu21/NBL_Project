﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Manager.Controllers
{
    [Authorize(Roles = "Distributor")]
    public class DeliveryController : Controller
    {

        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly IDeliveryManager _iDeliveryManager;
        private readonly IClientManager _iClientManager;

        public DeliveryController(IDeliveryManager iDeliveryManager,IInventoryManager iInventoryManager,IProductManager iProductManager,IClientManager iClientManager,IInvoiceManager iInvoiceManager)
        {
            _iDeliveryManager = iDeliveryManager;
            _iInventoryManager = iInventoryManager;
            _iProductManager = iProductManager;
            _iClientManager = iClientManager;
            _iInvoiceManager = iInvoiceManager;
        }
        public ActionResult OrderList()
        {
            SummaryModel model=new SummaryModel();
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoicedOrders = _iInvoiceManager.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId).ToList();
            foreach (var invoice in invoicedOrders)
            {
                invoice.Client = _iClientManager.GetById(invoice.ClientId);
            }
            model.InvoicedOrderList = invoicedOrders;
            return View(model);
        }
       
        [HttpGet]
        public ActionResult Delivery(int id)
        {
            var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(id);
            invoice.Client = _iClientManager.GetById(invoice.ClientId);
            var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
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
                var invoice = _iInvoiceManager.GetInvoicedOrderByInvoiceId(invoiceId);
                var invoicedOrders = _iInvoiceManager.GetInvoicedOrderDetailsByInvoiceRef(invoice.InvoiceRef).ToList();
                int invoiceQty = invoicedOrders.Sum(n => n.InvoicedQuantity);

                List<InvoiceDetails> invoiceList = new List<InvoiceDetails>();

                foreach (var item in invoicedOrders)
                {
                    ProductDetails aProduct = _iProductManager.GetProductDetailsByProductId(item.ProductId);
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