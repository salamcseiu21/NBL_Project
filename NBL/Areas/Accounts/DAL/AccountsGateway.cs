﻿
using System;
using System.Collections.Generic;
using System.Linq;
using NBL.Areas.Accounts.Models;
using System.Data.SqlClient;
using System.Data;
using NBL.Areas.Accounts.DAL.Contracts;
using NBL.DAL;
using NBL.Models;

namespace NBL.Areas.Accounts.DAL
{
    public class AccountsGateway : DbGateway,IAccountGateway
    {
        public int SaveReceivable(Receivable receivable)
        {

            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {

                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveReceivable";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ClientId", receivable.ClientId);
                CommandObj.Parameters.AddWithValue("@ReceivableDateTime", receivable.ReceivableDateTime);
                CommandObj.Parameters.AddWithValue("@UserId", receivable.UserId);
                CommandObj.Parameters.AddWithValue("@ReceivableNo", receivable.ReceivableNo);
                CommandObj.Parameters.AddWithValue("@ReceivableRef", receivable.ReceivableRef);
                CommandObj.Parameters.AddWithValue("@InvoiceRef", receivable.InvoiceRef);
                CommandObj.Parameters.AddWithValue("@BranchId", receivable.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", receivable.CompanyId);
                CommandObj.Parameters.AddWithValue("@TransactionTypeId", receivable.TransactionTypeId);
                CommandObj.Parameters.AddWithValue("@Remarks", receivable.Remarks);
                CommandObj.Parameters.Add("@ReceivableId", SqlDbType.Int);
                CommandObj.Parameters["@ReceivableId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int receivableId = Convert.ToInt32(CommandObj.Parameters["@ReceivableId"].Value);
                int rowAffected = SaveReceivableDetails(receivable.Payments, receivableId);
                int result = 0;
                if (rowAffected > 0)
                {
                  result=SaveChequeDetails(receivable.Payments, receivableId);
                }
                if(result>0)
                {
                    sqlTransaction.Commit();
                }
                return result;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save receivable informaiton", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            } 
        }

        public ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId)
        {
            try
            {
                ChequeDetails details = new ChequeDetails();
                CommandObj.CommandText = "spGetReceivableChequeByDetailsId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@ChequeDetailsId", chequeDetailsId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                     details = new ChequeDetails
                    {
                        ChequeDetailsId = Convert.ToInt32(reader["ChequeDetailsId"]),
                        ReceivableId = Convert.ToInt32(reader["ReceivableId"]),
                        SourceBankName = reader["SourceBankName"].ToString(),
                        BankAccountNo = reader["BankAccountNo"].ToString(),
                        ChequeNo = reader["ChequeNo"].ToString(),
                        ChequeDate = Convert.ToDateTime(reader["ChequeDate"]),
                        ChequeAmount = Convert.ToDecimal(reader["ChequeAmount"]),
                        PaymentTypeId = Convert.ToInt32(reader["PaymentTypeId"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ReceivableRef = reader["ReceivableRef"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"])
                    };
                }
                reader.Close();
                return details;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect receivable cheques", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId)
        {
            try
            {
                var details = new List<VoucherDetails>();
                CommandObj.CommandText = "UDSP_GetVoucherDetailsByVoucherId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", voucherId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var voucher = new VoucherDetails
                    {
                        VoucherDetailsId=Convert.ToInt32(reader["VoucherDetailsId"]),
                        VoucherByUserId=Convert.ToInt32(reader["UserId"]),
                        VoucherDate=Convert.ToDateTime(reader["VoucherDate"]),
                        VoucherNo=Convert.ToInt32(reader["VoucherNo"]),
                        VoucherRef=reader["VoucherRef"].ToString(),
                        VoucherType=Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        BranchId=Convert.ToInt32(reader["BranchId"]),
                        CompanyId=Convert.ToInt32(reader["CompanyId"]),
                        AccountCode=Convert.ToString(reader["AccountCode"]),
                        Amounts=Convert.ToDecimal(reader["Amounts"]),
                        DebitOrCredit=reader["DebitOrCredit"].ToString(),
                        TransactionTypeId=Convert.ToInt32(reader["TransactionTypeId"]),
                        SysDateTime=Convert.ToDateTime(reader["SysDateTime"]),
                    };
                    details.Add(voucher);

                }
                reader.Close();
                return details;
            }
            catch(Exception exception)
            {
                throw new Exception("Could not collected voucher details by Voucher id",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }



        public int GetMaxVoucherNoOfCurrentYearByVoucherType(int voucherType)
        {
            try
            {
                CommandObj.CommandText = "spGetMaxVoucherNoOfCurrentYearByVoucherType";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherType", voucherType);
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
                throw new Exception("Could not collect max serial no of credit voucher of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                List<ChequeDetails> details = new List<ChequeDetails>();
                CommandObj.CommandText = "spGetAllReceivableChequeByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    ChequeDetails aPayment = new ChequeDetails
                    {
                        ChequeDetailsId = Convert.ToInt32(reader["ChequeDetailsId"]),
                        ReceivableId = Convert.ToInt32(reader["ReceivableId"]),
                        SourceBankName = reader["SourceBankName"].ToString(),
                        BankAccountNo = reader["BankAccountNo"].ToString(),
                        ChequeNo = reader["ChequeNo"].ToString(),
                        ChequeDate = Convert.ToDateTime(reader["ChequeDate"]),
                        ChequeAmount = Convert.ToDecimal(reader["ChequeAmount"]),
                        PaymentTypeId = Convert.ToInt32(reader["PaymentTypeId"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        ReceivableRef = reader["ReceivableRef"].ToString(),
                        ClientId = Convert.ToInt32(reader["ClientId"])
                    };
                    details.Add(aPayment);
                }
                reader.Close();
                return details;

            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect receivable cheques", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveReceivableDetails(List<Payment> payments, int receivableId)
        {
            int i = 0;
            CommandObj.CommandText = "spSaveReceivableDetails";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@ReceivableId", receivableId);
            CommandObj.Parameters.AddWithValue("@Amount", payments.Sum(n=>n.ChequeAmount));
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return i;
        }

        private int SaveChequeDetails(IEnumerable<Payment> payments, int receivableId)
        {
            int i = 0;
            foreach (var item in payments) 
            {
                CommandObj.CommandText = "spSaveChequeDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@ReceivableId", receivableId);
                CommandObj.Parameters.AddWithValue("@SourceBankName", item.SourceBankName);
                CommandObj.Parameters.AddWithValue("@BankAccountNo", item.BankAccountNo);
                CommandObj.Parameters.AddWithValue("@ChequeDate", item.ChequeDate);
                CommandObj.Parameters.AddWithValue("@ChequeNo", item.ChequeNo);
                CommandObj.Parameters.AddWithValue("@ChequeAmount", item.ChequeAmount);
                CommandObj.Parameters.AddWithValue("@PaymentTypeId", item.PaymentTypeId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public int GetMaxReceivableSerialNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "spGetMaxRecivableNoOfCurrentYear";
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
                throw new Exception("Could not collect max serial no of receivable of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }



        public int ActiveReceivableCheque(ChequeDetails chequeDetails, Receivable aReceivable, Client aClient)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int rowAffected = 0;
                int accountAffected = 0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "spSaveActiveReceivable";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@TransactionDate", aReceivable.ReceivableDateTime);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aReceivable.TransactionRef);
                CommandObj.Parameters.AddWithValue("@UserId", aReceivable.UserId);
                CommandObj.Parameters.AddWithValue("@BranchId", aReceivable.BranchId);
                CommandObj.Parameters.AddWithValue("@Paymode",aReceivable.Paymode);
                CommandObj.Parameters.AddWithValue("@CompanyId", aReceivable.CompanyId);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aReceivable.VoucherNo);
                CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.Int);
                CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int accountMasterId = Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value);
                rowAffected = ActiveCheque(chequeDetails.ChequeDetailsId);
                if (rowAffected > 0)
                {
                    string subSubSubAccountCode = aReceivable.SubSubSubAccountCode;
                    for (int i = 1; i <= 2; i++)
                    {
                        if (i == 1)
                        {
                            aReceivable.TransactionType = "Cr";
                            aReceivable.SubSubSubAccountCode = aClient.SubSubSubAccountCode;
                            aReceivable.Amount = chequeDetails.ChequeAmount * (-1);
                           
                        }
                        else
                        {
                            aReceivable.TransactionType = "Dr";
                            aReceivable.SubSubSubAccountCode = subSubSubAccountCode;
                            aReceivable.Amount = chequeDetails.ChequeAmount;
                        }
                        accountAffected += SaveReceivableDetailsToAccountsDetails(aReceivable, accountMasterId,chequeDetails.ChequeDetailsId);
                    }
                }
                if (accountAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return accountAffected;

            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not Save Invoiced order info", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        private int SaveReceivableDetailsToAccountsDetails(Receivable aReceivable, int accountMasterId,int chequeDetailsId)
        {

            int i;
            CommandObj.CommandText = "spSaveInvoiceDetailsToAccountsDetails";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@AccountMasterId", accountMasterId);
            CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", aReceivable.SubSubSubAccountCode);
            CommandObj.Parameters.AddWithValue("@TransactionType",  aReceivable.TransactionType);
            CommandObj.Parameters.AddWithValue("@Amount", aReceivable.Amount);
            CommandObj.Parameters.AddWithValue("@Explanation", aReceivable.Remarks);
            CommandObj.Parameters.AddWithValue("@ChequeDetailsId", chequeDetailsId);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return i;


        }

        public int ActiveCheque(int chequeDetailsId)
        {
            CommandObj.CommandText = "spUpdateChequeActivationStatus";
            CommandObj.CommandType = CommandType.StoredProcedure;
            CommandObj.Parameters.Clear();
            CommandObj.Parameters.AddWithValue("@ChequeDetailsId", chequeDetailsId);
            CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
            CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
            CommandObj.ExecuteNonQuery();
            int i = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            return i;
        }


        //--------Vouchers-------
        public int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int rowAffected = 0;
                int journalId = 0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveJournalVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@UserId", aJournal.VoucherByUserId);
                CommandObj.Parameters.AddWithValue("@BranchId", aJournal.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aJournal.CompanyId);
                CommandObj.Parameters.AddWithValue("@VoucherDate", aJournal.VoucherDate);
                CommandObj.Parameters.AddWithValue("@Amount", journals.ToList().FindAll(n=>n.DebitOrCredit.Equals("Dr")).Sum(n=>n.Amounts));
                CommandObj.Parameters.AddWithValue("@VoucherRef", aJournal.VoucherRef);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aJournal.VoucherNo);
                CommandObj.Parameters.Add("@JournalId", SqlDbType.Int);
                CommandObj.Parameters["@JournalId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                journalId = Convert.ToInt32(CommandObj.Parameters["@JournalId"].Value);
                rowAffected = SaveJournalVoucherDetails(journals, journalId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch(Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not save journal voucher information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveJournalVoucherDetails(List<JournalVoucher> journals, int journalId)
        {
            int i = 0;
            foreach (var item in journals)
            {
                if (item.DebitOrCredit.Equals("Cr"))
                {
                    item.Amounts = item.Amounts * -1;
                }
                CommandObj.CommandText = "UDSP_SaveJournalVoucherDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@JournalId", journalId);
                CommandObj.Parameters.AddWithValue("@PurposeName", item.PurposeName);
                CommandObj.Parameters.AddWithValue("@PurposeCode", item.PurposeCode);
                CommandObj.Parameters.AddWithValue("@Amount", item.Amounts);
                CommandObj.Parameters.AddWithValue("@Remarks", item.Remarks);
                CommandObj.Parameters.AddWithValue("@DebitOrCredit", item.DebitOrCredit);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                List<JournalVoucher> journals = new List<JournalVoucher>();
                CommandObj.CommandText = "UDSP_GetAllJournalVouchersByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var journal = new JournalVoucher
                    {
                        JournalId = Convert.ToInt32(reader["JournalId"]),
                        Amounts = Convert.ToDecimal(reader["Amount"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        BranchId = branchId,
                        CompanyId = companyId,
                        Status = Convert.ToInt32(reader["Status"])
                    };
                    journals.Add(journal);
                }
                reader.Close();
                return journals;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect journal informaiton by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId)
        {
            try
            {
                List<JournalVoucher> journals = new List<JournalVoucher>();
                CommandObj.CommandText = "UDSP_GetAllJournalVouchersByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var journal = new JournalVoucher
                    {
                        JournalId = Convert.ToInt32(reader["JournalId"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        CompanyId = companyId,
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Amounts =Convert.ToDecimal(reader["Amount"]),
                        Status = Convert.ToInt32(reader["Status"])
                    };
                    journals.Add(journal);
                }
                reader.Close();
                return journals;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect journal informaiton by Company Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int SaveVoucher(Voucher voucher) 
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_SaveVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherDate", voucher.VoucherDate);
                CommandObj.Parameters.AddWithValue("@UserId", voucher.VoucherByUserId);
                CommandObj.Parameters.AddWithValue("@BranchId", voucher.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", voucher.CompanyId);
                CommandObj.Parameters.AddWithValue("@VoucherNo", voucher.VoucherNo);
                CommandObj.Parameters.AddWithValue("@VoucherRef", voucher.VoucherRef);
                CommandObj.Parameters.AddWithValue("@TransactionTypeId", voucher.TransactionTypeId);
                CommandObj.Parameters.AddWithValue("@VoucherType", voucher.VoucherType);
                CommandObj.Parameters.AddWithValue("@VoucherName", voucher.VoucherName);
                CommandObj.Parameters.AddWithValue("@Amounts", voucher.Amounts);
                CommandObj.Parameters.Add("@VoucherId", SqlDbType.Int);
                CommandObj.Parameters["@VoucherId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int voucherId = Convert.ToInt32(CommandObj.Parameters["@VoucherId"].Value);
                int rowAffected = SaveVoucherDetails(voucher.PurposeList, voucherId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not save voucher information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveVoucherDetails(List<Purpose> purposeList, int voucherId)
        {
            int i = 0;
            foreach (var purpose in purposeList) 
            {
                CommandObj.CommandText = "UDSP_SaveVoucherDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@VoucherId", voucherId);
                CommandObj.Parameters.AddWithValue("@AccountCode", purpose.PurposeCode);
                CommandObj.Parameters.AddWithValue("@DebitOrCredit", purpose.DebitOrCredit);
                CommandObj.Parameters.AddWithValue("@Amounts", purpose.Amounts);
                CommandObj.Parameters.AddWithValue("@Remarks", purpose.Remarks?? "None....");
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }


        public IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType)
        {
            try
            {
                List<Voucher> vouchers = new List<Voucher>();
                CommandObj.CommandText = "UDSP_GetAllVouchersByBranchCompanyIdAndVoucherType";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                CommandObj.Parameters.AddWithValue("@VoucherType", voucherType);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var voucher = new Voucher
                    {
                        VoucherId = Convert.ToInt32(reader["VoucherId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef=reader["VoucherRef"].ToString(),
                        VoucherType=Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        VoucherNo=Convert.ToInt32(reader["VoucherNo"]),
                        TransactionTypeId=Convert.ToInt32(reader["TransactionTypeId"])
                    };
                    vouchers.Add(voucher);
                }
                reader.Close();
                return vouchers;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect voucher informaiton by branch and Company Id and voucher type", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Voucher> GetVoucherList()
        {
            try
            {
                List<Voucher> vouchers = new List<Voucher>();
                CommandObj.CommandText = "UDSP_GetVoucherList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var voucher = new Voucher
                    {
                        VoucherId = Convert.ToInt32(reader["VoucherId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherType = Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                        CompanyId=Convert.ToInt32(reader["CompanyId"]),
                        BranchId=Convert.ToInt32(reader["BranchId"]),
                        Status=Convert.ToInt32(reader["Status"]),
                        Cancel = reader["Cancel"].ToString()
                    };
                    vouchers.Add(voucher);
                }
                reader.Close();
                return vouchers;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect voucher lsit", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Voucher> GetPendingVoucherList()
        {
            try
            {
                List<Voucher> vouchers = new List<Voucher>();
                CommandObj.CommandText = "UDSP_GetPendingVoucherList";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var voucher = new Voucher
                    {
                        VoucherId = Convert.ToInt32(reader["VoucherId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherType = Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        Cancel = reader["Cancel"].ToString()
                    };
                    vouchers.Add(voucher);
                }
                reader.Close();
                return vouchers;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending voucher lsit ", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                List<Voucher> vouchers = new List<Voucher>();
                CommandObj.CommandText = "UDSP_GetVoucherListByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var voucher = new Voucher
                    {
                        VoucherId = Convert.ToInt32(reader["VoucherId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherType = Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                        CompanyId = companyId,
                        BranchId = companyId,
                        Status = Convert.ToInt32(reader["Status"]),
                        Cancel = reader["Cancel"].ToString()
                    };
                    vouchers.Add(voucher);
                }
                reader.Close();
                return vouchers;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect voucher lsit ", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId)
        {
            try
            {
                List<Voucher> vouchers = new List<Voucher>();
                CommandObj.CommandText = "UDSP_GetVoucherListByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var voucher = new Voucher
                    {
                        VoucherId = Convert.ToInt32(reader["VoucherId"]),
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherType = Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                        CompanyId = companyId,
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        Status = Convert.ToInt32(reader["Status"]),
                        Cancel = reader["Cancel"].ToString()
                    };
                    vouchers.Add(voucher);
                }
                reader.Close();
                return vouchers;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect voucher lsit ", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
        public Voucher GetVoucheByVoucherId(int voucherId)
        {
            try
            {
                Voucher voucher =new Voucher();
                CommandObj.CommandText = "UDSP_GetVoucheByVoucherId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", voucherId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                    voucher=new Voucher
                    {
                        VoucherId = voucherId,
                        Amounts = Convert.ToDecimal(reader["Amounts"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherType = Convert.ToInt32(reader["VoucherType"]),
                        VoucherName = reader["VoucherName"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        TransactionTypeId = Convert.ToInt32(reader["TransactionTypeId"]),
                        Status=Convert.ToInt32(reader["Status"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    };
                    
                }
                reader.Close();
                return voucher;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect voucher informaiton  voucher id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int CancelVoucher(int voucherId, string reason, int userId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_CancelVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", voucherId);
                CommandObj.Parameters.AddWithValue("@Reason", reason);
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not cancel the voucher",exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails, int userId)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int rowAffected=0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_ApproveVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", aVoucher.VoucherId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aVoucher.VoucherRef);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aVoucher.VoucherNo);
                CommandObj.Parameters.AddWithValue("@BranchId", aVoucher.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aVoucher.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.Int);
                CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int accountId = Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value); 
                rowAffected = SaveVoucherDetailsIntoAccountDetails(voucherDetails, accountId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not approve the voucher", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveVoucherDetailsIntoAccountDetails(List<VoucherDetails> voucherDetails, int accountId)
        {
            var i = 0;
            foreach (var detail in voucherDetails)
            {
                if (detail.DebitOrCredit.Equals("Cr"))
                {
                    detail.Amounts = detail.Amounts *-1;
                }
                CommandObj.CommandText = "UDSP_SaveVoucherDetailsIntoAccountDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@AccountMasterId", accountId);
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", detail.AccountCode);
                CommandObj.Parameters.AddWithValue("@TransactionType", detail.DebitOrCredit);
                CommandObj.Parameters.AddWithValue("@Amount", detail.Amounts);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public int UpdateVoucher(Voucher voucher,List<VoucherDetails> voucherDetails) 
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_UpdateVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", voucher.VoucherId);
                CommandObj.Parameters.AddWithValue("@UpdatedByUserId", voucher.UpdatedByUserId);
                CommandObj.Parameters.AddWithValue("@Amounts", voucher.Amounts);
                CommandObj.ExecuteNonQuery();
                int rowAffected = UpdateVoucherDetails(voucherDetails);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not update voucher information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int UpdateVoucherDetails(List<VoucherDetails> voucherDetails) 
        {
            int i = 0;
            foreach (var detail in voucherDetails)
            {
                CommandObj.CommandText = "UDSP_UpdateVoucherDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@VoucherDetailId", detail.VoucherDetailsId);
                CommandObj.Parameters.AddWithValue("@Amounts", detail.Amounts);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public int GetMaxJournalVoucherNoOfCurrentYear()
        {
            try
            {
                CommandObj.CommandText = "UDSP_GetMaxJournalVoucherNoOfCurrentYear";
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
                throw new Exception("Could not collect max serial no of journal voucher of current Year", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId)
        {
            try
            {
                List<JournalDetails> journals = new List<JournalDetails>();
                CommandObj.CommandText = "UDSP_GetJournalVoucherDetailsById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@JournalId", journalVoucherId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var journal = new JournalDetails
                    {
                        JournalId = Convert.ToInt32(reader["JournalId"]),
                        AccountCode = reader["PurposeCode"].ToString(),
                        Remarks = reader["Remarks"].ToString(),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        DebitOrCredit = reader["DebitOrCredit"].ToString(),
                        JournalDetailsId = Convert.ToInt32(reader["JournalDetailsId"])
                        
                        
                    };
                    journals.Add(journal);
                }
                reader.Close();
                return journals;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect journal details by journal Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public JournalVoucher GetJournalVoucherById(int journalId)
        {
            try
            {
                JournalVoucher journal = new JournalVoucher();
                CommandObj.CommandText = "UDSP_GetJournalVoucherById";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@JournalId", journalId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if(reader.Read())
                {
                     journal = new JournalVoucher
                    {
                        JournalId = journalId,
                        Amounts = Convert.ToDecimal(reader["Amount"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        BranchId = Convert.ToInt32(reader["BranchId"]),
                        CompanyId = Convert.ToInt32(reader["CompanyId"])
                    };
                   
                }
                reader.Close();
                return journal;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect journal voucher by Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }

        public int CancelJournalVoucher(int voucherId, string reason, int userId)
        {
            try
            {
                CommandObj.CommandText = "UDSP_CancelJournalVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", voucherId);
                CommandObj.Parameters.AddWithValue("@Reason", reason);
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not cancel journal voucher", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        public int UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_UpdateJournalVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", voucher.JournalId);
                CommandObj.Parameters.AddWithValue("@UpdatedByUserId", voucher.UpdatedByUserId);
                CommandObj.Parameters.AddWithValue("@Amount", voucher.Amounts);
                CommandObj.ExecuteNonQuery();
                int rowAffected = UpdateJournalVoucherDetails(journalVouchers);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                sqlTransaction.Rollback();
                throw new Exception("Could not update journal voucher information", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }


        private int UpdateJournalVoucherDetails(List<JournalDetails> journalVouchers) 
        {
            int i = 0;
            foreach (var detail in journalVouchers)
            {
                CommandObj.CommandText = "UDSP_UpdateJournalVoucherDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@JournalDetailsId", detail.JournalDetailsId);
                CommandObj.Parameters.AddWithValue("@Amount", detail.Amount);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }

            return i;
        }

        public List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                List<JournalVoucher> journals = new List<JournalVoucher>();
                CommandObj.CommandText = "UDSP_GetAllPendingJournalVoucherByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                while (reader.Read())
                {
                    var journal = new JournalVoucher
                    {
                        JournalId = Convert.ToInt32(reader["JournalId"]),
                        Amounts = Convert.ToDecimal(reader["Amount"]),
                        VoucherByUserId = Convert.ToInt32(reader["UserId"]),
                        VoucherDate = Convert.ToDateTime(reader["VoucherDate"]),
                        SysDateTime = Convert.ToDateTime(reader["SysDateTime"]),
                        VoucherRef = reader["VoucherRef"].ToString(),
                        VoucherNo = Convert.ToInt32(reader["VoucherNo"]),
                        BranchId = branchId,
                        CompanyId = companyId,
                        Status =Convert.ToInt32(reader["Status"])
                    };
                    journals.Add(journal);
                }
                reader.Close();
                return journals;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect pending journal Vouchers by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Parameters.Clear();
                CommandObj.Dispose();
                ConnectionObj.Close();
            }
        }
     
        public int ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId)
        {
            ConnectionObj.Open();
            SqlTransaction sqlTransaction = ConnectionObj.BeginTransaction();
            try
            {
                int rowAffected = 0;
                CommandObj.Transaction = sqlTransaction;
                CommandObj.CommandText = "UDSP_ApproveJournalVoucher";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VoucherId", aVoucher.JournalId);
                CommandObj.Parameters.AddWithValue("@TransactionRef", aVoucher.VoucherRef);
                CommandObj.Parameters.AddWithValue("@VoucherNo", aVoucher.VoucherNo);
                CommandObj.Parameters.AddWithValue("@BranchId", aVoucher.BranchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", aVoucher.CompanyId);
                CommandObj.Parameters.AddWithValue("@UserId", userId);
                CommandObj.Parameters.Add("@AccountMasterId", SqlDbType.Int);
                CommandObj.Parameters["@AccountMasterId"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                int accountId = Convert.ToInt32(CommandObj.Parameters["@AccountMasterId"].Value);
                rowAffected = SaveJournalVoucherDetailsIntoAccountDetails(voucherDetails, accountId);
                if (rowAffected > 0)
                {
                    sqlTransaction.Commit();
                }
                return rowAffected;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not approve journal voucher", exception);
            }
            finally
            {
                ConnectionObj.Close();
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
            }
        }

        private int SaveJournalVoucherDetailsIntoAccountDetails(List<JournalDetails> voucherDetails, int accountId)
        {
            var i = 0;
            foreach (var detail in voucherDetails)
            {
                if (detail.DebitOrCredit.Equals("Cr"))
                {
                    detail.Amount = detail.Amount * -1;
                }
                CommandObj.CommandText = "UDSP_SaveJournalVoucherDetailsIntoAccountDetails";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.Clear();
                CommandObj.Parameters.AddWithValue("@AccountMasterId", accountId);
                CommandObj.Parameters.AddWithValue("@SubSubSubAccountCode", detail.AccountCode);
                CommandObj.Parameters.AddWithValue("@TransactionType", detail.DebitOrCredit);
                CommandObj.Parameters.AddWithValue("@Amount", detail.Amount);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                CommandObj.ExecuteNonQuery();
                i += Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
            }
            return i;
        }

        public int ApproveVat(Vat vat)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveVat";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@VatId", vat.VatId);
                CommandObj.Parameters.AddWithValue("@ProductId", vat.ProductId);
                CommandObj.Parameters.AddWithValue("@ApproveByUserId", vat.ApprovedByUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Unable to approve vat info",exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public int ApproveDiscount(Discount discount)
        {
            try
            {
                CommandObj.CommandText = "UDSP_ApproveDiscount";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@DiscountId", discount.DiscountId);
                CommandObj.Parameters.AddWithValue("@ProductId", discount.ProductId);
                CommandObj.Parameters.AddWithValue("@ClientTypeId", discount.ClientTypeId);
                CommandObj.Parameters.AddWithValue("@ApproveByUserId", discount.ApprovedByUserId);
                CommandObj.Parameters.Add("@RowAffected", SqlDbType.Int);
                CommandObj.Parameters["@RowAffected"].Direction = ParameterDirection.Output;
                ConnectionObj.Open();
                CommandObj.ExecuteNonQuery();
                int rowAffected = Convert.ToInt32(CommandObj.Parameters["@RowAffected"].Value);
                return rowAffected;

            }
            catch (Exception exception)
            {
                throw new Exception("Unable to approve Discount info", exception);
            }
            finally
            {
                CommandObj.Dispose();
                ConnectionObj.Close();
                CommandObj.Parameters.Clear();
            }
        }

        public decimal GetTotalSaleValueOfCurrentMonth()
        {
            try
            {
                decimal totalSaleValue = 0;
                CommandObj.CommandText = "UDSP_GetTotalSaleValueofCurrentMonth";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    totalSaleValue = Convert.ToDecimal(reader["TotalSaleValue"]);
                }
                reader.Close();
                return totalSaleValue;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Sale value of Current Month",exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public decimal GetTotalSaleValueOfCurrentMonthByCompanyId(int companyId)
        {
            try
            {
                decimal totalSaleValue = 0;
                CommandObj.CommandText = "UDSP_GetTotalSaleValueofCurrentMonthByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    totalSaleValue = Convert.ToDecimal(reader["TotalSaleValue"]);
                }
                reader.Close();
                return totalSaleValue;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Sale value of Current Month by Company Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }

        }
        public decimal GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                decimal totalSaleValue = 0;
                CommandObj.CommandText = "UDSP_GetTotalSaleValueofCurrentMonthByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    totalSaleValue = Convert.ToDecimal(reader["TotalSaleValue"]);
                }
                reader.Close();
                return totalSaleValue;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Sale value of Current Month by branch and Company Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }


        public decimal GetTotalCollectionOfCurrentMonth()
        {
            try
            {
                decimal totalCollection = 0;
                CommandObj.CommandText = "UDSP_GetTotalCollectionOfCurrentMonth";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    totalCollection = Convert.ToDecimal(reader["TotalCollection"]);
                }
                reader.Close();
                return totalCollection;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Collection  of Current Month", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public decimal GetTotalCollectionOfCurrentMonthByCompanyId(int companyId)
        {
            try
            {
                decimal totalCollection = 0;
                CommandObj.CommandText = "UDSP_GetTotalCollectionOfCurrentMonthByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    totalCollection = Convert.ToDecimal(reader["TotalCollection"]);
                }
                reader.Close();
                return totalCollection;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Collection  of Current Month by Branch Id", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public decimal GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId)
        {
            try
            {
                decimal totalCollection = 0;
                CommandObj.CommandText = "UDSP_GetTotalCollectionOfCurrentMonthByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    totalCollection = Convert.ToDecimal(reader["TotalCollection"]);
                }
                reader.Close();
                return totalCollection;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Collection  of Current Month", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public decimal GetTotalOrderedAmountOfCurrentMonth()
        {
            try
            {
                decimal orderedAmount = 0;
                CommandObj.CommandText = "UDSP_GetTotalOrderedAmountOfCurrentMonth";
                CommandObj.CommandType = CommandType.StoredProcedure;
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    orderedAmount = Convert.ToDecimal(reader["OrderedAmount"]); 
                }
                reader.Close();
                return orderedAmount;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Ordered Amount  of Current Month", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public decimal GetTotalOrderedAmountOfCurrentMonthByCompanyId(int companyId)
        {
            try
            {
                decimal orderedAmount = 0;
                CommandObj.CommandText = "UDSP_GetTotalOrderedAmountOfCurrentMonthByCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    orderedAmount = Convert.ToDecimal(reader["OrderedAmount"]);
                }
                reader.Close();
                return orderedAmount;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Ordered Amount  of Current Month", exception);
            }
            finally
            {
                CommandObj.Dispose();
                CommandObj.Parameters.Clear();
                ConnectionObj.Close();
            }
        }
        public decimal GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(int branchId, int companyId)
        {
            try
            {
                decimal orderedAmount = 0;
                CommandObj.CommandText = "UDSP_GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId";
                CommandObj.CommandType = CommandType.StoredProcedure;
                CommandObj.Parameters.AddWithValue("@BranchId", branchId);
                CommandObj.Parameters.AddWithValue("@CompanyId", companyId);
                ConnectionObj.Open();
                SqlDataReader reader = CommandObj.ExecuteReader();
                if (reader.Read())
                {
                    orderedAmount = Convert.ToDecimal(reader["OrderedAmount"]);
                }
                reader.Close();
                return orderedAmount;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not collect Total Ordered Amount  of Current Month", exception);
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