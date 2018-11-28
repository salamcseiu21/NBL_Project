
using System;
using System.Collections.Generic;
using System.Linq;
using NBL.Models;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles = "Factory")]
    public class TransferController : Controller
    {
        private readonly BranchManager _branchManager = new BranchManager();
        private readonly ProductManager _productManager = new ProductManager();
        // GET: Factory/Transfer
        [HttpGet]
        public ActionResult Issue()
        {
            Session["factory_transfer_product_list"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Issue(FormCollection collection)
        {
            List<Product> productList = (List<Product>)Session["factory_transfer_product_list"]; 
            if (productList != null)
            {

                int fromBranchId = 9;
                int toBranchId = Convert.ToInt32(collection["ToBranchId"]);
                var user =(User)Session["user"];
                TransferIssue aTransferIssue = new TransferIssue();
                aTransferIssue.Products = productList;
                aTransferIssue.FromBranchId = fromBranchId;
                aTransferIssue.ToBranchId = toBranchId;
                aTransferIssue.IssueByUserId = user.UserId;
                aTransferIssue.TransferIssueDate = Convert.ToDateTime(collection["TransactionDate"]);
                int rowAffected = _productManager.IssueProductToTransfer(aTransferIssue);
                if (rowAffected > 0)
                {
                    Session["factory_transfer_product_list"] = null;
                    TempData["message"] = "Transfer Issue  Successful!";
                }
                else
                {
                    TempData["message"] = "Failed to Issue Transfer!";
                }

            }

            return View();
        }


        [HttpPost]
        public void TempTransferIssue(FormCollection collection) 
        {
            try
            {
                // TODO: Add Transcition logic here
                List<Product> productList = (List<Product>)Session["factory_transfer_product_list"]; 
                int productId = Convert.ToInt32(collection["ProductId"]);
                var product = _productManager.GetAll.ToList().Find(n => n.ProductId == productId);
                int quantiy = Convert.ToInt32(collection["Quantity"]);
                product.Quantity = quantiy;

                if(productList!=null)
                {
                    productList.Add(product);
                }
                else
                {
                    productList = new List<Product> { product };
                }
                
                Session["factory_transfer_product_list"] = productList;
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
                List<Product> productList = (List<Product>)Session["factory_transfer_product_list"];

                int productId = Convert.ToInt32(collection["productIdToRemove"]);
                if (productId != 0)
                {

                    var product = productList.Find(n => n.ProductId == productId);
                    productList.Remove(product);
                    Session["factory_transfer_product_list"] = productList;
                }
                else
                {
                    var collectionAllKeys = collection.AllKeys.ToList();
                    var productIdList = collectionAllKeys.FindAll(n => n.Contains("NewQuantity"));
                    foreach (string s in productIdList)
                    {
                        var value = s.Replace("NewQuantity_", "");
                        var user = (User)Session["user"];
                        int productIdToUpdate = Convert.ToInt32(collection["product_Id_" + value]);
                        int qty = Convert.ToInt32(collection["NewQuantity_" + value]);
                        var product = productList.Find(n => n.ProductId == productIdToUpdate); 

                        if (product != null)
                        {
                            productList.Remove(product);
                            product.Quantity = qty;
                            productList.Add(product);
                            Session["factory_transfer_product_list"] = productList;
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
        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix) 
        {
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _productManager.GetAll.ToList().DistinctBy(n => n.ProductId).ToList().FindAll(n=>n.CompanyId==companyId).ToList();
            var productList = (from c in products
                               where c.ProductName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.ProductName,
                                   val = c.ProductId
                               }).ToList();

            return Json(productList);
        }


        public JsonResult GetTempTransferIssueProductList()
        {
            if (Session["factory_transfer_product_list"] != null)
            {
                IEnumerable<Product> productList = ((List<Product>)Session["factory_transfer_product_list"]).ToList();
                return Json(productList, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Product>(), JsonRequestBehavior.AllowGet);
        }

    }
}