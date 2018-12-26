using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IInventoryManager
   {
       IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId);
       IEnumerable<TransactionModel> GetAllReceiveableProductByBranchAndCompanyId(int branchId, int companyId);
       int ReceiveProduct(List<TransactionModel> receiveProductList, TransactionModel model);
       IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef);
       int GetStockQtyByBranchAndProductId(int branchId, int productId);
       string Save(List<InvoiceDetails> invoicedOrders, Delivery aDelivery, int invoiceStatus, int orderStatus);
       string GenerateDeliveryReference(int maxRefNo);
   }
}
