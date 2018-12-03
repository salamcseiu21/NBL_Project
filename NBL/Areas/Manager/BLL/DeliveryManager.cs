using NBL.Areas.Manager.DAL;
using NBL.Areas.Manager.Models;
using System.Collections.Generic;
using NblClassLibrary.Models;

namespace NBL.Areas.Manager.BLL
{
    public class DeliveryManager
    {
        readonly DeliveryGateway _deliveryGateway=new DeliveryGateway();
        public int ChangeOrderStatusByManager(Order aModel) 
        {
            int rowAffected = _deliveryGateway.ChangeOrderStatusByManager(aModel);  
            return rowAffected;
        }

        public IEnumerable<Delivery> GetAllDeliveredOrders() 
        {
            return _deliveryGateway.GetAllDeliveredOrders();
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            return _deliveryGateway.GetAllDeliveredOrdersByBranchAndCompanyId(branchId,companyId);
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,int deliveredByUserId)
        {
            return _deliveryGateway.GetAllDeliveredOrdersByBranchCompanyAndUserId(branchId, companyId,deliveredByUserId);
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef)
        {
            return _deliveryGateway.GetAllDeliveredOrdersByInvoiceRef(invoiceRef);
        }
        public Delivery GetOrderByDeliveryId(int deliveryId) 
        {
            return _deliveryGateway.GetOrderByDeliveryId(deliveryId);
        }

        public IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(int deliveryId) 
        {
            return _deliveryGateway.GetDeliveredOrderDetailsByDeliveryId(deliveryId);
        }

        public IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId)
        {
            return _deliveryGateway.GetAllInvoiceOrderListByBranchId(branchId);
        }
    }
}