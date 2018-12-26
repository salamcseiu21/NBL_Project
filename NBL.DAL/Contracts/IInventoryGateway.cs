using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface IInventoryGateway
   {
       IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId);
       int GetMaxDeliveryRefNoOfCurrentYear();
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       IEnumerable<TransactionModel> GetAllReceiveableProductByBranchAndCompanyId(int branchId, int companyId);
       int ReceiveProduct(List<TransactionModel> receiveProductList, TransactionModel model);
       int SaveReceiveProductDetails(List<TransactionModel> receiveProductList, int inventoryId);
       int GetStockQtyByBranchAndProductId(int branchId, int productId);
       int Save(List<InvoiceDetails> invoicedOrders, Delivery aDelivery, int invoiceStatus, int orderStatus);
       int SaveDeliveredOrderDetails(List<InvoiceDetails> invoicedOrders, int inventoryId, int deliveryId);
   }
}
