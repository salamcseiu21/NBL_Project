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
    public class ProductController : Controller
    {
        readonly ProductManager _productManager = new ProductManager();
        readonly IInventoryManager _iInventoryManager;

        public ProductController(IInventoryManager iInventoryManager)
        {
            _iInventoryManager = iInventoryManager;
        }
     
        public PartialViewResult Stock()
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).ToList();
            return PartialView("_ViewStockProductInBranchPartialPage", products);
        }

        [HttpGet]
        public ActionResult Transaction()
        {
            Session["transactions"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Transaction(FormCollection collection)
        {
            List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];
            if (transactions != null)
            {

                var model = transactions.ToList().First();
                Random random = new Random();
                model.TransactionId = random.Next(1, 1000000);
                ProductManager productManager = new ProductManager();
                int rowAffected = productManager.TransferProduct(transactions, model);
                if (rowAffected > 0)
                {
                    TempData["message"] = "Transferred Successfully !";
                }
                else
                {
                    TempData["message"] = "Failed to Transfer Product !";
                }

            }

            return View();
        }
        [HttpPost]
        public void TempTransaction(FormCollection collection)
        {
            try
            {
                // TODO: Add Transcition logic here

                int productId = Convert.ToInt32(collection["ProductId"]);
                var product = _productManager.GetAll.ToList().Find(n => n.ProductId == productId);
                int fromBranchId = Convert.ToInt32(Session["BranchId"]);
                int toBranchId = Convert.ToInt32(collection["BranchId"]);
                int quantiy = Convert.ToInt32(collection["Quantity"]);
                int userId = ((ViewUser)Session["user"]).UserId;
                DateTime date = Convert.ToDateTime(collection["TransactionDate"]);
                TransactionModel aModel = new TransactionModel
                {
                    ProductId = productId,
                    FromBranchId = fromBranchId,
                    ToBranchId = toBranchId,
                    Quantity = quantiy,
                    UserId = userId,
                    ProductName = product.ProductName,
                    TransactionDate = date

                };

                List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];

                if (transactions != null)
                {
                    var order = transactions.Find(n => n.ProductId == aModel.ProductId);
                    if (order != null)
                    {
                        transactions.Remove(order);
                        transactions.Add(aModel);
                        Session["transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }
                    else
                    {
                        transactions.Add(aModel);
                        Session["transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }

                }
                else
                {
                    transactions = new List<TransactionModel> { aModel };
                    Session["transactions"] = transactions;
                    ViewBag.Transactions = transactions;
                }

                //return View();
            }
            catch
            {
                //return View();
            }
        }
        [HttpPost]
        public void Update(FormCollection collection)
        {
            try
            {
                List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {

                    var transaction = transactions.Find(n => n.ProductId == pid);
                    transactions.Remove(transaction);
                    Session["transactions"] = transactions;
                    ViewBag.Orders = transactions;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                        var user = (User)Session["user"];
                        int productId = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var transaction = transactions.Find(n => n.ProductId == productId);


                        if (transaction != null)
                        {
                            transactions.Remove(transaction);
                            transaction.Quantity = qty;
                            transaction.UserId = user.UserId;
                            transactions.Add(transaction);
                            Session["transactions"] = transactions;
                            ViewBag.Orders = transaction;
                        }

                    }
                }


            }
            catch (Exception e)
            {

                if (e.InnerException != null)
                    ViewBag.Error = e.Message + " <br /> System Error:" + e.InnerException.Message;

            }
        }

        public ActionResult Receive()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var result = _iInventoryManager.GetAllReceiveableProductByBranchAndCompanyId(branchId,companyId).ToList(); 
            ViewBag.ProductList = result;
            return View(result);
        }

        [HttpPost]
        public ActionResult Receive(FormCollection collection)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            string deliveryRef = collection["DeliveryRef"];
            List<TransactionModel> receivesProductList = _iInventoryManager.GetAllReceiveableProductToBranchByDeliveryRef(deliveryRef).ToList();
            var collectionAllKeys = collection.AllKeys.ToList().FindAll(n => n.Contains("product_qty_"));
            foreach (var item in collectionAllKeys)
            {
                var value = item.Replace("product_qty_", "");
                int productId = Convert.ToInt32(value);
                ProductDetails aProduct = _productManager.GetProductDetailsByProductId(productId);
                int qty = Convert.ToInt32(collection["product_qty_" + value]);
                var transaction = receivesProductList.Find(n => n.ProductId == productId);
                transaction.Quantity = qty;
                transaction.CostPrice = aProduct.UnitPrice;
                transaction.StockQuantity = _iInventoryManager.GetStockQtyByBranchAndProductId(branchId, productId) + qty;
                receivesProductList.Remove(transaction);
                receivesProductList.Add(transaction);

            }
            var model = receivesProductList.ToList().First();
            model.TransactionRef = deliveryRef;
            int rowAffected = _iInventoryManager.ReceiveProduct(receivesProductList.ToList(), model);
            var result = _iInventoryManager.GetAllReceiveableProductByBranchAndCompanyId(branchId,companyId).ToList();
            ViewBag.ProductList = result;
            return View(result);

        }

        public JsonResult GetTempTransaction()
        {
            if (Session["transactions"] != null)
            {
                IEnumerable<TransactionModel> transactions = ((List<TransactionModel>)Session["transactions"]).ToList();
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }

    }
}