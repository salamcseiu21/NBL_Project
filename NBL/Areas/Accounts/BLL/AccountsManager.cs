﻿using System;
using NBL.Areas.Accounts.Models;
using NBL.Areas.Accounts.DAL;
using System.Collections.Generic;
using System.Linq;
using NBL.Areas.Accounts.BLL.Contracts;
using NBL.Areas.Admin.DAL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Areas.Accounts.DAL.Contracts;

namespace NBL.Areas.Accounts.BLL
{
    public class AccountsManager:IAccountsManager
    {
       private readonly IAccountGateway _iAccountGateway;
       private readonly InvoiceGateway _invoiceGateway = new InvoiceGateway();
       private readonly ICommonManager _iCommonManager;

        public AccountsManager(ICommonManager iCommonManager,IAccountGateway iAccountGateway)
        {
            _iCommonManager = iCommonManager;
            _iAccountGateway = iAccountGateway;
        }

        public int SaveReceivable(Receivable receivable)
        {
            int maxSl = GetMaxReceivableSerialNoOfCurrentYear();
            receivable.ReceivableRef = GenerateReceivableRef(maxSl);
            receivable.ReceivableNo = GenerateReceivableNo(maxSl);
            return _iAccountGateway.SaveReceivable(receivable);
        }

        private int GenerateReceivableNo(int maxSl)
        {
            int receivableNo = maxSl + 1;
            return receivableNo;
        }

        public IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetAllReceivableChequeByBranchAndCompanyId(branchId, companyId);
        }

        public ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId)
        {
            return _iAccountGateway.GetReceivableChequeByDetailsId(chequeDetailsId);
        }

        private string GenerateReceivableRef(int maxSl)
        {
            var refCode = _iCommonManager.GetAllSubReferenceAccounts().ToList().Find(n=>n.Id==6).Code;
            string reference = $"{DateTime.Now.Year.ToString().Substring(2, 2)}{refCode}{maxSl + 1}";
            return reference;
        }

        private int GetMaxReceivableSerialNoOfCurrentYear()
        {
          return _iAccountGateway.GetMaxReceivableSerialNoOfCurrentYear();
        }

        public bool ActiveReceivableCheque(ChequeDetails chequeDetails,Receivable aReceivable, Client aClient) 
        {
            aReceivable.VoucherNo = GetMaxVoucherNoByTransactionInfix("101");
            int rowAffected= _iAccountGateway.ActiveReceivableCheque(chequeDetails, aReceivable, aClient);
            return rowAffected > 0;
        }
        private int GetMaxVoucherNoByTransactionInfix(string infix)
        {
            int temp = _invoiceGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        public int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals)
        {
            int maxSl = GetMaxJournalVoucherNoOfCurrentYear();
            aJournal.VoucherRef = DateTime.Now.Year.ToString().Substring(2, 2) + "106" + (maxSl + 1);
            aJournal.VoucherNo = maxSl + 1;
            return _iAccountGateway.SaveJournalVoucher(aJournal,journals);
        }

        private int GetMaxJournalVoucherNoOfCurrentYear()
        {
            return _iAccountGateway.GetMaxJournalVoucherNoOfCurrentYear();
        }

        public IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iAccountGateway.GetAllJournalVouchersByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId)
        {
            return _iAccountGateway.GetAllJournalVouchersByCompanyId(companyId);
        }


        public int SaveVoucher(Voucher voucher)
        {
            int maxSl = GetMaxVoucherNoOfCurrentYearByVoucherType(voucher.VoucherType);
            voucher.VoucherRef = GenerateVoucherRef(maxSl,voucher.VoucherType);
            voucher.VoucherNo = maxSl+1;
            return _iAccountGateway.SaveVoucher(voucher);
        }
        private int GetMaxVoucherNoOfCurrentYearByVoucherType(int voucherType)
        {
            return _iAccountGateway.GetMaxVoucherNoOfCurrentYearByVoucherType(voucherType); 
        }

        private string GenerateVoucherRef(int maxSl,int voucherType)
        {
            string infix = "";
            switch (voucherType)
            {
                case 1:infix = "102";
                    break;
                case 2:
                    infix = "103";
                    break;
                case 3:
                    infix = "104";
                    break;
                case 4:
                    infix = "105";
                    break;
            }
            string reference = DateTime.Now.Year.ToString().Substring(2, 2) + infix + (maxSl + 1);
            return reference;
        }

        public IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType) 
        {
           return _iAccountGateway.GetAllVouchersByBranchCompanyIdVoucherType(branchId, companyId, voucherType);
        }


        public Voucher GetVoucherByVoucherId(int voucherId)
        {
            return _iAccountGateway.GetVoucheByVoucherId(voucherId);
        }

        public IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId)
        {
            return _iAccountGateway.GetVoucherDetailsByVoucherId(voucherId);
        }

        public IEnumerable<Voucher> GetVoucherList()
        {
            return _iAccountGateway.GetVoucherList();
        }

        public IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId)
        {
            return _iAccountGateway.GetVoucherListByCompanyId(companyId);
        }

        public IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetVoucherListByBranchAndCompanyId(branchId,companyId);
        }
        public IEnumerable<Voucher> GetPendingVoucherList()
        {
            return _iAccountGateway.GetPendingVoucherList();
        }
        public bool CancelVoucher(int voucherId, string reason, int userId)
        {
            int rowAffected= _iAccountGateway.CancelVoucher(voucherId,reason,userId);
            return rowAffected>0;
        }

        public bool ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails,int userId)
        {
            int rowAffected = _iAccountGateway.ApproveVoucher(aVoucher, voucherDetails,userId);
            return rowAffected > 0;
        }

        public bool UpdateVoucher(Voucher voucher,List<VoucherDetails> voucherDetails)
        {
            int rowAffected = _iAccountGateway.UpdateVoucher(voucher,voucherDetails);
            return rowAffected > 0;
        }

        public List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId)
        {
            return _iAccountGateway.GetJournalVoucherDetailsById(journalVoucherId);
        }

        public JournalVoucher GetJournalVoucherById(int journalId)
        {
            return _iAccountGateway.GetJournalVoucherById(journalId);
        }

        public bool CancelJournalVoucher(int voucherId, string reason, int userId)
        {
            int rowAffected = _iAccountGateway.CancelJournalVoucher(voucherId, reason, userId);
            return rowAffected > 0;
        }

        public bool UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers)
        {
            int rowAffected = _iAccountGateway.UpdateJournalVoucher(voucher, journalVouchers);
            return rowAffected > 0;
        }

        public List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId)
        {
            return _iAccountGateway.GetAllPendingJournalVoucherByBranchAndCompanyId(branchId, companyId);
        }

        public bool ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId)
        {
            int rowAffected = _iAccountGateway.ApproveJournalVoucher(aVoucher, voucherDetails, userId);
            return rowAffected > 0;
        }

        public bool ApproveVat(Vat vat)
        {
            return _iAccountGateway.ApproveVat(vat) > 0;
        }

        public bool ApproveDiscount(Discount discount)
        {
            return _iAccountGateway.ApproveDiscount(discount) > 0;
        }

        public decimal GetTotalSaleValueOfCurrentMonth()
        {
            return _iAccountGateway.GetTotalSaleValueOfCurrentMonth();
        }
        public decimal GetTotalSaleValueOfCurrentMonthByCompanyId(int companyId)
        {
            return _iAccountGateway.GetTotalSaleValueOfCurrentMonthByCompanyId(companyId);
        }
        public decimal GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iAccountGateway.GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(branchId,companyId);
        }
        public decimal GetTotalCollectionOfCurrentMonth()
        {
            return _iAccountGateway.GetTotalCollectionOfCurrentMonth();
        }
        public decimal GetTotalCollectionOfCurrentMonthByCompanyId(int companyId)
        {
            return _iAccountGateway.GetTotalCollectionOfCurrentMonthByCompanyId(companyId);
        }
        public decimal GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId)
        {
            return _iAccountGateway.GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(branchId,companyId);
        }
        public decimal GetTotalOrderedAmountOfCurrentMonth() 
        {
            return _iAccountGateway.GetTotalOrderedAmountOfCurrentMonth();
        }
        public decimal GetTotalOrderedAmountOfCurrentMonthByCompanyId(int companyId)
        {
            return _iAccountGateway.GetTotalOrderedAmountOfCurrentMonthByCompanyId(companyId);
        }
        public decimal GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(int branchId,int companyId) 
        {
            return _iAccountGateway.GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(branchId,companyId);
        }
    }
}