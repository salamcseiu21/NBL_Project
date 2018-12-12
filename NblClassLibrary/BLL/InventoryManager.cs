using System;
using System.Collections.Generic;
using System.Linq;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.BLL
{
    public class InventoryManager
    {

        readonly InventoryGateway _inventoryGateway=new InventoryGateway();
        readonly CommonGateway _commonGateway=new CommonGateway();
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {
            return _inventoryGateway.GetStockProductByBranchAndCompanyId(branchId, companyId);
        }

        public IEnumerable<TransactionModel> GetAllReceiveableProductByBranchAndCompanyId(int branchId,int companyId)
        {
            return _inventoryGateway.GetAllReceiveableProductByBranchAndCompanyId(branchId,companyId); 
        }

        public int ReceiveProduct(List<TransactionModel> receiveProductList,TransactionModel model)
        {
            
            return _inventoryGateway.ReceiveProduct(receiveProductList, model);
        }


        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            return _inventoryGateway.GetAllReceiveableProductToBranchByDeliveryRef(deliveryRef);
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            return _inventoryGateway.GetStockQtyByBranchAndProductId(branchId,productId);
        }

        public string Save(List<InvoiceDetails> invoicedOrders, Delivery aDelivery,int invoiceStatus,int orderStatus)
        {

            int maxRefNo = _inventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxRefNo);
            int rowAffected = _inventoryGateway.Save(invoicedOrders, aDelivery, invoiceStatus, orderStatus);
            return rowAffected > 0 ? "Saved Successfully!" : "Failed to Save";
        }

        private string GenerateDeliveryReference(int maxRefNo)
        {
            //---------Id=4 means delivery for sales 
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id.Equals(5)).Code;
            string temp = (maxRefNo + 1).ToString();
            string reference =DateTime.Now.Year.ToString().Substring(2,2)+refCode+ temp;
            return reference;
        }
    }
}