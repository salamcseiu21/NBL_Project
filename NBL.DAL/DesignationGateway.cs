using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.Models;

namespace NBL.DAL
{
    public class DesignationGateway:DbGateway
    {
        public IEnumerable<Designation> GetAll
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllDesignation";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    List<Designation> designations = new List<Designation>();
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        designations.Add(new Designation
                        {
                            DesignationId = Convert.ToInt32(reader["DesignationId"]),
                            DesignationName = reader["DesignationName"].ToString(),
                            DesignationCode = reader["DesignationCode"].ToString(),
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            Department = new Department
                            {
                                DepartmentCode = reader["DepartmentCode"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString()
                            }
                        });
                    }
                    reader.Close();
                    return designations;
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not collect designations", exception);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }

        public Designation GetDesignationByCode(string code)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDesignationByCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DesignationCode", code);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Designation aDesignation = new Designation();
                if (reader.Read())
                {

                    aDesignation.DesignationId = Convert.ToInt32(reader["DesignationId"]);
                    aDesignation.DesignationCode = reader["DesignationCode"].ToString();
                    aDesignation.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                    aDesignation.Department = new Department
                    {
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        DepartmentName = reader["DesignationName"].ToString()
                    };

                }
                reader.Close();
                return aDesignation;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect designation by Code", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Designation GetDesignationById(int designationId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDesignationById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DesignationId", designationId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Designation aDesignation=null;
                if (reader.Read())
                {
                    aDesignation = new Designation
                    {
                        DesignationId = Convert.ToInt32(reader["DesignationId"]),
                        DesignationCode = reader["DesignationCode"].ToString(),
                        DesignationName = reader["DesignationName"].ToString(),
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        Department = new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DesignationName"].ToString()
                        }


                   }; 
                }
                reader.Close();
                return aDesignation;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect designation by Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Update(Designation aDesignation)
        {
            try
            {
                CommandObj.CommandText = "UDSP_UpdateDesignationInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", aDesignation.DepartmentId);
                CommandObj.Parameters.AddWithValue("@DesignationCode", aDesignation.DesignationCode);
                CommandObj.Parameters.AddWithValue("@DesignationName", aDesignation.DesignationName);
                CommandObj.Parameters.AddWithValue("@DesignationId", aDesignation.DesignationId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Update designation information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int Save(Designation aDesignation)
        {
            try
            {
                CommandObj.CommandText = "spAddNewDesignation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", aDesignation.DepartmentId);
                CommandObj.Parameters.AddWithValue("@DesignationCode", aDesignation.DesignationCode);
                CommandObj.Parameters.AddWithValue("@DesignationName", aDesignation.DesignationName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Save Designation", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public List<Designation> GetDesignationsByDepartmentId(int departmentId)
        {

            try
            {
                List<Designation> designations=new List<Designation>();
                CommandObj.CommandText = "UDSP_GetDesignationsByDepartmentId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", departmentId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while(reader.Read())
                {

                   designations.Add(new Designation
                   {
                       DesignationId = Convert.ToInt32(reader["DesignationId"]),
                       DesignationCode = reader["DesignationCode"].ToString(),
                       DesignationName =reader["DesignationName"].ToString()
                   });
                }
                reader.Close();
                return designations;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect designations by departments", exception);
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