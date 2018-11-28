using System;
using NBL.Areas.Accounts.Models;
using NBL.Areas.Accounts.DAL;
using System.Collections.Generic;
using NblClassLibrary.Models;
using NBL.Areas.Admin.DAL;

namespace NBL.Areas.Accounts.BLL
{
    public class AccountsManager
    {
        readonly AccountsGateway _accountsGateway = new AccountsGateway();
        readonly InvoiceGateway _invoiceGateway = new InvoiceGateway();
        internal int SaveReceivable(Receivable receivable)
        {
            int maxSl = GetMaxReceivableSerialNoOfCurrentYear();
            receivable.ReceivableRef = GenerateReceivableRef(maxSl);
            receivable.ReceivableNo = GenerateReceivableNo(maxSl);
            return _accountsGateway.SaveReceivable(receivable);
        }

        private int GenerateReceivableNo(int maxSl)
        {
            int receivableNo = maxSl + 1;
            return receivableNo;
        }

        internal IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId)
        {
            return _accountsGateway.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId);
        }

        public ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId)
        {
            return _accountsGateway.GetReceivableChequeByDetailsId(chequeDetailsId);
        }

        private string GenerateReceivableRef(int maxSl)
        {
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + "AR" + (maxSl + 1);
            return reference;
        }

        private int GetMaxReceivableSerialNoOfCurrentYear()
        {
          return _accountsGateway.GetMaxReceivableSerialNoOfCurrentYear();
        }

        internal bool ActiveReceivableCheque(ChequeDetails chequeDetails,Receivable aReceivable, Client aClient) 
        {
            aReceivable.VoucherNo = GetMaxVoucherNoByTransactionInfix("AR");
            int rowAffected= _accountsGateway.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
            return rowAffected > 0 ? true : false;
        }
        private int GetMaxVoucherNoByTransactionInfix(string infix)
        {
            int temp = _invoiceGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        internal int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals)
        {
            int maxSl = GetMaxJournalVoucherNoOfCurrentYear();
            aJournal.VoucherRef = DateTime.Now.Year.ToString().Substring(2, 2) + "JV" + (maxSl + 1);
            aJournal.VoucherNo = maxSl + 1;
            return _accountsGateway.SaveJournalVoucher(aJournal,journals);
        }

        private int GetMaxJournalVoucherNoOfCurrentYear()
        {
            return _accountsGateway.GetMaxJournalVoucherNoOfCurrentYear();
        }

        public IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId,int companyId)
        {
            return _accountsGateway.GetAllJournalVouchersByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId)
        {
            return _accountsGateway.GetAllJournalVouchersByCompanyId(companyId);
        }


        internal int SaveVoucher(Voucher voucher)
        {
            int maxSl = GetMaxVoucherNoOfCurrentYearByVoucherType(voucher.VoucherType);
            voucher.VoucherRef = GenerateVoucherRef(maxSl,voucher.VoucherType);
            voucher.VoucherNo = maxSl+1;
            return _accountsGateway.SaveVoucher(voucher);
        }
        private int GetMaxVoucherNoOfCurrentYearByVoucherType(int voucherType)
        {
            return _accountsGateway.GetMaxVoucherNoOfCurrentYearByVoucherType(voucherType); 
        }

        private string GenerateVoucherRef(int maxSl,int voucherType)
        {
            string infix = "";
            switch (voucherType)
            {
                case 1:infix = "CrV";
                    break;
                case 2:
                    infix = "DrV";
                    break;
                case 3:
                    infix = "CPV";
                    break;
                case 4:
                    infix = "CRV";
                    break;
            }
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + infix + (maxSl + 1);
            return reference;
        }

        internal IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType) 
        {
           return _accountsGateway.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, voucherType);
        }


        internal Voucher GetVoucherByVoucherId(int voucherId)
        {
            return _accountsGateway.GetVoucheByVoucherId(voucherId);
        }

        public IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId)
        {
            return _accountsGateway.GetVoucherDetailsByVoucherId(voucherId);
        }

        internal IEnumerable<Voucher> GetVoucherList()
        {
            return _accountsGateway.GetVoucherList();
        }

        internal IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId)
        {
            return _accountsGateway.GetVoucherListByCompanyId(companyId);
        }

        internal IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId, int companyId)
        {
            return _accountsGateway.GetVoucherListByBranchAndCompanyId(branchId,companyId);
        }
        internal IEnumerable<Voucher> GetPendingVoucherList()
        {
            return _accountsGateway.GetPendingVoucherList();
        }
        internal bool CancelVoucher(int voucherId, string reason, int userId)
        {
            int rowAffected=_accountsGateway.CancelVoucher(voucherId,reason,userId);
            return rowAffected>0;
        }

        public bool ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails,int userId)
        {
            int rowAffected = _accountsGateway.ApproveVoucher(aVoucher, voucherDetails,userId);
            return rowAffected > 0;
        }

        public bool UpdateVoucher(Voucher voucher,List<VoucherDetails> voucherDetails)
        {
            int rowAffected = _accountsGateway.UpdateVoucher(voucher,voucherDetails);
            return rowAffected > 0;
        }

        public List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId)
        {
            return _accountsGateway.GetJournalVoucherDetailsById(journalVoucherId);
        }

        public JournalVoucher GetJournalVoucherById(int journalId)
        {
            return _accountsGateway.GetJournalVoucherById(journalId);
        }

        public bool CancelJournalVoucher(int voucherId, string reason, int userId)
        {
            int rowAffected = _accountsGateway.CancelJournalVoucher(voucherId, reason, userId);
            return rowAffected > 0;
        }

        public bool UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers)
        {
            int rowAffected = _accountsGateway.UpdateJournalVoucher(voucher, journalVouchers);
            return rowAffected > 0;
        }

        public List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId)
        {
            return _accountsGateway.GetAllPendingJournalVoucherByBranchAndCompanyId(branchId, companyId);
        }

        public bool ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId)
        {
            int rowAffected = _accountsGateway.ApproveJournalVoucher(aVoucher, voucherDetails, userId);
            return rowAffected > 0;
        }

        public bool ApproveVat(Vat vat)
        {
            return _accountsGateway.ApproveVat(vat) > 0;
        }

        public bool ApproveDiscount(Discount discount)
        {
            return _accountsGateway.ApproveDiscount(discount) > 0;
        }

        public decimal GetTotalSaleValueOfCurrentMonth()
        {
            return _accountsGateway.GetTotalSaleValueOfCurrentMonth();
        }
        public decimal GetTotalSaleValueOfCurrentMonthByCompanyId(int companyId)
        {
            return _accountsGateway.GetTotalSaleValueOfCurrentMonthByCompanyId(companyId);
        }
        public decimal GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId)
        {
            return _accountsGateway.GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(branchId,companyId);
        }
        public decimal GetTotalCollectionOfCurrentMonth()
        {
            return _accountsGateway.GetTotalCollectionOfCurrentMonth();
        }
        public decimal GetTotalCollectionOfCurrentMonthByCompanyId(int companyId)
        {
            return _accountsGateway.GetTotalCollectionOfCurrentMonthByCompanyId(companyId);
        }
        public decimal GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId)
        {
            return _accountsGateway.GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(branchId,companyId);
        }
        public decimal GetTotalOrderedAmountOfCurrentMonth() 
        {
            return _accountsGateway.GetTotalOrderedAmountOfCurrentMonth();
        }
        public decimal GetTotalOrderedAmountOfCurrentMonthByCompanyId(int companyId)
        {
            return _accountsGateway.GetTotalOrderedAmountOfCurrentMonthByCompanyId(companyId);
        }
        public decimal GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId) 
        {
            return _accountsGateway.GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(branchId,companyId);
        }
    }
}