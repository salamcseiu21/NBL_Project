﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL
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
                        clientTypes.Add(new ClientType
                        {
                            ClientTypeId = Convert.ToInt32(reader["ClientTypeId"]),
                            ClientTypeName = reader["ClientTypeName"].ToString()

                        });
                    }
                    reader.Close();
                    return clientTypes;
                }
                catch (SqlException sqlException)
                {
                    throw new Exception("Could not Collect Client Type due to Sql Exception", sqlException);
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not Collect Client Type", exception);
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
                        categories.Add(new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["ProductCategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString(),
                            CompanyId = Convert.ToInt32(reader["CompanyId"])
                        });
                    }
                    reader.Close();
                    return categories.OrderBy(n => n.ProductCategoryName).ToList();
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not Collect Product Category", exception);
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
                        types.Add(new ProductType
                        {
                            ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                            ProductTypeName = reader["ProductTypeName"].ToString()
                        });
                    }
                    reader.Close();
                    return types.OrderBy(n => n.ProductTypeName).ToList();
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not Collect Product Category", exception);
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
                    branches.Add(new Branch
                    {
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        BranchName = reader["BranchName"].ToString(),
                        BranchEmail = reader["Email"].ToString(),
                        BranchAddress = reader["BranchAddress"].ToString(),
                        BranchPhone = reader["Phone"].ToString(),
                        BranchOpenigDate = Convert.ToDateTime(reader["BranchOpenigDate"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString()
                    });
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
                        roles.Add(new UserRole
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString(),
                            Notes = reader["Notes"].ToString(),
                            CreatedDate = Convert.ToDateTime(reader["CreatedAt"])
                        });
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
                    paymentTypes.Add(new PaymentType
                    {
                        PaymentTypeId = Convert.ToInt32(reader["PaymentTypeId"]),
                        PaymentTypeName = reader["PaymentTypeName"].ToString(),


                    });
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
        public IEnumerable<TransactionType> GetAllTransactionTypes()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllTransactionTypes";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransactionType> transactionTypes = new List<TransactionType>(); 
                while (reader.Read())
                {
                    transactionTypes.Add(new TransactionType
                    {
                        TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                        TransactionTypeName = reader["TransactionTypeName"].ToString()
                    });
                }
                reader.Close();
                return transactionTypes;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not Collect transaction types due to sql exception", sqlException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not Collect transaction types", exception);
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
                    suppliers.Add(new Supplier
                    {
                        SupplierId = Convert.ToInt32(reader["SupplierId"]),
                        CompanyName = reader["CompanyName"].ToString(),
                        Address = reader["Address"].ToString(),
                        ContactPersonName = reader["ContactPersonName"].ToString(),
                        City = reader["City"].ToString()
                    });
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
                    bankList.Add(new Bank
                    {
                        BankId = Convert.ToInt32(reader["BankId"]),
                        BankName = reader["BankName"].ToString(),
                        BankAccountCode = reader["BankAccountCode"].ToString()
                    });
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
                    bankBranchList.Add(new BankBranch
                    {
                        BankId = Convert.ToInt32(reader["BankId"]),
                        BankName = reader["BankName"].ToString(),
                        BankAccountCode = reader["BankAccountCode"].ToString(),
                        BankBranchId = Convert.ToInt32(reader["BankBranchId"]),
                        BankBranchName = reader["BankBranchName"].ToString(),
                        BankBranchAccountCode = reader["BankBranchAccountCode"].ToString()
                    });
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
                    accountList.Add(new MobileBanking
                    {
                        MobileBankingId = Convert.ToInt32(reader["MobileBankingId"]),
                        MobileBankingAccountNo = reader["MobileBankingAccountNo"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        MobileBankingTypeId = Convert.ToInt32(reader["MobileBankingTypeId"]),
                    });
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
                    accountList.Add(new SubSubSubAccount
                    {
                        SubSubSubAccountId = Convert.ToInt32(reader["SubSubSubAccountListId"]),
                        SubSubSubAccountName = reader["SubSubSubAccountName"].ToString(),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        SubSubSubAccountType = Convert.ToString(reader["SubSubSubAccountType"])
                    });
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

        public IEnumerable<ReferenceAccount> GetAllReferenceAccounts()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllReferenceAccount";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ReferenceAccount> accounts=new List<ReferenceAccount>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    accounts.Add(new ReferenceAccount
                    {
                        Id = Convert.ToInt32(reader["ReferenceAccountId"]),
                        Description = reader["Description"].ToString(),
                        Code = reader["ReferenceCode"].ToString().Trim(),
                        Name = reader["Name"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"])
                    });
                }
                reader.Close();
                return accounts;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect reference accounts due to sql exception", sqlException.InnerException);
            }
            catch (Exception exception)
            {
                throw  new Exception("Could not collect reference accounts",exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<ViewReferenceAccountModel> GetAllSubReferenceAccounts() 
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllSubReferenceAccountList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewReferenceAccountModel> accounts = new List<ViewReferenceAccountModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    accounts.Add(new ViewReferenceAccountModel
                    {
                        Id = Convert.ToInt32(reader["SubReferenceAccountId"]),
                        Code = reader["Code"].ToString().Trim(),
                        ReferenceAccountCode = reader["ReferenceAccountCode"].ToString(),
                        Name = reader["Name"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"])
                    });
                }
                reader.Close();
                return accounts;
            }
            catch (SqlException sqlException)
            {
                throw new Exception("Could not collect sub reference accounts due to sql exception", sqlException.InnerException);
            }
            catch (Exception exception)
            {
                throw new Exception("Could not  collect sub reference accounts", exception.InnerException);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Status> GetAllStatus()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllStatus";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<Status> statusList=new List<Status>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    statusList.Add(new Status
                    {
                        Id = Convert.ToInt32(reader["StatusId"]),
                        Flag = Convert.ToInt32(reader["Flag"]),
                        Description = reader["Description"].ToString()
                    });
                }
                reader.Close();
                return statusList;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect status",exception.InnerException);
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