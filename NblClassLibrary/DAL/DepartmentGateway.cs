using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NblClassLibrary.DAL 
{
    public class DepartmentGateway:DbGateway
    {
        readonly DesignationGateway _designationGateway=new DesignationGateway();
        readonly EmployeeManager _employeeManager=new EmployeeManager();
        public IEnumerable<Department> GetAll
        {
            get
            {
                
                try
                {
                    CommandObj.CommandText = "spGetAllDepartment";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    List<Department> departments=new List<Department>();
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        Department aDepartment = new Department
                        {
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            DepartmentCode = reader["DepartmentCode"].ToString(),
                            //Designations = GetDesignationListByDepartmentId(Convert.ToInt32(reader["DepartmentId"]))
                        };
                        departments.Add(aDepartment);
                    }
                    reader.Close();
                    foreach (Department department in departments)
                    {
                        department.Designations=_designationGateway.GetDesignationsByDepartmentId(department.DepartmentId);
                        department.Employees = _employeeManager.GetEmpoyeeListByDepartmentId(department.DepartmentId).ToList();
                    }
                    return departments;
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not collect department",exception);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }
        internal Department GetDepartmentByCode(string code)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDeparmentByCode"; 
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentCode", code);
                List<Department> departments = new List<Department>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Department aDepartment = new Department();
                if (reader.Read())
                {

                    aDepartment.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
                    aDepartment.DepartmentName = reader["DepartmentName"].ToString();
                    aDepartment.DepartmentCode = reader["DepartmentCode"].ToString();

                }
                reader.Close();
                return aDepartment;
                
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect department by Code", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public Department GetDepartmentById(int deptId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetDepartmentById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", deptId);
                Department aDepartment = null;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    aDepartment=new Department
                    {
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        DepartmentCode = reader["DepartmentCode"].ToString(),
                        DepartmentName = reader["DepartmentName"].ToString()
                    };
                }
                reader.Close();
                return aDepartment;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not get Department by Id",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        internal int Update(Department aDepartment)
        {
            try
            {
                CommandObj.CommandText = "spUpdateDepartmentInformation";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentId", aDepartment.DepartmentId);
                CommandObj.Parameters.AddWithValue("@DepartmentCode", aDepartment.DepartmentCode);
                CommandObj.Parameters.AddWithValue("@DepartmentName", aDepartment.DepartmentName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Update department information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        internal int Save(Department aDepartment)
        {
            try
            {
                CommandObj.CommandText = "spAddNewDepartment";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DepartmentCode", aDepartment.DepartmentCode);
                CommandObj.Parameters.AddWithValue("@DepartmentName", aDepartment.DepartmentName);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (SqlException sqlException)
            {
                if (sqlException.Number == 2601)
                {
                    throw new Exception("Cannot insert duplicate key", sqlException);
                }
                throw new Exception("Could not Save department", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Save department", exception);
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