using System;
using System.Collections.Generic;
using System.Linq;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;
namespace NblClassLibrary.BLL
{
    public class OrderManager
    {

        readonly OrderGateway _orderGateway=new OrderGateway();
        public IEnumerable<Order> GetAll => _orderGateway.GetAll;
        public IEnumerable<Order> GetOrdersByBranchId(int branchId)
        {
            return _orderGateway.GetOrdersByBranchId(branchId);
        }
        public IEnumerable<ViewOrder> GetOrdersByCompanyId(int companyId)
        {
            var orders = _orderGateway.GetOrdersByCompanyId(companyId);
            foreach (var order in orders)
            {
                order.OrderItems = _orderGateway.GetOrderItemsByOrderId(order.OrderId);
            }
            return orders;
        }

        public IEnumerable<ViewInvoicedOrder> GetOrderListByClientId(int clientId)
        {
           return  _orderGateway.GetOrderListByClientId(clientId);
        }
        public IEnumerable<ViewOrder> GetOrdersByBranchAndCompnayId(int branchId, int companyId)
        {
            return _orderGateway.GetOrdersByBranchAndCompnayId(branchId,companyId);
        }
        public IEnumerable<ViewOrder> GetAllOrderWithClientInformationByCompanyId(int companyId)
        { 
            return _orderGateway.GetAllOrderWithClientInformationByCompanyId(companyId);
        }

        public IEnumerable<ViewOrder> GetAllOrderByBranchAndCompanyIdWithClientInformation(int branchId, int companyId)
        {
            return _orderGateway.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId, companyId);
        }

        public IEnumerable<ViewOrder> GetOrdersByBranchCompanyAndNsmUserId(int branchId, int companyId, int nsmUserId)
        {
            return _orderGateway.GetOrdersByBranchCompanyAndNsmUserId(branchId, companyId, nsmUserId);
        }
        public IEnumerable<ViewOrder> GetOrdersByNsmUserId(int nsmUserId)
        {
            return _orderGateway.GetOrdersByNsmUserId(nsmUserId);
        }
        public IEnumerable<ViewOrder> GetOrdersByBranchIdCompanyIdAndStatus(int branchId, int companyId,int status)
        {
            return _orderGateway.GetOrdersByBranchIdCompanyIdAndStatus(branchId,companyId,status);
        }
        public IEnumerable<ViewOrder> GetLatestOrdersByBranchAndCompanyId(int branchId, int companyId)
        {
            return _orderGateway.GetLatestOrdersByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<OrderDetails> GetOrderDetailsByOrderId(int orderId)
        {
            var orderModels =_orderGateway.GetOrderDetailsByOrderId(orderId);
            return orderModels;
        }

        public int Save(Order order)
        {
            int maxSl = _orderGateway.GetOrderMaxSerialNoByYear(DateTime.Now.Year);
            order.OrderSlipNo = GenerateOrderSlipNo(maxSl);
            order.OrederRef = GenerateOrderRefNo(maxSl);
            int rowAffected= _orderGateway.Save(order);
            return rowAffected;
        }

        private string GenerateOrderRefNo(int maxsl)
        {
            int sN =maxsl+1;
            string reference = DateTime.Now.Date.Year.ToString().Substring(2, 2) + "OR" + sN;
            return reference;
        }

        private string GenerateOrderSlipNo(int maxSl)
        {
            int sN=1+maxSl;
            string ordSlipNo = DateTime.Now.Date.Year.ToString().Substring(2, 2)+"OS"+sN;
            return ordSlipNo;
        }

        public string ApproveOrderByNsm(ViewOrder order)
        {
            int rowAffected = _orderGateway.ApproveOrderByNsm(order);
            if (rowAffected > 0)
            {
                return "Approved by NSM Successfully!";
            }

            return "Failed to approve by NSM";
        }

        public string ApproveOrderByAdmin(ViewOrder order)
        {
            int rowAffected = _orderGateway.ApproveOrderByAdmin(order); 
            if (rowAffected > 0)
            {
                return "Approved by Admin Successfully!";
            }

            return "Failed to approve by Admin";
        }

        public ViewOrder GetOrderByOrderId(int orderId)
        {
            ClientManager clientManager=new ClientManager();
            var order = _orderGateway.GetOrderByOrderId(orderId);
            order.Client = clientManager.GetClientById(order.ClientId);
            order.OrderItems = _orderGateway.GetOrderItemsByOrderId(orderId);
            return order;
        }
      
        public int CancelOrder(ViewOrder order)
        {
            return _orderGateway.CancelOrder(order);
        }

        public IEnumerable<ViewOrder> GetLatestOrdersByCompanyId(int companyId)
        {
            var orders= _orderGateway.GetLatestOrdersByCompanyId(companyId);
            foreach (ViewOrder order in orders)
            {
                order.OrderItems=_orderGateway.GetOrderItemsByOrderId(order.OrderId);
            }

            return orders.ToList();

        }

        public string UpdateOrderDetails(IEnumerable<OrderItem> orderItems)
        {
            int rowAffected = _orderGateway.UpdateOrderDetails(orderItems);
            return rowAffected > 0 ? "Updated Successfully!" : "Failed to Update";
        }

        public bool DeleteProductFromOrderDetails(int orderItemId) 
        {
            int rowAffected = _orderGateway.DeleteProductFromOrderDetails(orderItemId);
            return rowAffected > 0;
        }

        public string GetDiscountAccountCodeByClintTypeId(int typeId)
        {
            string discountCode= "26010";

            // 1=Individual or retail Customer
            // 2= Corporate coustomer 
            // 3=Dealer
            if(typeId==1)
            {
                discountCode += "21";
            }
            if (typeId == 3)
            {
                discountCode += "11";
            }
            if (typeId == 2)
            {
                discountCode += "31";
            }
            return discountCode;
        }

     

        public IEnumerable<OrderDetails> GetAllOrderDetails()
        {
            return _orderGateway.GetAllOrderDetails;
        }

        public Order GetOrderInfoByTransactionRef(string transactionRef)
        {
            return _orderGateway.GetOrderInfoByTransactionRef(transactionRef);
        }

        public int AddNewItemToExistingOrder(Product aProduct,int orderId)
        {
            return _orderGateway.AddNewItemToExistingOrder(aProduct,orderId);
        }

        public bool UpdateOrder(ViewOrder order)
        {
            int rowAffected = _orderGateway.UpdateOrder(order);
            return rowAffected > 0;
        }

        public IEnumerable<ChartModel> GetTotalOrdersOfCurrentYearByCompanyId(int companyId)
        {
            return _orderGateway.GetTotalOrdersOfCurrentYearByCompanyId(companyId);
        }
        public IEnumerable<ChartModel> GetTotalOrdersByBranchIdCompanyIdAndYear(int branchId, int companyId,int year) 
        {
            return _orderGateway.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId,companyId,year);
        }

        public IEnumerable<Order> GetOrdersByClientId(int clientId)
        {
           return _orderGateway.GetOrdersByClientId(clientId);
        }

        public List<Product> GetProductListByOrderId(int orderId)
        {
           return _orderGateway.GetProductListByOrderId(orderId);
        }

        public IEnumerable<ChartModel> GetTotalOrdersByCompanyIdAndYear(int companyId, int year)
        {
            return _orderGateway.GetTotalOrdersByCompanyIdAndYear(companyId, year);
        }

        public IEnumerable<ChartModel> GetTotalOrdersByYear(int year)
        {
            return _orderGateway.GetTotalOrdersByYear(year);
        }
    }


}