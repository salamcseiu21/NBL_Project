using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NblClassLibrary.Models;
using NBL.Models;

namespace NblClassLibrary.DAL
{
    public class CommonGateway:DbGateway
    {
        public IEnumerable<ClientType> GetAllClientType
        {
            get
            {
                try
                {
                    List<ClientType> clientTypes = new List<ClientType>();
                    ConnectionObj.Open();
                    CommandObj.CommandText = "spGetAllClientType";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        ClientType clientType = new ClientType
                        {
                            ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                            ClientTypeName = reader["ClientTypeName"].ToString()

                        };
                        clientTypes.Add(clientType);
                    }
                    reader.Close();
                    return clientTypes;
                }
                catch (Exception e)
                {
                    throw new Exception("Could not Collect Client Type", e);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }
        public IEnumerable<ProductCategory> GetAllProductCategory
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllProductCategory";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    List<ProductCategory> categories = new List<ProductCategory>();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductCategory aCategory = new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["ProductCategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString(),
                            CompanyId = Convert.ToInt32(reader["CompanyId"])
                        };
                        categories.Add(aCategory);
                    }
                    reader.Close();
                    return categories.OrderBy(n => n.ProductCategoryName).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Could not Collect Product Category", e);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }
        public IEnumerable<ProductType> GetAllProductType
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllProductType";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    List<ProductType> types = new List<ProductType>();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductType type = new ProductType
                        {
                            ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                            ProductTypeName = reader["ProductTypeName"].ToString()
                        };
                        types.Add(type);
                    }
                    reader.Close();
                    return types.OrderBy(n => n.ProductTypeName).ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Could not Collect Product Category", e);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }

        public IEnumerable<Branch> GetAssignedBranchesToUserByUserId(int userId)
        {
            try
            {

                CommandObj.CommandText = "spGetAssignedBranchToUserByUserId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Branch> branches = new List<Branch>();
                while (reader.Read())
                {
                    Branch branch = new Branch
                    {
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        BranchEmail = reader["Email"].ToString(),
                        BranchAddress = reader["BranchAddress"].ToString(),
                        BranchPhone = reader["Phone"].ToString(),
                        BranchOpenigDate = Convert.ToDateTime(reader["BranchOpenigDate"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString()
                    };
                    branches.Add(branch);
                }
                reader.Close();
                return branches;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect assigned Branch info by user id", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<UserRole> GetAllUserRoles
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllRoles";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    List<UserRole> roles = new List<UserRole>();
                    while (reader.Read())
                    {
                        UserRole role = new UserRole
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString(),
                            Notes = reader["Notes"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedAt"])
                        };
                        roles.Add(role);
                    }
                    reader.Close();
                    return roles;
                }
                catch (Exception e)
                {
                    throw new Exception("Could not Collect users roles", e);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }

        public IEnumerable<PaymentType> GetAllPaymentTypes()
        {
            try
            {
                CommandObj.CommandText = "spGetAllPaymentTypes";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<PaymentType> paymentTypes = new List<PaymentType>();
                while (reader.Read())
                {
                    PaymentType pType = new PaymentType
                    {
                        PaymentTypeId = Convert.ToInt32(reader["PaymentTypeId"]),
                        PaymentTypeName = reader["PaymentTypeName"].ToString(),


                    };
                    paymentTypes.Add(pType);
                }
                reader.Close();
                return paymentTypes;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect payment types", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Supplier> GetAllSupplier()
        {
            try
            {
                CommandObj.CommandText = "spGetAllSuppliers";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Supplier> suppliers = new List<Supplier>();
                while (reader.Read())
                {
                    Supplier aSupplier = new Supplier
                    {
                        SupplierId = Convert.ToInt32(reader["SupplierId"]),
                        CompanyName = reader["CompanyName"].ToString(),
                        Address = reader["Address"].ToString(),
                        ContactPersonName = reader["ContactPersonName"].ToString(),
                        City = reader["City"].ToString()

                    };
                    suppliers.Add(aSupplier);
                }
                reader.Close();
                return suppliers;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect suppliers", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<Bank> GetAllBank()
        {
            try
            {
                CommandObj.CommandText = "spGetAllBank";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<Bank> bankList = new List<Bank>();
                while (reader.Read())
                {
                    Bank aBank = new Bank
                    {
                        BankId = Convert.ToInt32(reader["BankId"]),
                        BankName = reader["BankName"].ToString(),
                        BankAccountCode = reader["BankAccountCode"].ToString()
                    };
                    bankList.Add(aBank);
                }
                reader.Close();
                return bankList;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect Bank List", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<BankBranch> GetAllBankBranch()
        {
            try
            {
                CommandObj.CommandText = "spGetAllBankBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<BankBranch> bankBranchList = new List<BankBranch>();
                while (reader.Read())
                {
                    BankBranch aBranch = new BankBranch
                    {
                        BankId = Convert.ToInt32(reader["BankId"]),
                        BankName = reader["BankName"].ToString(),
                        BankAccountCode = reader["BankAccountCode"].ToString(),
                        BankBranchId = Convert.ToInt32(reader["BankBranchId"]),
                        BankBranchName = reader["BankBranchName"].ToString(),
                        BankBranchAccountCode = reader["BankBranchAccountCode"].ToString()
                    };
                    bankBranchList.Add(aBranch);
                }
                reader.Close();
                return bankBranchList;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect Bank Branch List", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<MobileBanking> GetAllMobileBankingAccount()
        {
            try
            {
                CommandObj.CommandText = "spGetAllMobileBankingAccount";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<MobileBanking> accountList = new List<MobileBanking>();
                while (reader.Read())
                {
                    var account = new MobileBanking 
                    {
                        MobileBankingId = Convert.ToInt32(reader["MobileBankingId"]),
                        MobileBankingAccountNo = reader["MobileBankingAccountNo"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        MobileBankingTypeId = Convert.ToInt32(reader["MobileBankingTypeId"]),
                    };
                    accountList.Add(account);
                }
                reader.Close();
                return accountList;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect Mobile Bank Account List", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<SubSubSubAccount> GetAllSubSubSubAccounts()
        {
            try
            {
                CommandObj.CommandText = "spGetAllSubSubSubAccounts";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<SubSubSubAccount> accountList = new List<SubSubSubAccount>();
                while (reader.Read())
                {
                    var account = new SubSubSubAccount
                    {
                        SubSubSubAccountId = Convert.ToInt32(reader["SubSubSubAccountListId"]),
                        SubSubSubAccountName = reader["SubSubSubAccountName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        SubSubSubAccountType = Convert.ToString(reader["SubSubSubAccountType"])
                    };
                    accountList.Add(account);
                }
                reader.Close();
                return accountList;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect sub sub sub Account List", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public SubSubSubAccount GetSubSubSubAccountByCode(string accountCode)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetSubSubSubAccountByCode";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Code", accountCode);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                SubSubSubAccount account = new SubSubSubAccount();
                if(reader.Read())
                {
                     account = new SubSubSubAccount
                    {
                        SubSubSubAccountId = Convert.ToInt32(reader["SubSubSubAccountListId"]),
                        SubSubSubAccountName = reader["SubSubSubAccountName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        SubSubSubAccountType = Convert.ToString(reader["SubSubSubAccountType"])
                    };
                    
                }
                reader.Close();
                return account;
            }
            catch (Exception e)
            {
                throw new Exception("Could not Collect sub sub sub Account by code", e);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public Vat GetCurrentVatByProductId(int productId)
        {
            try
            {
                CommandObj.CommandText = "spGetCurrentVatByProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId",productId);
                ConnectionObj.Open();
                Vat aVat = new Vat();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    aVat =new Vat
                    {
                        VatId=Convert.ToInt32(reader["VatId"]),
                        VatAmount=Convert.ToDecimal(reader["VatAmount"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ApprovedByUserId=Convert.ToInt32(reader["ApproveByUserId"]),
                        UpdateDate=Convert.ToDateTime(reader["UpdateDate"]),
                        UpdateByUserId=Convert.ToInt32(reader["UpdateByUserId"]),
                        EntryStatus=reader["EntryStatus"].ToString(),
                        IsCurrent=reader["IsCurrent"].ToString(),
                        SysDateTime=Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedDate=Convert.ToDateTime(reader["ApproveDate"])
                    };
                    reader.Close();
                }
                return aVat;

            }
            catch(Exception exception)
            {
                throw new Exception("Could not collect Current Vat by Product Id",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            } 
        }

        public Discount GetCurrentDiscountByClientTypeId(int clientTypeId)
        {
            try
            {
                CommandObj.CommandText = "spGetCurrentDiscountByClientTypeId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientTypeId", clientTypeId);
                ConnectionObj.Open();
                Discount discount = new Discount();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    discount = new Discount
                    {
                        DiscountId = Convert.ToInt32(reader["DiscountId"]),
                        DiscountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                        TerritoryId = Convert.ToInt32(reader["TerritoryId"]),
                        ClientTypeId=Convert.ToInt32(reader["TerritoryId"]),
                        ProductTypeId=Convert.ToInt32(reader["ProductTypeId"]),
                        ApprovedByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        UpdateDate = Convert.ToDateTime(reader["UpdateDate"]),
                        UpdateByUserId = Convert.ToInt32(reader["UpdateByUserId"]),
                        EntryStatus = reader["EntryStatus"].ToString(),
                        IsCurrent = reader["IsCurrent"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApprovedDate = Convert.ToDateTime(reader["ApproveDate"])
                    };
                    reader.Close();
                }
                return discount;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Current discount by Client type Id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<dynamic> TestMethod()
        {


            try
            {

                List<dynamic> values = new List<dynamic>();

                CommandObj.CommandText = "UDSP_test";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var a = new
                    {
                       ProductName=reader["ProductName"].ToString(),
                       Vat=Convert.ToDecimal(reader["VatAmount"])
                    };
                    dynamic ad = a;
                    values.Add(ad);
                   
                }
                reader.Close();
                return values;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Current discount by Client type Id", exception);
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