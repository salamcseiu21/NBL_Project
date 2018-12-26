using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IDeliveryManager
   {
       int ChangeOrderStatusByManager(Order aModel);
       

        IEnumerable<Delivery> GetAllDeliveredOrders();
        

        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId, int companyId);
        

        IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,
            int deliveredByUserId);
        

        IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef);
       

        Delivery GetOrderByDeliveryId(int deliveryId);
       

        IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(int deliveryId);
        

        IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId);
        

        ViewChalanModel GetChalanByDeliveryId(int deliveryId);

    }
}
