using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;


namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles ="User")]
    public class OrderController : Controller
    {
        readonly ProductManager _productManager = new ProductManager();
        readonly OrderManager _orderManager = new OrderManager();
        readonly ClientManager _clientManager = new ClientManager();
        readonly InventoryManager _inventoryManager = new InventoryManager();
      
        public PartialViewResult All()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId,companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }
        public ActionResult Order()
        {
            Session["ProductList"] = null;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return View(products);
        }

        [HttpPost]
        public ActionResult Order(FormCollection collection)
        {
            try
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                List<Product> productList = (List<Product>)Session["ProductList"];
                int clientId = Convert.ToInt32(collection["CId"]);
                Client aClient=_clientManager.GetClientById(clientId);
                int productId = Convert.ToInt32(collection["ProductId"]);
                int qty = Convert.ToInt32(collection["Quantity"]);

                var aProduct = _productManager.GetProductByProductAndClientTypeId(productId,aClient.ClientTypeId); 
                aProduct.Quantity = qty;
              
                if (productList != null)
                {
                    productList.Add(aProduct);
                    Session["ProductList"] = productList;
                    ViewBag.ProductList = productList;
                }
                else
                {
                    productList = new List<Product> { aProduct };
                    Session["ProductList"] = productList;
                    ViewBag.ProductList = productList;
                }

                var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
                ViewBag.Total = productList.Sum(n => n.SubTotal);
                return View(products);
            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                return View();
            }
        }
        /// <summary>
        /// update or modify order item before save to databse...
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        
        public void Update(FormCollection collection)
        {
            try
            {
                List<Product> productList = (List<Product>)Session["ProductList"];
                int pid = Convert.ToInt32(collection["productIdToRemove"]);

                if (pid != 0)
                {
                    var aProduct = productList.Find(n => n.ProductId == pid);
                    productList.Remove(aProduct);
                    Session["ProductList"] = productList;
                    ViewBag.ProductList = productList;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("product"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("product_Id_", "");
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var aProduct = productList.Find(n => n.ProductId == productId);


                        if (aProduct != null)
                        {
                            productList.Remove(aProduct);
                            aProduct.Quantity = qty;
                            productList.Add(aProduct);
                            Session["ProductList"] = productList;
                            ViewBag.ProductList = productList;
                        }

                    }
                }

             

            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    string msg = e.InnerException.Message;
                }
            }
        }

        /// <summary>
        /// Save Order to database---
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Confirm(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var branchId = Convert.ToInt32(Session["BranchId"]);
            try
            {
                decimal vat = 0;
                var user = (User)Session["user"];
                int clientId = Convert.ToInt32(collection["ClientId"]);
                int orderByUserId = user.UserId;
                decimal amount = Convert.ToDecimal(collection["Total"]);
                DateTime orderDate = Convert.ToDateTime(collection["OrderDate"]);
                List<Product> productList = (List<Product>)Session["ProductList"];
                vat = productList.Sum(n => n.Vat * n.Quantity);
                var order = new Order
                {
                BranchId = branchId,
                ClientId = clientId,
                UserId = orderByUserId,
                OrderDate = orderDate,
                CompanyId = companyId,
                Discount = productList.Sum(n => n.Quantity * n.DiscountAmount),
                Products = productList,
                SpecialDiscount = Convert.ToDecimal(collection["SD"]),
                Vat = vat,
                
            };
                order.Amounts = amount + order.Discount;
                var result = _orderManager.Save(order);
                if (result > 0)
                {
                    Session["Orders"] = null;
                    aModel.Message = "<p class='text-green'>Order Submitted Successfully!!</p>";
                }

                else
                {
                    aModel.Message = "<p class='text-danger'>Failed to Submit!!</p>";

                }


            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    aModel.Message = "<p style='color:red'>" + e.InnerException.Message + "</p>";
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetProductList()
        {
            if(Session["ProductList"] != null)
            {
                IEnumerable<Product> products = ((List<Product>)Session["ProductList"]).ToList();
                return Json(products, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Product>(), JsonRequestBehavior.AllowGet);
        }

        //--Cancel Order---

        public ActionResult Cancel(int id)
        {
            Order order = _orderManager.GetOrderByOrderId(id); 
            return View(order);
        }

        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {

            //---------Status=5 means order cancel by sales person------------------

            var user = (User)Session["user"];

            int orderId = Convert.ToInt32(collection["OrderId"]);
            Order order = _orderManager.GetOrderByOrderId(orderId);
            order.ResonOfCancel = collection["Reason"];
            order.CancelByUserId = user.UserId;
            order.Status = 5;
            int status = _orderManager.CancelOrder(order);
            return RedirectToAction("All");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var order = _orderManager.GetOrderByOrderId(id);
            order.Client = _clientManager.GetClientById(order.ClientId);
            var orderdetails = _orderManager.GetOrderDetailsByOrderId(id).ToList();
            var products = _inventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.Products = products;
            Session["TOrders"] = orderdetails;
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                decimal amount = Convert.ToDecimal(collection["Amount"]);
                var dicount = Convert.ToDecimal(collection["Discount"]);
                var order = _orderManager.GetOrderByOrderId(id);
                List<OrderDetails> orders = (List<OrderDetails>)Session["TOrders"];
                order.Status = 0;
                order.SpecialDiscount = dicount;
                order.Discount = orders.Sum(n=>n.Quantity*n.DeletionStatus);
                order.OrderDate = DateTime.Now;
                decimal vat =orders.Sum(n=>n.Vat*n.Quantity);
                order.Vat = vat;
                order.Amounts = amount+order.Discount;
                bool result = _orderManager.UpdateOrder(order);
                string r = _orderManager.UpdateOrderDetails(orders);
                return RedirectToAction("PendingOrders");
            }
            catch (Exception exception)
            {
                string messge = exception.Message;
                return View();
            }
        }
        //--Edit/Update order after submition 
        public void UpdateOrder(FormCollection collection)
        {
            try
            {
                List<OrderDetails> orders = (List<OrderDetails>)Session["TOrders"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {
                    var anOrder = orders.Find(n => n.ProductId == pid);
                    orders.Remove(anOrder);
                    var rowAffected = _orderManager.DeleteProductFromOrderDetails(anOrder.OrderDetailsId);
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


        [HttpPost]
        public ActionResult AddNewItemToExistingOrder(FormCollection collection)
        {
            int orderId = Convert.ToInt32(collection["OrderId"]);
            List<OrderDetails> orders = (List<OrderDetails>)Session["TOrders"];
            try
            {
                var ord=_orderManager.GetOrderByOrderId(orderId);
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


        public JsonResult GetOrderDetails()
        {
            if (Session["TOrders"] != null)
            {
                IEnumerable<OrderDetails> orders = ((List<OrderDetails>)Session["TOrders"]).ToList();
                return Json(orders, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClients(string term)
        {

            List<string> clients = _clientManager.GetAll.ToList().Where(s => s.ClientName.StartsWith(term))
                  .Select(x => x.ClientName).ToList();
            return Json(clients, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Error()
        {
            ErrorModel errorModel = new ErrorModel
            {
                Heading = "Test",
                ErrorType = "Own",
                Description = "ABDDKSDjfa"
            };
            return View("_ErrorPartial", errorModel);
        }

        public ActionResult OrderList()
        {
         
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId,companyId).ToList().OrderByDescending(n => n.OrderId).ToList();
            return View(orders);
        }

        public ActionResult PendingOrders() 
        {
            //------------- Status=0 means the order is at initial stage----------
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0).ToList().OrderByDescending(n => n.OrderId).ToList();
            return View(orders);
        }

        public ActionResult OrderSlip(int id)
        {

            var order = _orderManager.GetOrderByOrderId(id);
            var orderDetails = _orderManager.GetOrderDetailsByOrderId(id);
            var client = _clientManager.GetClientDeailsById(order.ClientId);
            ViewBag.Client = client;
            ViewBag.Order = order;
            return View(orderDetails);

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