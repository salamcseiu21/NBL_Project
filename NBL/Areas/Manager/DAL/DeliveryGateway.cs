﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using NBL.DAL;
using NBL.Models;

namespace NBL.Areas.Manager.DAL
{
    public class DeliveryGateway:DbGateway
    {
        public int ChangeOrderStatusByManager(Order aModel)
        {
            try
            {

                CommandObj.CommandText = "spChangeOrderStatusByManager";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@OrderAndTransctionId", aModel.OrderId);
                CommandObj.Parameters.AddWithValue("@Status ", aModel.Status);
                CommandObj.Parameters.AddWithValue("@DeliveryDate", aModel.DeliveryDateTime);
                CommandObj.Parameters.AddWithValue("@DeliveryOrRcvUserId", aModel.DeliveredOrReceiveUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Change the Order status", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Delivery GetOrderByDeliveryId(int deliveryId)
        {
            try
            {
                CommandObj.CommandText = "spGetOrderByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Delivery aModel = new Delivery();
                if (reader.Read())
                {
                    aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"])
                    };
                }
                reader.Close();
                return aModel;
            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<DeliveryModel> GetAllInvoiceOrderListByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "spGetAllInvoiceListByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ToBranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<DeliveryModel> orders = new List<DeliveryModel>();
                while (reader.Read())
                {
                    DeliveryModel aModel = new DeliveryModel
                    {
                        InventoryId = Convert.ToInt32(reader["BranchInventoryId"]),
                        BranchId = Convert.ToInt32(reader["ToBranchId"]),
                        Invoice = reader["InvoiceNo"].ToString(),
                        TransactionDate = Convert.ToDateTime(reader["TransactionDate"]),
                        Transactionid = Convert.ToInt32(reader["Transactionid"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Status = Convert.ToInt32(reader["Status"])
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Invoiced Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Invoiced Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<DeliveryDetails> GetDeliveredOrderDetailsByDeliveryId(int deliveryId)
        {
            try
            {

                CommandObj.CommandText = "spGetDeliveredOrderDetailsByDeliveryId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryId", deliveryId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<DeliveryDetails> orders = new List<DeliveryDetails>();
                while (reader.Read())
                {
                    DeliveryDetails aModel = new DeliveryDetails
                    {
                        DeliveryId = deliveryId,
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId=Convert.ToInt32(reader["CategoryId"]),
                        CategoryName=reader["ProductCategoryName"].ToString()
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders details due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered details Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Delivery> GetAllDeliveredOrders()
        {
            try
            {
                CommandObj.CommandText = "spGetAllDeliveredOrders";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        InvoiceId=Convert.ToInt32(reader["InvoiceId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        SysDateTime=Convert.ToDateTime(reader["SysDateTime"]),
                        DriverName = reader["DriverName"].ToString(),
                        Transportation = reader["Transportation"].ToString(),
                        TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                        VehicleNo = reader["VehicleNo"].ToString()
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered Orders", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                CommandObj.CommandText = "spGetAllDeliveredOrdersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId",branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = branchId,
                        FromBranchId = branchId,
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered Orders by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByBranchCompanyAndUserId(int branchId, int companyId,int deliveredByUserId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllDeliveredOrdersByBranchCompanyAndUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", deliveredByUserId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = branchId,
                        FromBranchId = branchId,
                        CompanyId = companyId,
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Transport = new Transport
                        {
                            DriverName = reader["DriverName"].ToString(),
                            DriverPhone = reader["DriverPhone"].ToString(),
                            Transportation = reader["Transportation"].ToString(),
                            TransportationCost = Convert.ToDecimal(reader["TransportationCost"]),
                            VehicleNo = reader["VehicleNo"].ToString()
                        }
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered Orders by branch,Company and User Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<Delivery> GetAllDeliveredOrdersByInvoiceRef(string invoiceRef)
        {
            try
            {
                CommandObj.CommandText = "spGetAllDeliveredOrderByInvoiceRef";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@InvoiceRef",invoiceRef);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Delivery> orders = new List<Delivery>();
                while (reader.Read())
                {
                    Delivery aModel = new Delivery
                    {
                        DeliveryId = Convert.ToInt32(reader["DeliveryId"]),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        DeliveryRef = reader["DeliveryRef"].ToString(),
                        DeliveryDate = Convert.ToDateTime(reader["DeliveryDate"]),
                        TransactionRef = reader["TransactionRef"].ToString(),
                        DeliveredByUserId = Convert.ToInt32(reader["DeliveredByUserId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductId=Convert.ToInt32(reader["ProductId"]),
                        Quantity=Convert.ToInt32(reader["Quantity"])
                    };
                    orders.Add(aModel);
                }
                reader.Close();
                return orders;

            }
            catch (SqlException exception)
            {
                throw new Exception("Could not Collect Delivered Orders due to Db Exception", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect Delivered Order by Invoice ref", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
    }
}