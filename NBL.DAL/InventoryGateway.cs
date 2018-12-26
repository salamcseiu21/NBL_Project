using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class InventoryGateway:DbGateway,IInventoryGateway
    {
        public IEnumerable<ViewProduct> GetStockProductByBranchAndCompanyId(int branchId, int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductInByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<ViewProduct> GetStockProductByCompanyId(int companyId)
        {

            try
            {
                CommandObj.CommandText = "UDSP_GetStockProductByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> products = new List<ViewProduct>();
                while (reader.Read())
                {
                    products.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                        Vat = Convert.ToDecimal(reader["Vat"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public int GetMaxDeliveryRefNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxDeliveryRefOfCurrentYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                int slNo = 0;
                if (reader.Read())
                {
                    slNo = Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return slNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect max delivery ref of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<TransactionModel> GetAllReceiveableProductToBranchByDeliveryRef(string deliveryRef)
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductToBranchByDeliveryRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryRef", deliveryRef);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransactionModel> list = new List<TransactionModel>();
                while (reader.Read())
                {
                    list.Add(new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get receivable product list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<TransactionModel> GetAllReceiveableProductByBranchAndCompanyId(int branchId,int companyId) 
        {
            try
            {
                CommandObj.CommandText = "spGetReceiveableProductByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId",companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();

                List<TransactionModel> list = new List<TransactionModel>();
                while (reader.Read())
                {
                    list.Add(new TransactionModel
                    {
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        UserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        TransactionDate = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        DriverName = reader["DriverName"].ToString(),
                        VehicleNo = reader["VehicleNo"].ToString(),
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"])
                    });
                }

                reader.Close();
                return list;

            }
            catch (Exception exception)
            {

                throw new Exception("Could not Get receivable product list", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int ReceiveProduct(List<TransactionModel> receiveProductList,TransactionModel model)
        {
            
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spReceiveProuctToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", model.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.ToBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", model.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int rowAffected = SaveReceiveProductDetails(receiveProductList, inventoryId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not receive product", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveReceiveProductDetails(List<TransactionModel> receiveProductList, int inventoryId)
        {
            int i = 0;
            foreach (var item in receiveProductList) 
            {
                CommandObj.CommandText = "spSaveReceiveProduct";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@StockQuantity", item.StockQuantity);
                CommandObj.Parameters.AddWithValue("@CostPrice", item.CostPrice);
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }
        public int GetStockQtyByBranchAndProductId(int branchId, int productId)
        {
            try
            {
                int stockQty = 0;
                CommandObj.CommandText = "spGetStockQtyByBranchIdAndProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    stockQty = Convert.ToInt32(reader["StockQuantity"]);
                }
                reader.Close();
                return stockQty;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect Stock Qty", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Save(List<InvoiceDetails> invoicedOrders, Delivery aDelivery, int invoiceStatus,int orderStatus)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.CommandText = "spSaveDeliveredOrderInformation";
                CommandObj.Parameters.AddWithValue("@TransactionDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", aDelivery.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@InvoiceId",aDelivery.InvoiceId);
                CommandObj.Parameters.AddWithValue("@InvoiceStatus", invoiceStatus);
                CommandObj.Parameters.AddWithValue("@OrderStatus", orderStatus);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation);
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName);
                CommandObj.Parameters.AddWithValue("@DriverPhone", aDelivery.DriverPhone);
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.Add("@InventoryId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryId"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int inventoryId = Convert.ToInt32(CommandObj.Parameters["@InventoryId"].Value);
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);

                int rowAffected = SaveDeliveredOrderDetails(invoicedOrders, inventoryId, deliveryId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Order", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveDeliveredOrderDetails(List<InvoiceDetails> invoicedOrders, int inventoryId,int deliveryId)
        {
            int i = 0;
            foreach (var item in invoicedOrders)
            {
                CommandObj.CommandText = "spSaveDeliveredOrderDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@InventoryId", inventoryId);
                CommandObj.Parameters.AddWithValue("@ProductId", item.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", item.Quantity);
                CommandObj.Parameters.AddWithValue("@CostPrice", item.UnitPrice);
                CommandObj.Parameters.AddWithValue("@StockQuantity", item.StockQuantity);
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }
    }
}