using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;
namespace NBL.Areas.Sales.Controllers
{
    [Authorize(Roles ="User")]
    public class OrderController : Controller
    {
        private readonly ProductManager _productManager = new ProductManager();
        private  readonly IOrderManager _iOrderManager;
        private readonly IClientManager _iClientManager;
        private readonly IInventoryManager _iInventoryManager;

        public OrderController(IClientManager iClientManager,IOrderManager iOrderManager,IInventoryManager iInventoryManager)
        {
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _iInventoryManager = iInventoryManager;
        }
        public PartialViewResult All()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId,companyId).OrderByDescending(n => n.OrderId).DistinctBy(n => n.OrderId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public PartialViewResult LatestOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetLatestOrdersByBranchAndCompanyId(branchId, companyId).OrderByDescending(n => n.OrderId).ToList();
            ViewBag.Heading = "Latest Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        
        public ActionResult Order()
        {
            Session["ProductList"] = null;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
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
                Client aClient=_iClientManager.GetClientById(clientId);
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

                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
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
                    foreach (var s in productIdList)
                    {
                        var value = s.Replace("product_Id_", "");
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var aProduct = productList.Find(n => n.ProductId == productId);


                        if(aProduct != null)
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
                var user = (ViewUser)Session["user"];
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
                var result = _iOrderManager.Save(order);
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
            var order = _iOrderManager.GetOrderByOrderId(id); 
            return View(order);
        }

        [HttpPost]
        public ActionResult Cancel(FormCollection collection)
        {

            //---------Status=5 means order cancel by sales person------------------

            var user = (ViewUser)Session["user"];
            int orderId = Convert.ToInt32(collection["OrderId"]);
            var order = _iOrderManager.GetOrderByOrderId(orderId);
            order.ResonOfCancel = collection["Reason"];
            order.CancelByUserId = user.UserId;
            order.Status = 5;
            bool status = _iOrderManager.CancelOrder(order);
            return status ? RedirectToAction("All") : RedirectToAction("Cancel",orderId);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var order = _iOrderManager.GetOrderByOrderId(id);
            //var orderdetails = _orderManager.GetOrderDetailsByOrderId(id).ToList();
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            ViewBag.Products = products;
            Session["TOrders"] = order.OrderItems;
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                decimal amount = Convert.ToDecimal(collection["Amount"]);
                var dicount = Convert.ToDecimal(collection["Discount"]);
                var order = _iOrderManager.GetOrderByOrderId(id);
                order.Status = 0;
                var orderItems = (IEnumerable<OrderItem>) Session["TOrders"];
                order.SpecialDiscount = dicount;
                order.Discount = orderItems.ToList().Sum(n=>n.Quantity*n.DeletionStatus);
                order.OrderDate = DateTime.Now;
                decimal vat = orderItems.Sum(n=>n.Vat*n.Quantity);
                order.Vat = vat;
                order.Amounts = amount+order.Discount;
                bool result = _iOrderManager.UpdateOrder(order);
                if (result)
                {
                    string r = _iOrderManager.UpdateOrderDetails(orderItems.ToList());
                }
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
                var orderItems = (List<OrderItem>)Session["TOrders"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {
                    var anOrder = orderItems.Find(n => n.ProductId == pid);
                    orderItems.Remove(anOrder);
                    var result = _iOrderManager.DeleteProductFromOrderDetails(anOrder.OrderItemId); 
                    Session["TOrders"] = orderItems;
                    ViewBag.Orders = orderItems;
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
                        var anItem = orderItems.Find(n => n.ProductId == productId); 
                        if (anItem != null)
                        {
                            orderItems.Remove(anItem);
                            anItem.Quantity = qty;
                            orderItems.Add(anItem);
                            Session["TOrders"] = orderItems;
                            ViewBag.Orders = orderItems;
                        }

                    }
                }


                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).DistinctBy(n => n.ProductId).ToList();
                ViewBag.Products = products;

            }
            catch (Exception e)
            {
                int companyId = Convert.ToInt32(Session["CompanyId"]);
                int branchId = Convert.ToInt32(Session["BranchId"]);
                var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).DistinctBy(n => n.ProductId).ToList();
                ViewBag.Products = products;
                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }


        [HttpPost]
        public ActionResult AddNewItemToExistingOrder(FormCollection collection)
        {
            int orderId = Convert.ToInt32(collection["OrderId"]);
            var items = (List<OrderItem>)Session["TOrders"];
            try
            {
                var ord= _iOrderManager.GetOrderByOrderId(orderId);
                Client aClient = _iClientManager.GetClientById(ord.ClientId);
                int productId = Convert.ToInt32(collection["ProductId"]);
                var aProduct = _productManager.GetProductByProductAndClientTypeId(productId, aClient.ClientTypeId);
                aProduct.Quantity = Convert.ToInt32(collection["Quantity"]);

                var item = items.Find(n => n.ProductId == productId);
                if (item != null)
                {
                    ViewBag.Result = "This product already is in the list!";
                }
                else
                {
                    bool rowAffected = _iOrderManager.AddNewItemToExistingOrder(aProduct,orderId);
                    if (rowAffected)
                    {
                        ViewBag.Result = "1 new Item added successfully!";
                    }
                    
                }

                return RedirectToAction("Edit",orderId);

            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;
                return RedirectToAction("Edit",orderId);
            }
        }


        public JsonResult GetOrderDetails()
        {
            if (Session["TOrders"] != null)
            {
                var orders = ((List<OrderItem>)Session["TOrders"]).ToList();
                return Json(orders, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetClients(string term)
        {

            List<string> clients = _iClientManager.GetAll().ToList().Where(s => s.ClientName.StartsWith(term))
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
            var orders = _iOrderManager.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId,companyId).ToList().OrderByDescending(n => n.OrderId).ToList();
            return View(orders);
        }

        public ActionResult PendingOrders() 
        {
            //------------- Status=0 means the order is at initial stage----------
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetOrdersByBranchIdCompanyIdAndStatus(branchId, companyId, 0).ToList().OrderByDescending(n => n.OrderId).ToList();
            return View(orders);
        }

        public ActionResult DelayedOrders() 
        {
         
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _iOrderManager.GetDelayedOrdersToSalesPersonByBranchAndCompanyId(branchId,companyId);
            return View(orders);
        }

        public ActionResult OrderSlip(int id)
        {
            var orderSlip = _iOrderManager.GetOrderSlipByOrderId(id);
            var user = (ViewUser) Session["user"];
            orderSlip.ViewUser = user;
            return View(orderSlip);

        }

        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix)
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
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