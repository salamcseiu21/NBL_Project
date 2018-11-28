using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.DAL
{
    public class ReportGateway:DbGateway
    {
        internal IEnumerable<ViewClient> GetTopClients()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClients";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect top clients due to Sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        internal IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClientsByBranchId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect top clients by branch Id due to sql Exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients by branch Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        internal IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetTopClientsByBranchIdAndYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewClient> clients = new List<ViewClient>();
                while (reader.Read())
                {
                    clients.Add(new ViewClient
                    {
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        ClientName = reader["ClientName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalDebitAmount = Convert.ToDecimal(reader["TotalDebitAmount"])
                    });
                }
                reader.Close();
                return clients;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect top clients by branch id and Year", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        internal IEnumerable<ViewProduct> GetPopularBatteries()
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetPopularsBatteries";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> batteries = new List<ViewProduct>(); 
                while (reader.Read())
                {
                    batteries.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalSoldQty = Convert.ToInt32(reader["TotalSoldQty"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return batteries;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect popular batteries", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        internal IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_RptGetPopularsBatteriesByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<ViewProduct> batteries = new List<ViewProduct>();
                while (reader.Read())
                {
                    batteries.Add(new ViewProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        TotalSoldQty = Convert.ToInt32(reader["TotalSoldQty"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString()
                    });
                }
                reader.Close();
                return batteries;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect popular batteries by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

    }
}