﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IOrderManager
    {
        IEnumerable<Order> GetAll();

        IEnumerable<Order> GetOrdersByBranchId(int branchId);
       
        IEnumerable<ViewOrder> GetOrdersByCompanyId(int companyId);
       

        IEnumerable<ViewInvoicedOrder> GetOrderListByClientId(int clientId);
        

        IEnumerable<ViewOrder> GetOrdersByBranchAndCompnayId(int branchId, int companyId);
       

        IEnumerable<ViewOrder> GetAllOrderWithClientInformationByCompanyId(int companyId);
       

        IEnumerable<ViewOrder> GetAllOrderByBranchAndCompanyIdWithClientInformation(int branchId, int companyId);
       

        IEnumerable<ViewOrder> GetOrdersByBranchCompanyAndNsmUserId(int branchId, int companyId, int nsmUserId);
       

        IEnumerable<ViewOrder> GetOrdersByNsmUserId(int nsmUserId);
      

        IEnumerable<ViewOrder> GetOrdersByBranchIdCompanyIdAndStatus(int branchId, int companyId, int status);
        

        IEnumerable<ViewOrder> GetPendingOrdersByBranchAndCompanyId(int branchId, int companyId);
       

        IEnumerable<ViewOrder> GetLatestOrdersByBranchAndCompanyId(int branchId, int companyId);
        

        IEnumerable<OrderDetails> GetOrderDetailsByOrderId(int orderId);
       

        int Save(Order order);
       
        string GenerateOrderRefNo(int maxsl);
      

        string GenerateOrderSlipNo(int maxSl);
      

        string GetReferenceAccountCodeById(int subReferenceAccountId);
       

        string ApproveOrderByNsm(ViewOrder order);
    

        string ApproveOrderByAdmin(ViewOrder order);
        
        ViewOrder GetOrderByOrderId(int orderId);
        
        bool CancelOrder(ViewOrder order);
        
        IEnumerable<ViewOrder> GetLatestOrdersByCompanyId(int companyId);
       
        string UpdateOrderDetails(IEnumerable<OrderItem> orderItems);
        
        bool DeleteProductFromOrderDetails(int orderItemId);
        
        string GetDiscountAccountCodeByClintTypeId(int typeId);
        

        IEnumerable<OrderDetails> GetAllOrderDetails();
        

        Order GetOrderInfoByTransactionRef(string transactionRef);
        
        bool AddNewItemToExistingOrder(Product aProduct, int orderId);
        bool UpdateOrder(ViewOrder order);
        
        IEnumerable<ChartModel> GetTotalOrdersOfCurrentYearByCompanyId(int companyId);
        

        IEnumerable<ChartModel> GetTotalOrdersByBranchIdCompanyIdAndYear(int branchId, int companyId, int year);
        

        IEnumerable<Order> GetOrdersByClientId(int clientId);
       

        List<Product> GetProductListByOrderId(int orderId);
        

        IEnumerable<ChartModel> GetTotalOrdersByCompanyIdAndYear(int companyId, int year);
        

        IEnumerable<ChartModel> GetTotalOrdersByYear(int year);
        

        ViewOrderSlipModel GetOrderSlipByOrderId(int orderId);
        

        IEnumerable<ViewOrder> GetDelayedOrdersToSalesPersonByBranchAndCompanyId(int branchId, int companyId);
        

        IEnumerable<ViewOrder> GetDelayedOrdersToNsmByBranchAndCompanyId(int branchId, int companyId);
        

        IEnumerable<ViewOrder> GetDelayedOrdersToAdminByBranchAndCompanyId(int branchId, int companyId);
        

        bool UpdateVerificationStatus(int orderId, string verificationNote, int userUserId);
        

        IEnumerable<ViewVerifiedOrderModel> GetVerifiedOrdersByBranchAndCompanyId(int branchId, int companyId);

    }
}
