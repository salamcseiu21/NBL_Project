﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Factory.Controllers
{
    [Authorize(Roles = "Factory")]
    public class ProductController : Controller
    {
        // GET: Factory/Product
        private readonly IProductManager _iProductManager;

        public ProductController(IProductManager iProductManager)
        {
            _iProductManager = iProductManager;
        }

        [HttpGet]
        public ActionResult Transaction()
        {
            Session["factory_transactions"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Transaction(FormCollection collection)
        {
            List<TransactionModel> transactions = (List<TransactionModel>)Session["factory_transactions"];
            if (transactions != null)
            {
                
                Random rnd = new Random();
                var model = transactions.ToList().First();

                model.TransactionId = rnd.Next(1, 90000000);
                model.Transportation = collection["Transportation"];
                model.DriverName = collection["DriverName"];
                model.TransportationCost = Convert.ToDecimal(collection["Cost"]);
                model.VehicleNo = collection["VehicleNo"];
                int rowAffected = _iProductManager.TransferProduct(transactions, model);
                if (rowAffected > 0)
                {
                    Session["factory_transactions"] = null;
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
                    TransactionDate = date,
                    UnitPrice = product.UnitPrice,
                    DealerPrice = product.DealerPrice,
                   

                };

                List<TransactionModel> transactions = (List<TransactionModel>)Session["factory_transactions"];

                if (transactions != null)
                {
                    var order = transactions.Find(n => n.ProductId == aModel.ProductId);
                    if (order != null)
                    {
                        transactions.Remove(order);
                        transactions.Add(aModel);
                        Session["factory_transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }
                    else
                    {
                        transactions.Add(aModel);
                        Session["factory_transactions"] = transactions;
                        ViewBag.Transactions = transactions;
                    }

                }
                else
                {
                    transactions = new List<TransactionModel> { aModel };
                    Session["factory_transactions"] = transactions;
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
                List<TransactionModel> transactions = (List<TransactionModel>)Session["factory_transactions"];

                int pid = Convert.ToInt32(collection["productIdToRemove"]);
                if (pid != 0)
                {

                    var transaction = transactions.Find(n => n.ProductId == pid);
                    transactions.Remove(transaction);
                    Session["factory_transactions"] = transactions;
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
                            Session["factory_transactions"] = transactions;
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
        [HttpPost]
        public JsonResult ProductNameAutoComplete(string prefix)
        {

            var products = _iProductManager.GetAll().ToList().DistinctBy(n => n.ProductId).ToList();
            var productList = (from c in products
                where c.ProductName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.ProductName,
                    val = c.ProductId
                }).ToList();

            return Json(productList);
        }


        public JsonResult GetTempTransactionProductList()
        {
            if (Session["factory_transactions"] != null)
            {
                IEnumerable<TransactionModel> transactions = ((List<TransactionModel>)Session["factory_transactions"]).ToList(); 
                return Json(transactions, JsonRequestBehavior.AllowGet);
            }
            return Json(new List<Order>(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProductionNote()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ProductionNote(ViewCreateProductionNoteModel model)
        {
            if(ModelState.IsValid)
            {
                var productionNote = Mapper.Map<ProductionNote>(model);
                var user = (ViewUser) Session["user"];
                productionNote.ProductionNoteByUserId = user.UserId;
                bool result = _iProductManager.SaveProductionNote(productionNote);
                if (result)
                {
                    ModelState.Clear();
                    ViewBag.Message = "<p class='text-success'>Production Note save successfully!</p>";
                }
                   
                return View();
            }
            ViewBag.Message = "<p class='text-danger'>Production Note failed to save</p>";
            return View();
        }

        public ActionResult ViewPendingProductionNote()
        {
            var productionNotes = _iProductManager.PendingProductionNote();
            return View(productionNotes);
        }

        public ActionResult GetAllProductionNotes()
        {
            return Json(_iProductManager.PendingProductionNote(), JsonRequestBehavior.AllowGet);
        }
    }
}