using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NblClassLibrary.Models;

namespace NblClassLibrary.DAL
{
    public class CompanyGateway:DbGateway
    {
        public IEnumerable<Company> GetAll
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllCompany";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();

                    List<Company> companies = new List<Company>();
                    while (reader.Read())
                    {
                        Company aCompany = new Company
                        {
                            CompanyId = Convert.ToInt32(reader["CompanyId"]),
                            CompanyName = reader["CompanyName"].ToString(),
                            Logo = reader["Logo"].ToString()
                        };
                        companies.Add(aCompany);
                    }
                    reader.Close();
                    return companies;
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not collect companies", exception);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }

        public Company GetCompanyById(int companyId)
        {
            try
            {
                CommandObj.CommandText = "spGetCompanyById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();

                Company aCompany = new Company();
                if (reader.Read())
                {
                    aCompany = new Company
                    {
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        CompanyName = reader["CompanyName"].ToString(),
                        Logo = reader["Logo"].ToString()
                    };

                }
                reader.Close();
                return aCompany;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect company", exception);
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