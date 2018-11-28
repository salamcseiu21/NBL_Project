﻿using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Nsm.Controllers
{
    [Authorize(Roles = "Nsm")]
    public class OrderController : Controller
    {
        // GET: Nsm/Order

        readonly OrderManager _orderManager=new OrderManager();
        readonly ClientManager _clientManager = new ClientManager();
        readonly InventoryManager _inventoryManager = new InventoryManager();
        readonly ProductManager _productManager = new ProductManager();
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
            //------------- Status=0 means the order is at initial stage----------

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId,companyId,0).ToList();
            return View(orders);

        }

       

        [HttpPost]
        public ActionResult AddNewItemToExistingOrder(FormCollection collection)
        {
            int orderId = Convert.ToInt32(collection["OrderId"]);
            List<OrderDetails> orders = (List<OrderDetails>)Session["TOrders"];
            try
            {
                var ord = _orderManager.GetOrderByOrderId(orderId);
                Client aClient = _clientManager.GetClientById(ord.ClientId);
                int productId = Convert.ToInt32(collection["ProductId"]);
                var aProduct = _productManager.GetProductByProductAndClientTypeId(productId, aClient.ClientTypeId);
                aProduct.Quantity = Convert.ToInt32(collection["Quantity"]);

                OrderDetails order = orders.Find(n => n.ProductId == productId);
                if (order != null)
                {
                    ViewBag.Result = "This product already is in the list!";
                }
                else
                {
                    int rowAffected = _orderManager.AddNewItemToExistingOrder(aProduct,orderId);
                    ViewBag.Result = "1 new Item added successfully!";
                }

                return RedirectToAction("Edit", new
                {
                    id = orderId
                });

            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                return RedirectToAction("Edit", new
                {
                    id = orderId
                });
            }
        }
        //---Eidt and approved the order-------
        public ActionResult Edit(int id)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var order = _orderManager.GetOrderByOrderId(id);
           List<OrderDetails> orders = _orderManager.GetOrderDetailsByOrderId(id).ToList();
            //List<Product> productList = _orderManager.GetProductListByOrderId(id);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.Products = products;
            Session["TOrders"] = orders;
            return View(order);

        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                
                var user = Session["user"];
                decimal amount = Convert.ToDecimal(collection["Amount"]);
                var dicount = Convert.ToDecimal(collection["Discount"]);
                var order = _orderManager.GetOrderByOrderId(id);
                List<OrderDetails> orders = (List<OrderDetails>)Session["TOrders"];
                order.Discount = orders.Sum(n => n.Quantity * n.DiscountAmount);
                order.Vat = orders.Sum(n=>n.Vat * n.Quantity);
                order.Amounts = amount + order.Discount;
                order.Status = 1; //----- Status=1 means approve by NSM
                order.SpecialDiscount = dicount;
                order.ApprovedByNsmDateTime = DateTime.Now;
                string r = _orderManager.UpdateOrderDetails(orders);
                order.NsmUserId = ((User)user).UserId;
                string result = _orderManager.ApproveOrderByNsm(order);
                ViewBag.Message = result;
                return RedirectToAction("PendingOrder");
            }
            catch (Exception exception)
            {
                string messge = exception.Message;
                return View();
            }
        }


        public void Update(FormCollection collection)
        {
            try
            {
                List<OrderDetails> orders = (List<OrderDetails>)Session["TOrders"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {
                  

                    var anOrder = orders.Find(n => n.ProductId == pid);
                    orders.Remove(anOrder);
                    var rowAffected= _orderManager.DeleteProductFromOrderDetails(anOrder.OrderDetailsId);
                    Session["TOrders"] = orders;
                    ViewBag.Orders = orders;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("product"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("product_Id_", "");
                        var user = (User)Session["user"];
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var anOrder = orders.Find(n => n.ProductId == productId);


                        if (anOrder != null)
                        {
                            orders.Remove(anOrder);
                            anOrder.Quantity = qty;
                            orders.Add(anOrder);
                            Session["TOrders"] = orders;
                            ViewBag.Orders = orders;
                        }

                    }
                }


                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).DistinctBy(n => n.ProductId).ToList();
                ViewBag.Products = products;

            }
            catch (Exception e)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).DistinctBy(n => n.ProductId).ToList();
                ViewBag.Products = products;
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }


        public JsonResult GetTempOrders()
        {
            if (Session["TOrders"] != null)
            {
                IEnumerable<OrderDetails> orders = ((List<OrderDetails>)Session["TOrders"]).ToList();
                return Json(orders, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Product>(), JsonRequestBehavior.AllowGet);
        }

        //--Cancel Order---


        public ActionResult Cancel(int id)
        {
            Order anOrder = _orderManager.GetOrderByOrderId(id);
            return View(anOrder);
        }
        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {

            //---------Status=6 means order cancel by NSM------------------
            var user = (User)Session["user"];
            int orderId = Convert.ToInt32(collection["OrderId"]);
            Order order = _orderManager.GetOrderByOrderId(orderId);
            order.CancelByUserId = user.UserId;
            order.ResonOfCancel = collection["Reason"];
            order.Status = 6;
            int status = _orderManager.CancelOrder(order);
            return RedirectToAction("PendingOrder");

        }
        public ActionResult OrderList()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            User user = (User) Session["user"];
            var orders = _orderManager.GetOrdersByBranchCompanyAndNsmUserId(branchId,companyId,user.UserId).ToList().OrderByDescending(n => n.OrderId).ToList().FindAll(n => n.Status < 2).ToList();
            return View(orders);
        }

        [HttpGet]
        public ActionResult OrderSlip(int id)
        {

            var order = _orderManager.GetOrderByOrderId(id);
            var orderDetails = _orderManager.GetOrderDetailsByOrderId(id);
            var client = _clientManager.GetClientDeailsById(order.ClientId);
            ViewBag.Client = client;
            ViewBag.Order = order;
            return View(orderDetails);

        }

        public JsonResult GetPendingOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = _orderManager.GetOrdersByBranchAndCompnayId(branchId,companyId).ToList().FindAll(n => n.Status == 0).ToList().Count();
            return Json(stock, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            var productList = (from c in products
                where c.ProductName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.ProductName,
                    val = c.ProductId
                }).ToList();

            return Json(productList);
        }
    }
}