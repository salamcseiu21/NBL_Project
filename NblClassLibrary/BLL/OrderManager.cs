using System;
using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;
using NBL;

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
        public IEnumerable<Order> GetOrdersByCompanyId(int companyId)
        {
            return _orderGateway.GetOrdersByCompanyId(companyId);
        }

        public IEnumerable<ViewInvoicedOrder> GetOrderListByClientId(int clientId)
        {
           return  _orderGateway.GetOrderListByClientId(clientId);
        }
        public IEnumerable<Order> GetOrdersByBranchAndCompnayId(int branchId, int companyId)
        {
            return _orderGateway.GetOrdersByBranchAndCompnayId(branchId,companyId);
        }
        public IEnumerable<ViewOrder> GetAllOrderWithClientInformationByCompanyId(int companyId)
        { 
            return _orderGateway.GetAllOrderWithClientInformationByCompanyId(companyId);
        }

        public IEnumerable<ViewOrder> GetAllOrderByBranchAndCompanyIdWithClientInformation(int branchId,int companyId)
        {
            return _orderGateway.GetAllOrderByBranchAndCompanyIdWithClientInformation(branchId,companyId);
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

        public string ApproveOrderByNsm(Order order)
        {
            int rowAffected = _orderGateway.ApproveOrderByNsm(order);
            if (rowAffected > 0)
            {
                return "Approved by NSM Successfully!";
            }

            return "Failed to approve by NSM";
        }

        public string ApproveOrderByAdmin(Order order)
        {
            int rowAffected = _orderGateway.ApproveOrderByAdmin(order); 
            if (rowAffected > 0)
            {
                return "Approved by Admin Successfully!";
            }

            return "Failed to approve by Admin";
        }

        public Order GetOrderByOrderId(int orderId)
        {
            ClientManager clientManager=new ClientManager();
            var order = _orderGateway.GetOrderByOrderId(orderId);
            order.Client = clientManager.GetClientDeailsById(order.ClientId);
            return order;
        }

        public int CancelOrder(Order order)
        {
            return _orderGateway.CancelOrder(order);
        }

        public IEnumerable<ViewOrder> GetLatestOrdersByCompanyId(int companyId)
        {
            return _orderGateway.GetLatestOrdersByCompanyId(companyId); 
        }

        public string UpdateOrderDetails(List<OrderDetails> orders)
        {
            int rowAffected = _orderGateway.UpdateOrderDetails(orders);
            return rowAffected > 0 ? "Updated Successfully!" : "Failed to Update";
        }

        public string DeleteProductFromOrderDetails(int orderDetailsId)
        {
            int rowAffected = _orderGateway.DeleteProductFromOrderDetails(orderDetailsId);
            return rowAffected > 0 ? "Deleted Successfully!" : "Failed to Delete";
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

        public bool UpdateOrder(Order order)
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