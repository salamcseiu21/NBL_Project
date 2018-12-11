using NBL.Areas.Manager.DAL;
using NBL.Areas.Manager.Models;
using System.Collections.Generic;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Manager.BLL
{
    public class DeliveryManager
    {
        readonly DeliveryGateway _deliveryGateway=new DeliveryGateway();
        readonly OrderManager _orderManager=new OrderManager();
        readonly ClientManager _clientManager=new ClientManager();
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
            var deliveredOrders =
                _deliveryGateway.GetAllDeliveredOrdersByBranchCompanyAndUserId(branchId, companyId, deliveredByUserId);
            foreach (Delivery delivery in deliveredOrders)
            {
                var order = _orderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                delivery.Client = _clientManager.GetClientById(order.ClientId);
            }

            return deliveredOrders;
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

        public ViewChalanModel GetChalanByDeliveryId(int deliveryId) 
        {
            Delivery delivery =GetOrderByDeliveryId(deliveryId);
            var details = GetDeliveredOrderDetailsByDeliveryId(deliveryId);
            Order order = _orderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
            var client = _clientManager.GetClientDeailsById(order.ClientId);
            var chalan = new ViewChalanModel
            {
                DeliveryDetailses = details,
                ViewClient = client
            };
            return chalan;
        }
    }
}