using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.DAL
{
    public class VatGateway:DbGateway,IVatGateway
    {
        public int AddVat(Vat vat)
        {
            
            try
            {
                CommandObj.CommandText = "UDSP_AddVat";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", vat.ProductId);
                CommandObj.Parameters.AddWithValue("@VatAmount", vat.VatAmount);
                CommandObj.Parameters.AddWithValue("@UpdateDate", vat.UpdateDate);
                CommandObj.Parameters.AddWithValue("@UpdateByUserId", vat.UpdateByUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not save Vat info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Vat> GetAllPendingVats()
        {
            try
            {
                List<Vat> vats=new List<Vat>();
                CommandObj.CommandText = "UDSP_GetAllPendingVats";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader= CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    vats.Add(new Vat
                    {
                        VatId = Convert.ToInt32(reader["VatId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        VatAmount = Convert.ToDecimal(reader["VatAmount"]),
                        UpdateDate = Convert.ToDateTime(reader["UpdateDate"]),
                        UpdateByUserId = Convert.ToInt32(reader["UpdateByUserId"])

                    });
                }
                reader.Close();
                return vats;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending Vat info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Vat> GetProductWishVat() 
        {
            try
            {
                List<Vat> vats = new List<Vat>();
                CommandObj.CommandText = "UDSP_GetProductWishVat";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    vats.Add(new Vat
                    {
                        VatId = Convert.ToInt32(reader["VatId"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        VatAmount = Convert.ToDecimal(reader["VatAmount"]),
                        UpdateDate = Convert.ToDateTime(reader["UpdateDate"]),
                        UpdateByUserId = Convert.ToInt32(reader["UpdateByUserId"])

                    });
                }
                reader.Close();
                return vats;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Vat info", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
    }
}