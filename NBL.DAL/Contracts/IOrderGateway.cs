﻿using System.Collections.Generic;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IOrderGateway
   {
       IEnumerable<Order> GetAll();
       IEnumerable<Order> GetOrdersByBranchId(int branchId);
       IEnumerable<ViewOrder> GetLatestOrdersByCompanyId(int companyId);
       IEnumerable<ViewOrder> GetOrdersByCompanyId(int companyId);
       IEnumerable<ViewOrder> GetOrdersByBranchAndCompnayId(int branchId, int companyId);
       IEnumerable<ViewInvoicedOrder> GetOrderListByClientId(int clientId);
       IEnumerable<ViewOrder> GetOrdersByBranchIdCompanyIdAndStatus(int branchId, int companyId, int status);
       IEnumerable<ViewOrder> GetPendingOrdersByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewOrder> GetAllOrderWithClientInformationByCompanyId(int companyId);
       IEnumerable<OrderItem> GetOrderItemsByOrderId(int orderId);
       IEnumerable<ViewOrder> GetAllOrderByBranchAndCompanyIdWithClientInformation(int branchId, int companyId);
       IEnumerable<ViewOrder> GetOrdersByBranchCompanyAndNsmUserId(int branchId, int companyId, int nsmUserId);
       IEnumerable<ViewOrder> GetOrdersByNsmUserId(int nsmUserId);
       IEnumerable<ViewOrder> GetLatestOrdersByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<OrderDetails> GetAllOrderDetails();
       IEnumerable<OrderDetails> GetOrderDetailsByOrderId(int orderId);
       ViewOrder GetOrderByOrderId(int orderId);
       int GetOrderMaxSerialNoByYear(int year);
       Order GetOrderInfoByTransactionRef(string transactionRef);
       IEnumerable<ChartModel> GetTotalOrdersOfCurrentYearByCompanyId(int companyId);
       IEnumerable<ChartModel> GetTotalOrdersByBranchIdCompanyIdAndYear(int branchId, int companyId, int year);
       IEnumerable<ChartModel> GetTotalOrdersByCompanyIdAndYear(int companyId, int year);
       IEnumerable<ChartModel> GetTotalOrdersByYear(int year);
       int Save(Order order);
       int UpdateOrder(ViewOrder order);
       int AddNewItemToExistingOrder(Product aProduct, int orderId);
       int DeleteProductFromOrderDetails(int orderItemId);
       int CancelOrder(ViewOrder order);
       int ApproveOrderByNsm(ViewOrder aModel);
       int ApproveOrderByAdmin(ViewOrder order);
       int UpdateOrderDetails(IEnumerable<OrderItem> orderItems);
       IEnumerable<Order> GetOrdersByClientId(int clientId);
       List<Product> GetProductListByOrderId(int orderId);
       IEnumerable<ViewOrder> GetDelayedOrdersToSalesPersonByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewOrder> GetDelayedOrdersToNsmByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<ViewOrder> GetDelayedOrdersToAdminByBranchAndCompanyId(int branchId, int companyId);
       int UpdateVerificationStatus(int orderId, string verificationNote, int userUserId);
       IEnumerable<ViewVerifiedOrderModel> GetVerifiedOrdersByBranchAndCompanyId(int branchId, int companyId);
   }
}
