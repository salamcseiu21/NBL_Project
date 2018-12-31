using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;

namespace NBL.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManager _iProductManager;
        private readonly IBranchManager _iBranchManager;

        public ProductController(IBranchManager iBranchManager,IProductManager iProductManager)
        {
            _iBranchManager = iBranchManager;
            _iProductManager = iProductManager;
        }

        // GET: Admin/Product
        public ActionResult Transfer()  
        {

            Session["transactions"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(FormCollection collection)
        {
            List<TransactionModel> transactions = (List<TransactionModel>)Session["transactions"];
            if (transactions != null)
            {
                var model = transactions.ToList().First();
                int rowAffected = _iProductManager.TransferProduct(transactions, model);
                if (rowAffected > 0)
                {
                    Session["transactions"] = null;
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
                var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
                int fromBranchId = 9;
                int toBranchId = Convert.ToInt32(collection["BranchId"]);
                int quantiy = Convert.ToInt32(collection["Quantity"]);
                int userId = 4;
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


        public JsonResult GetProductById(int productId)

        {
            var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
            return Json(product, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductByProductCategoryId(int productCategoryId)
        {
            var products = _iProductManager.GetAll().ToList();
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {

            var products = _iProductManager.GetAll().ToList();

            var productList = (from c in products
                               where c.ProductName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.ProductName,
                                   val = c.ProductId
                               }).ToList();

            return Json(productList);
        }

        [HttpPost]
        public JsonResult BranchAutoComplete(string prefix)
        {

            //BranchManager branchManager = new BranchManager();
            //int corporateBarachIndex = 9;
            //int branchId = 9;


            var branches = _iBranchManager.GetAll().ToList();

           //branches.RemoveAt(corporateBarachIndex);
           //int currentBranchIndex = branches.ToList().FindIndex(n => n.BranchId == branchId);
           // branches.RemoveAt(currentBranchIndex);

            var branchList = (from c in branches.ToList()
                              where c.BranchName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = c.BranchName,
                                  val = c.BranchId
                              }).ToList();

            return Json(branchList);
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