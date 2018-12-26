using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class InventoryManager:IInventoryManager
    {

        private readonly  IInventoryGateway _iInventoryGateway;

        public InventoryManager(IInventoryGateway iInventoryGateway)
        {
            _iInventoryGateway = iInventoryGateway;
        }
        readonly CommonGateway _commonGateway=new CommonGateway();
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iInventoryGateway.GetStockProductByBranchAndCompanyId(branchId, companyId);
        }
        public IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId)
        {
            return _iInventoryGateway.GetStockProductByCompanyId(companyId);
        }

        public IEnumerable<TransactionModel> GetAllReceiveableProductByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iInventoryGateway.GetAllReceiveableProductByBranchAndCompanyId(branchId,companyId); 
        }

        public int ReceiveProduct(List<TransactionModel> receiveProductList,TransactionModel model)
        {
            
            return _iInventoryGateway.ReceiveProduct(receiveProductList, model);
        }


        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            return _iInventoryGateway.GetAllReceiveableProductToBranchByDeliveryRef(deliveryRef);
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            return _iInventoryGateway.GetStockQtyByBranchAndProductId(branchId,productId);
        }

        public string Save(List<InvoiceDetails> invoicedOrders, Delivery aDelivery,int invoiceStatus,int orderStatus)
        {

            int maxRefNo = _iInventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _iInventoryGateway.Save(invoicedOrders, aDelivery, invoiceStatus, orderStatus);
            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        public string GenerateDeliveryReference(int maxRefNo)
        {
            //---------Id=4 means delivery for sales 
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(5)).Code;
            string temp = (maxRefNo + 1).ToString();
            string reference =DateTime.Now.Year.ToString().Substring(2,2)+refCode+ temp;
            return reference;
        }
    }
}