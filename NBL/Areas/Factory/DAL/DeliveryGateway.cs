
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL;
using NBL.Models;

namespace NBL.Areas.Factory.DAL
{
    public class DeliveryGateway:DbGateway
    {
        public int SaveDeliveryInformation(Delivery aDelivery, IEnumerable<TransferIssueDetails> issueDetails)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveDeliveryInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DeliveryDate", aDelivery.DeliveryDate);
                CommandObj.Parameters.AddWithValue("@DeliveryRef", aDelivery.DeliveryRef);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aDelivery.TransactionRef);
                CommandObj.Parameters.AddWithValue("@Transportation", aDelivery.Transportation);
                CommandObj.Parameters.AddWithValue("@DriverName", aDelivery.DriverName);
                CommandObj.Parameters.AddWithValue("@TransportationCost", aDelivery.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", aDelivery.VehicleNo);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aDelivery.ToBranchId);
                CommandObj.Parameters.AddWithValue("@FromBranchId", aDelivery.FromBranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aDelivery.CompanyId);
                CommandObj.Parameters.AddWithValue("@DeliveredByUserId", aDelivery.DeliveredByUserId);
                CommandObj.Parameters.Add("@DeliveryId", SqlDbType.Int);
                CommandObj.Parameters["@DeliveryId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int deliveryId = Convert.ToInt32(CommandObj.Parameters["@DeliveryId"].Value);
                int rowAffected = SaveDeliveryInformationDetails(issueDetails, deliveryId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save delivery Info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveDeliveryInformationDetails(IEnumerable<TransferIssueDetails> issueDetails, int deliveryId)
        {
            int i = 0;
            foreach (TransferIssueDetails tr in issueDetails)
            {
                CommandObj.CommandText = "spSaveDeliveryInformationDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", tr.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", tr.Quantity);
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