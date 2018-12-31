using NBL.Areas.Manager.DAL;
using System.Collections.Generic;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Manager.BLL
{
    public class DeliveryManager:IDeliveryManager
    {
        readonly DeliveryGateway _deliveryGateway=new DeliveryGateway();
        readonly IOrderManager _iOrderManager;
        readonly IClientManager _iClientManager;

        public DeliveryManager(IOrderManager iOrderManager,IClientManager iClientManager)
        {
            _iOrderManager = iOrderManager;
            _iClientManager = iClientManager;
        }
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
                var order = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
                delivery.Client = _iClientManager.GetById(order.ClientId);
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
            Order order = _iOrderManager.GetOrderInfoByTransactionRef(delivery.TransactionRef);
            var client = _iClientManager.GetClientDeailsById(order.ClientId);
            var chalan = new ViewChalanModel
            {
                DeliveryDetailses = details,
                ViewClient = client
            };
            return chalan;
        }
    }
}