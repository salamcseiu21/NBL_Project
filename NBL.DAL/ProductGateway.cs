using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL
{
    public class ProductGateway:DbGateway
    {
        public IEnumerable<Product> GetAll
        {
            get
            {
                try
                {
                    CommandObj.CommandText = "spGetAllProduct";
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    List<Product> products = new List<Product>();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            Vat = Convert.ToDecimal(reader["VatAmount"]),
                            CompanyId = Convert.ToInt32(reader["CompanyId"])
                        });
                    }

                    reader.Close();
                    return products;
                }
                catch (Exception exception)
                {
                    throw  new Exception("Colud not collect product list",exception);
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            }
        }

        public IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                CommandObj.CommandText = "spGetAllProductByBranchAndCompanyId";
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
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        ProductCategoryName = reader["ProductCategoryName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Vat = Convert.ToDecimal(reader["Vat"])
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        
        public Product GetProductByProductAndClientTypeId(int productId,int clientTypeId)
        {
            try
            {
                CommandObj.CommandText = "spGetProductByProductAndClientTypeId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientTypeId", clientTypeId);
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                Product product = new Product();
                if (reader.Read())
                {
                    product = new Product 
                    {
                        ProductId = productId,
                        ProductName = reader["ProductName"].ToString(),
                        UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        Vat = Convert.ToDecimal(reader["VatAmount"]),
                        VatId=Convert.ToInt32(reader["VatId"]),
                        DiscountAmount= Convert.ToDecimal(reader["DiscountAmount"]),
                        DiscountId=Convert.ToInt32(reader["DiscountId"]),
                        SalePrice=Convert.ToDecimal(reader["SalePrice"]),
                        ProductDetailsId=Convert.ToInt32(reader["ProductDetailsId"])

                    };
                }

                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect product list by branch and company id", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxTransferIssueNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxTransferIssueNoOfCurrentYear";
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
                throw new Exception("Could not collect max transfer issue no of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        //-----------------Load deliverable Issue list----------------
        public IEnumerable<TransferIssue> GetDeliverableTransferIssueList()
        {
            try
            {
                CommandObj.CommandText = "spGetDeliverableTransferIssueList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferIssue> issueList = new List<TransferIssue>(); 
                while (reader.Read())
                {
                    issueList.Add(new TransferIssue
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"])
                    });
                }

                reader.Close();
                return issueList;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issued list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int ApproveTransferIssue(TransferIssue transferIssue)
        {
            try
            {
                CommandObj.CommandText = "spApproveTransferIssue";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", transferIssue.TransferIssueId);
                CommandObj.Parameters.AddWithValue("@ApproveByUserId", transferIssue.ApproveByUserId);
                CommandObj.Parameters.AddWithValue("@ApproveDateTime", transferIssue.ApproveDateTime);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int IssueProductToTransfer(TransferIssue aTransferIssue)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveTransferIssue";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueDate", aTransferIssue.TransferIssueDate);
                CommandObj.Parameters.AddWithValue("@FromBranchId", aTransferIssue.FromBranchId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", aTransferIssue.ToBranchId);
                CommandObj.Parameters.AddWithValue("@IssueByUserId", aTransferIssue.IssueByUserId);
                CommandObj.Parameters.AddWithValue("@TransferIssueRef", aTransferIssue.TransferIssueRef);
                CommandObj.Parameters.Add("@TransferIssueId", SqlDbType.Int);
                CommandObj.Parameters["@TransferIssueId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int transferIssueId = Convert.ToInt32(CommandObj.Parameters["@TransferIssueId"].Value);
                int rowAffected = SaveTransferIssueDetails(aTransferIssue.Products, transferIssueId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Transfer issue Info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id)
        {
            try
            {
                CommandObj.CommandText = "spGetTransferIssueDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransferIssueId", id);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferIssueDetails> products = new List<TransferIssueDetails>();
                while (reader.Read())
                {
                    products.Add(new TransferIssueDetails
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        TransferIssueDetailsId = Convert.ToInt32(reader["TransferIssueDetailsId"])
                    });
                }

                reader.Close();
                return products;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issued product list by id ", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<TransferIssue> GetTransferIssueList()  
        {
            try
            {
                CommandObj.CommandText = "spGetTransferIssueList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                List<TransferIssue> issueList = new List<TransferIssue>();
                while (reader.Read())
                {
                    issueList.Add(new TransferIssue
                    {
                        TransferIssueId = Convert.ToInt32(reader["TransferIssueId"]),
                        TransferIssueDate = Convert.ToDateTime(reader["TransferIssueDate"]),
                        TransferIssueRef = reader["TransferIssueRef"].ToString(),
                        ToBranchId = Convert.ToInt32(reader["ToBranchId"]),
                        FromBranchId = Convert.ToInt32(reader["FromBranchId"]),
                        IssueByUserId = Convert.ToInt32(reader["IssueByUserId"]),
                        Status = Convert.ToInt16(reader["Status"]),
                        Cancel = Convert.ToChar(reader["Cancel"]),
                        EntryStatus = Convert.ToChar(reader["EntryStatus"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ApproveByUserId = Convert.ToInt32(reader["ApproveByUserId"]),
                        ApproveDateTime = Convert.ToDateTime(reader["ApproveDateTime"])
                    });
                }

                reader.Close();
                return issueList;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not collect transfer issued list", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveTransferIssueDetails(List<Product> products, int transferIssueId)
        {
            int i = 0;
            foreach (Product product in products)
            {
                CommandObj.CommandText = "spSaveTransferIssueDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", product.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", product.Quantity);
                CommandObj.Parameters.AddWithValue("@TransferIssueId", transferIssueId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

      
        public int GetProductMaxSerialNo()
        {
            try
            {
                int maxSlno = 0;
                CommandObj.CommandText = "spGetProductMaxSerialNo";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                   maxSlno=Convert.ToInt32(reader["MaxSlNo"]);
                }
                reader.Close();
                return maxSlno;
            }
            catch (Exception exception)
            {
                throw new Exception("Colud not get  product max serial", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }
        public IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId)
        {
          
                try
                {
                    CommandObj.CommandText = "spGetPoructByProductCategoryId";
                    CommandObj.Parameters.AddWithValue("@ProductCategoryId", productCategoryId);
                    CommandObj.CommandType = CommandType.StoredProcedure;
                    ConnectionObj.Open();
                    SqlDataReader reader = CommandObj.ExecuteReader();
                    List<ViewProduct> products = new List<ViewProduct>();
                    while (reader.Read())
                    {
                        products.Add(new ViewProduct
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductAddedDate = Convert.ToDateTime(reader["ProductAddedDate"]),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            ProductCategoryName = reader["ProductCategoryName"].ToString(),
                            ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                            ProductTypeName = reader["ProductTypeName"].ToString(),
                            Unit = reader["Unit"].ToString(),
                            UnitInStock = Convert.ToInt32(reader["UnitInStock"]),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                            DealerComision = Convert.ToDecimal(reader["DealerComision"]),
                            DealerPrice = Convert.ToDecimal(reader["DealerPrice"]),
                            Vat = Convert.ToDecimal(reader["Vat"]),
                            DiscountAmount = Convert.ToDecimal(reader["DiscountAmount"])

                        });
                    }
                    reader.Close();
                    return products;
                }
                finally
                {
                    ConnectionObj.Close();
                    CommandObj.Dispose();
                    CommandObj.Parameters.Clear();
                }
            
        }
        public int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model)
        {

            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Parameters.Clear();
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spTransferProductToBranch";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", model.TransactionDate);
                CommandObj.Parameters.AddWithValue("@FromBranchId", model.FromBranchId);
                CommandObj.Parameters.AddWithValue("@ToBranchId", model.ToBranchId);
                CommandObj.Parameters.AddWithValue("@UserId", model.UserId);
                CommandObj.Parameters.AddWithValue("@TransctionId", model.TransactionId);
                CommandObj.Parameters.AddWithValue("@Transportation", model.Transportation);
                CommandObj.Parameters.AddWithValue("@DriverName", model.DriverName);
                CommandObj.Parameters.AddWithValue("@TransportationCost", model.TransportationCost);
                CommandObj.Parameters.AddWithValue("@VehicleNo", model.VehicleNo);
                //CommandObj.Parameters.Add("@InventoryMasterIdRE", SqlDbType.Int);
                //CommandObj.Parameters["@InventoryMasterIdRE"].Direction = ParameterDirection.Output;
                CommandObj.Parameters.Add("@InventoryMasterId", SqlDbType.Int);
                CommandObj.Parameters["@InventoryMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
               // int inventoryMasterIdRe = Convert.ToInt32(CommandObj.Parameters["@InventoryMasterIdRE"].Value);  
                int inventoryMasterId = Convert.ToInt32(CommandObj.Parameters["@InventoryMasterId"].Value);
                int od = SaveTransferDetails(transactionModels,inventoryMasterId);
                if (od > 0)
                {
                    sqlTransaction.Commit();
                }
                return od;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Transfer Info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        private int SaveTransferDetails(List<TransactionModel> transactionModels, int inventoryMasterId)
        {
            int i = 0;
            foreach (var order in transactionModels)
            {
                CommandObj.CommandText = "spSaveTransferDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ProductId", order.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", order.Quantity);
                CommandObj.Parameters.AddWithValue("@InventoryMasterId", inventoryMasterId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public int Save(Product aProduct)
        {
            try
            {
                CommandObj.CommandText = "spAddNewProduct";
                throw new NotImplementedException();
            }
            catch (Exception exception)
            {
                throw  new Exception("Could not Save product info",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public ProductDetails GetProductDetailsByProductId(int productId)
        {
            try
            {
                CommandObj.CommandText = "spGetProductDetailsByProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                ProductDetails product = new ProductDetails();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product.UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]);
                    product.UnitPrice = Convert.ToDecimal(reader["UnitPrice"]);
                    product.DealerPrice = Convert.ToDecimal(reader["DealerPrice"]);
                }

                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get product details", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public Product GetProductByProductId(int productId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetProductByProductId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductId", productId);
                ConnectionObj.Open();
                Product product = null;
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = productId,
                        ProductTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        ProductCategory = new ProductCategory
                        {
                            ProductCategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ProductCategoryName = reader["ProductCategoryName"].ToString()
                        },
                        ProductType = new ProductType
                        {
                          ProductTypeId  = Convert.ToInt32(reader["ProductTypeId"]),
                          ProductTypeName = reader["ProductTypeName"].ToString()
                        }
                      
                    };

                }

                reader.Close();
                return product;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get product details", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int GetMaxProductionNoteNoByYear(int year)
        {
            try
            {
                int maxProductionNoteNo = 0;
                CommandObj.CommandText = "UDSP_GetMaxProductionNoteNoByYear";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@Year", year);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    maxProductionNoteNo = Convert.ToInt32(reader["MaxProductionNoteNo"]);
                }
                reader.Close();
                return maxProductionNoteNo;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get max production note no by year", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int SaveProductionNote(ProductionNote productionNote)
        {
            try
            {
               
                CommandObj.CommandText = "UDSP_SaveProductionNote";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ProductionNoteNo", productionNote.ProductionNoteNo);
                CommandObj.Parameters.AddWithValue("@ProductionNoteRef", productionNote.ProductionNoteRef);
                CommandObj.Parameters.AddWithValue("@ProductionNoteDate", productionNote.ProductionNoteDate);
                CommandObj.Parameters.AddWithValue("@ProductionNoteByUserId", productionNote.ProductionNoteByUserId);
                CommandObj.Parameters.AddWithValue("@ProductId", productionNote.ProductId);
                CommandObj.Parameters.AddWithValue("@Quantity", productionNote.Quantity);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not save production note", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<ViewProductionNoteModel> PendingProductionNote()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetAllPendingProducitonNote";
                CommandObj.CommandType = CommandType.StoredProcedure;
                List<ViewProductionNoteModel> production=new List<ViewProductionNoteModel>();
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    production.Add(new ViewProductionNoteModel
                    {
                        Id = Convert.ToInt32(reader["ProductionNoteId"]),
                        EntryStatus = reader["EntryStatus"].ToString(),
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductionNoteNo = reader["ProductionNoteNo"].ToString(),
                        ProductionNoteRef = reader["ProductionNoteRef"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ProductionNoteDate = Convert.ToDateTime(reader["ProductionNoteDate"]),
                        Product = new Product
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            SubSubSubAccountCode = reader["SubSubSubAccountCode"].ToString(),
                            CategoryId = Convert.ToInt32(reader["CategoryId"]),
                            ProductTypeId = Convert.ToInt32(reader["ProductTypeId"])
                        }
                    });
                }
                reader.Close();
                return production;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending production notes", exception);
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