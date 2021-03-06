﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NBL.Areas.Admin.BLL;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Controllers
{
    [Authorize]
    public class CommonController : Controller
    {
        private readonly ICommonManager _iCommonManager;
        private readonly IClientManager _iClientManager;
        private readonly IInventoryManager _iInventoryManager;
        private readonly IProductManager _iProductManager;
        private readonly DistrictGateway _districtGateway = new DistrictGateway();
        private readonly IUpazillaGateway _iUpazillaGateway;
        private readonly PostOfficeGateway _postOfficeGateway = new PostOfficeGateway();
        private readonly IInvoiceManager _iInvoiceManager;
        private readonly IRegionManager _iRegionManager;
        private readonly ITerritoryManager _iTerritoryManager;
        private readonly IDiscountManager _iDiscountManager;
        private readonly IDepartmentManager _idepartmentManager;
        private readonly IOrderManager _iOrderManager;
        private readonly IBranchManager _iBranchManager;

        public CommonController(IBranchManager iBranchManager,IClientManager iClientManager,IOrderManager iOrderManager,IDepartmentManager iDepartmentManager,IInventoryManager iInventoryManager,ICommonManager iCommonManager,IDiscountManager iDiscountManager,IRegionManager iRegionManager,ITerritoryManager iTerritoryManager,IProductManager iProductManager,IInvoiceManager iInvoiceManager,IUpazillaGateway iUpazillaGateway)
        {
            _iBranchManager = iBranchManager;
            _iClientManager = iClientManager;
            _iOrderManager = iOrderManager;
            _idepartmentManager = iDepartmentManager;
            _iInventoryManager = iInventoryManager;
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
            _iRegionManager = iRegionManager;
            _iProductManager = iProductManager;
            _iTerritoryManager = iTerritoryManager;
            _iUpazillaGateway = iUpazillaGateway;
            _iInvoiceManager = iInvoiceManager;
        }
        //------------Bank Name autocomplete-----------
        [HttpPost]
        public JsonResult BankNameAutoComplete(string prefix)
        {


            var bankList = (from c in _iCommonManager.GetAllBank().ToList()
                            where c.BankName.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.BankName,
                                val = c.BankId
                            }).ToList();

            return Json(bankList);
        }


        public JsonResult BankAccountNameAutoComplete(string prefix)
        {


            var bankAccountList = (from c in _iCommonManager.GetAllBankBranch().ToList()
                            where c.BankBranchName.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.BankBranchName,
                                val = c.BankBranchAccountCode
                            }).ToList();

            return Json(bankAccountList);
        }

        //-------DBBL Mobile Banking Account autocomplet ----------
        public JsonResult DbblMobileBankingAccountAutoComplete(string prefix)
        {
            var accountList = (from c in _iCommonManager.GetAllMobileBankingAccount().ToList().FindAll(n => n.MobileBankingTypeId == 2).ToList()
                               where c.MobileBankingAccountNo.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.MobileBankingAccountNo,
                                val = c.SubSubSubAccountCode
                            }).ToList();

            return Json(accountList);
        }

        //-------bKsah Mobile Banking Account autocomplet ----------
        [HttpPost]
            public JsonResult BikashMobileBankingAccountAutoComplete(string prefix)
        {
            var accountList = (from c in _iCommonManager.GetAllMobileBankingAccount().ToList().FindAll(n=>n.MobileBankingTypeId==1).ToList()
                            where c.MobileBankingAccountNo.ToLower().Contains(prefix.ToLower())
                            select new
                            {
                                label = c.MobileBankingAccountNo,
                                val = c.SubSubSubAccountCode
                            }).ToList();

            return Json(accountList);
        }
        //------------Client Name autocomplete-----------
        [HttpPost]
        public JsonResult ClientNameAutoComplete(string prefix)
        {

            var branchId = Convert.ToInt32(Session["BranchId"]);
            var clientList = (from c in _iClientManager.GetClientByBranchId(branchId).ToList().FindAll(n => n.Active.Equals("Y")).ToList()
                              where c.ClientName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = c.ClientName,
                                  val = c.ClientId
                              }).ToList();

            return Json(clientList);
        }

        public JsonResult GetAllClientNameForAutoComplete(string prefix) 
        {

          
            var clientList = (from c in _iClientManager.GetAll().ToList().FindAll(n => n.Active.Equals("Y")).ToList()
                where c.ClientName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.ClientName,
                    val = c.ClientId
                }).ToList();

            return Json(clientList);
        }
        //----------------------Get Client By Id----------
        public JsonResult GetClientById(int clientId)
        {
            Session["Orders"] = null;
            Session["ProductList"]= null;
            ViewClient client = _iClientManager.GetClientDeailsById(clientId);
            return Json(client, JsonRequestBehavior.AllowGet);
        }
        //----------------------Get Stock Quantiy  By  product Id----------
        public JsonResult GetProductQuantityInStockById(int productId)

        {
            StockModel stock = new StockModel();
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var qty = _iInventoryManager.GetStockQtyByBranchAndProductId(branchId, productId);
            stock.StockQty = qty;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        //----------------------Get product  By  product Id----------
        public JsonResult GetProductById(int productId)
        {
            var product = _iProductManager.GetAll().ToList().Find(n => n.ProductId == productId);
            return Json(product, JsonRequestBehavior.AllowGet);
        }
        //----------------------Get product  By  product category Id----------

        public JsonResult GetProductByProductCategoryId(int productCategoryId)
        {
            var products = _iProductManager.GetAllProductsByProductCategoryId(productCategoryId).ToList()
                .FindAll(n => n.CategoryId == productCategoryId).OrderBy(n => n.ProductName);
            return Json(products, JsonRequestBehavior.AllowGet);
        }


        //----------------Product Auto Complete------------------
        [HttpPost]
        public JsonResult ProductAutoComplete(string prefix)
        {

            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var products = _iInventoryManager.GetStockProductByBranchAndCompanyId(branchId, companyId).DistinctBy(n => n.ProductId).ToList();
            var productList = (from c in products
                               where c.ProductName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.ProductName,
                                   val = c.ProductId
                               }).ToList();

            return Json(productList);
        }

        public JsonResult ProductNameAutoComplete(string prefix)
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

        //---------Branch Auto Complete------------
        [HttpPost]
        public JsonResult BranchAutoComplete(string prefix)
        {

            int corporateBarachIndex = _iBranchManager.GetAll().ToList().FindIndex(n => n.BranchName.Contains("Corporate"));
            int branchId = Convert.ToInt32(Session["BranchId"]);
            var branches = _iBranchManager.GetAll().ToList();

            branches.RemoveAt(corporateBarachIndex);
            int currentBranchIndex = branches.ToList().FindIndex(n => n.BranchId == branchId);
            branches.RemoveAt(currentBranchIndex);

            var branchList = (from c in branches.ToList()
                              where c.BranchName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = c.BranchName,
                                  val = c.BranchId
                              }).ToList();

            return Json(branchList);
        }


        //-----------------Get All Bank Branch By Bank id----------------
        public JsonResult GetAllBankBranchByBankId(int bankId)
        {
            IEnumerable<BankBranch> branchList = _iCommonManager.GetAllBankBranch().ToList().FindAll(n => n.BankId == bankId).ToList();
            return Json(branchList, JsonRequestBehavior.AllowGet);
        }
        //-----------------Get All Bank Branch By  id----------------
        public JsonResult GetBankBranchById(int bankBranchId)
        {
            var bankBranch = _iCommonManager.GetAllBankBranch().ToList().Find(n => n.BankBranchId == bankBranchId);
            return Json(bankBranch, JsonRequestBehavior.AllowGet);
        }


        //---Load all District  by Region Id
        public JsonResult GetDistrictByRegionId(int regionId)
        {
            int divisionId = _iRegionManager.GetAll().ToList().Find(n => n.RegionId == regionId).DivisionId;
            IEnumerable<District> districts = _districtGateway.GetAllDistrictByDivistionId(divisionId);
            return Json(districts, JsonRequestBehavior.AllowGet);
        }
        //--------------Load all un assigned district by region id----------
        public JsonResult GetUnAssignedDistrictListByRegionId(int regionId)
        {
            
            IEnumerable<District> districts = _districtGateway.GetUnAssignedDistrictListByRegionId(regionId);
            return Json(districts, JsonRequestBehavior.AllowGet);
        }
       
        
        //---Load all District  by Division Id
        public JsonResult GetDistrictByDivisionId(int divisionId)
        {
            IEnumerable<District> districts = _districtGateway.GetAllDistrictByDivistionId(divisionId);
            return Json(districts, JsonRequestBehavior.AllowGet);
        }


        //---Load all Upazilla by District Id
        public JsonResult GetUpazillaByDistrictId(int districtId)
        {
            IEnumerable<Upazilla> upazillas = _iUpazillaGateway.GetAllUpazillaByDistrictId(districtId);
            return Json(upazillas, JsonRequestBehavior.AllowGet);
        }
        //---Load all post office by Upazilla Id
        public JsonResult GetPostOfficeByUpazillaId(int upazillaId)
        {

            IEnumerable<PostOffice> postOffices = _postOfficeGateway.GetAllPostOfficeByUpazillaId(upazillaId);
            return Json(postOffices, JsonRequestBehavior.AllowGet);
        }
        //---Load all territory by District Id
        public JsonResult GetTerritoryByRegionId(int regionId)
        {
            var territories = _iTerritoryManager.GetAll().ToList().FindAll(n => n.RegionId == regionId).ToList();
            return Json(territories, JsonRequestBehavior.AllowGet);
        }



        //------------Load all upazilla by territory Id-------

        public JsonResult GetUnAssignedUpazillaByTerritoryId(int territoryId)   
        {

            var upazillaList = _iUpazillaGateway.GetUnAssignedUpazillaByTerritoryId(territoryId).ToList();
            return Json(upazillaList, JsonRequestBehavior.AllowGet);
        }
        //-----------Sub Sub Sub Account autocomplete-----------------
        [HttpPost]
        public JsonResult SubSubSubAccountNameAutoCompleteByContra(string prefix,string isContra,int transactionTypeId)
        {

            if (isContra.Equals("true") && transactionTypeId==1)
            {
                var accountList = (from c in _iCommonManager.GetAllSubSubSubAccounts().ToList().FindAll(n=>n.SubSubSubAccountCode.StartsWith("3307")).ToList()
                                   where c.SubSubSubAccountName.ToLower().Contains(prefix.ToLower())
                                   select new
                                   {
                                       label = c.SubSubSubAccountName,
                                       val = c.SubSubSubAccountId
                                   }).ToList();

                return Json(accountList);
            }
            else if(isContra.Equals("true") && transactionTypeId == 2)
            {
                var accountList = (from c in _iCommonManager.GetAllSubSubSubAccounts().ToList().FindAll(n => n.SubSubSubAccountCode.StartsWith("3308")).ToList()
                                   where c.SubSubSubAccountName.ToLower().Contains(prefix.ToLower())
                                   select new
                                   {
                                       label = c.SubSubSubAccountName,
                                       val = c.SubSubSubAccountId
                                   }).ToList();

                return Json(accountList);
            }
            else
            {
                var accountList = (from c in _iCommonManager.GetAllSubSubSubAccounts().ToList()
                                   where c.SubSubSubAccountName.ToLower().Contains(prefix.ToLower())
                                   select new
                                   {
                                       label = c.SubSubSubAccountName,
                                       val = c.SubSubSubAccountId
                                   }).ToList();

                return Json(accountList);
            }
        }

        [HttpPost]
        public JsonResult SubSubSubAccountNameAutoComplete(string prefix)
        {

            var accountList = (from c in _iCommonManager.GetAllSubSubSubAccounts().ToList()
                               where c.SubSubSubAccountName.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.SubSubSubAccountName,
                                   val = c.SubSubSubAccountId
                               }).ToList();

            return Json(accountList);
        }

        //---Load all Invoice Ref  by client Id
        public JsonResult GetInvoiceRefByClientId(int clientId)
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var invoiceList= _iInvoiceManager.GetInvoicedRefferencesByClientId(clientId).ToList();
            return Json(invoiceList, JsonRequestBehavior.AllowGet);
        }

        //-----------Invoice ref autocomplete-----------------
        [HttpPost]
        public JsonResult InvoiceRefAutoComplete(string prefix,int clientId)
        {
           

            var invoiceList = (from c in _iInvoiceManager.GetInvoicedRefferencesByClientId(clientId).ToList()
                               where c.InvoiceRef.ToLower().Contains(prefix.ToLower())
                               select new
                               {
                                   label = c.InvoiceRef,
                                   val = c.InvoiceId
                               }).ToList();

            return Json(invoiceList);
        }
        [HttpPost]

       public JsonResult GetSubSubSubAccountById(int subSubSubAccountId)
        {
            SubSubSubAccount account = _iCommonManager.GetAllSubSubSubAccounts().ToList().Find(n=>n.SubSubSubAccountId==subSubSubAccountId);
            return Json(account, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnAssignedRegionList()
        {

            IEnumerable<Region> regions = _iRegionManager.GetUnAssignedRegionList();
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClientTypeWishDiscountByProductId(int productId)
        {
            var discounts = _iDiscountManager.GetAll().ToList().FindAll(n => n.ProductId == productId);
            var aDiscountModel=new ViewDiscountModel
            {
                 
                Corporate = discounts?.Find(n => n.ClientTypeId == 2)?.DiscountPercent ?? 0,
                Individual = discounts?.Find(n => n.ClientTypeId == 1)?.DiscountPercent??0,
                Dealer = discounts?.Find(n => n.ClientTypeId == 3)?.DiscountPercent??0
            };
            return Json(aDiscountModel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAssignedDistrictNameAutoComplete(string prefix)
        {

         var regions=_iRegionManager.GetRegionListWithDistrictInfo();
            var regionList = (from c in regions.ToList()
                where c.District.DistrictName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.District.DistrictName,
                    val = c.RegionDetailsId
                }).ToList();

            return Json(regionList);
        }

        public JsonResult GetRegionDetailsById(int rdId)
        {
            var regionDetails = _iRegionManager.GetRegionListWithDistrictInfo().ToList().Find(n=>n.RegionDetailsId==rdId);
            return Json(regionDetails, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssignedUpazillaNameAutoComplete(string prefix)
        {
            var upazillaList = _iUpazillaGateway.GetAssignedUpazillaList();
            var list = (from c in upazillaList.ToList()
                where c.UpazillaName.ToLower().Contains(prefix.ToLower())
                select new
                {
                    label = c.UpazillaName,
                    val = c.TerritoryDetailsId
                }).ToList();

            return Json(list);
        }

        public JsonResult GetTerritoryDetailsById(int tdId)
        {
            var territoryDetails = _iUpazillaGateway.GetAssignedUpazillaList().ToList().Find(n => n.TerritoryDetailsId == tdId).Territory;
            return Json(territoryDetails, JsonRequestBehavior.AllowGet);
        }

        public FilePathResult GetFileFromDisk(int attachmentId)
        {
         
            DirectoryInfo dirInfo = new DirectoryInfo(Server.MapPath("~/ClientDocuments"));
            var model = _iClientManager.GetClientAttachments().ToList().Find(n => n.Id == attachmentId);
            var fileName = model.FilePath.Replace("ClientDocuments/", "");

            model.FilePath = dirInfo.FullName + @"\" + fileName;
            // string CurrentFileName = 

            string contentType = string.Empty;

            if (fileName.Contains(".pdf"))
            {
                contentType = "application/pdf";
            }
            else if (fileName.Contains(".jpg") || fileName.Contains(".jpeg") || fileName.Contains(".png"))
            {

                contentType = "image/jpeg";
            }
            else if (fileName.Contains(".docx"))
            {
                contentType = "application/docx";
            }
            else if (fileName.Contains(".xlsx"))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            else if (fileName.Contains(".xls"))
            {
                contentType = "application/vnd.ms-excel";
            }
            return File(model.FilePath, contentType, model.AttachmentName + model.FileExtension);


        }

        /// <summary>
        /// Filter order..
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="clientName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public ActionResult GetOrdersByBranchId(SearchCriteria searchCriteria)
        {

            
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            IEnumerable<ViewOrder> orders= _iOrderManager.GetOrdersByCompanyId(companyId);
            if(searchCriteria.BranchId != null)
            {
                orders = orders.ToList().FindAll(n => n.BranchId.Equals(searchCriteria.BranchId));
            }
            else
            {
                orders = _iOrderManager.GetOrdersByCompanyId(companyId);
            }
            foreach (ViewOrder order in orders)
            {
                order.Client = _iClientManager.GetById(order.ClientId);
            }
            if (!string.IsNullOrEmpty(searchCriteria.ClientName))
            {
               orders=orders.ToList().FindAll(n => n.Client.ClientName.ToLower().Contains(searchCriteria.ClientName));
            }
            if (searchCriteria.StartDate!=null && searchCriteria.EndDate != null)
            {
                orders = orders.ToList().Where(n => n.OrderDate>= searchCriteria.StartDate && n.OrderDate <= searchCriteria.EndDate).ToList();
            }
          
            //return PartialView("_OrdersPartialPage", orders);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ViewModalPartial(int orderId)
        {
           var model= _iOrderManager.GetOrderByOrderId(orderId);
            model.Client = _iClientManager.GetById(model.ClientId);
            return PartialView("_ViewOrderDetailsModalPartialPage",model);
        }

        public PartialViewResult ViewOrderDetails(int orderId) 
        {

           
            var model = _iOrderManager.GetOrderByOrderId(orderId);
            model.Client = _iClientManager.GetById(model.ClientId);
            return PartialView("_ViewOrderDetailsModalPartialPage", model);
        }
        public ActionResult GetAllOrders()
        {
            return Json(_iOrderManager.GetAll().ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllOrdersByBranchAndCompanyId()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders= _iOrderManager.GetOrdersByBranchAndCompnayId(branchId, companyId);
            return Json(orders, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllDesignationByDepartmentId(int departmentId)
        {
            List<Designation> designations = _idepartmentManager.GetAllDesignationByDepartmentId(departmentId);
            return Json(designations, JsonRequestBehavior.AllowGet);
        }
    }


}