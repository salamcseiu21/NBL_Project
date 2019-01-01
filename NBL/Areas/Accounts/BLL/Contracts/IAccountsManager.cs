using System.Collections.Generic;
using NBL.Areas.Accounts.Models;
using NBL.Models;

namespace NBL.Areas.Accounts.BLL.Contracts
{
    public interface IAccountsManager
   {
       int SaveReceivable(Receivable receivable);
       IEnumerable<ChequeDetails> GetAllReceivableChequeByBranchAndCompanyId(int branchId, int companyId);
       ChequeDetails GetReceivableChequeByDetailsId(int chequeDetailsId);
       int SaveJournalVoucher(JournalVoucher aJournal, List<JournalVoucher> journals);
       IEnumerable<JournalVoucher> GetAllJournalVouchersByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<JournalVoucher> GetAllJournalVouchersByCompanyId(int companyId);
       int SaveVoucher(Voucher voucher);
       IEnumerable<Voucher> GetAllVouchersByBranchCompanyIdVoucherType(int branchId, int companyId, int voucherType);
       Voucher GetVoucherByVoucherId(int voucherId);
       IEnumerable<VoucherDetails> GetVoucherDetailsByVoucherId(int voucherId);
       IEnumerable<Voucher> GetVoucherList();
       IEnumerable<Voucher> GetVoucherListByCompanyId(int companyId);
       IEnumerable<Voucher> GetVoucherListByBranchAndCompanyId(int branchId, int companyId);
       IEnumerable<Voucher> GetPendingVoucherList();
       bool CancelVoucher(int voucherId, string reason, int userId);
       bool ApproveVoucher(Voucher aVoucher, List<VoucherDetails> voucherDetails, int userId);
       bool UpdateVoucher(Voucher voucher, List<VoucherDetails> voucherDetails);
       List<JournalDetails> GetJournalVoucherDetailsById(int journalVoucherId);
       JournalVoucher GetJournalVoucherById(int journalId);
       bool CancelJournalVoucher(int voucherId, string reason, int userId);
       bool UpdateJournalVoucher(JournalVoucher voucher, List<JournalDetails> journalVouchers);
       List<JournalVoucher> GetAllPendingJournalVoucherByBranchAndCompanyId(int branchId, int companyId);
       bool ApproveJournalVoucher(JournalVoucher aVoucher, List<JournalDetails> voucherDetails, int userId);
       bool ApproveVat(Vat vat);
       bool ApproveDiscount(Discount discount);
       decimal GetTotalSaleValueOfCurrentMonth();
       decimal GetTotalSaleValueOfCurrentMonthByCompanyId(int companyId);
       decimal GetTotalSaleValueOfCurrentMonthByBranchAndCompanyId(int branchId, int companyId);
       decimal GetTotalCollectionOfCurrentMonth();
       decimal GetTotalCollectionOfCurrentMonthByCompanyId(int companyId);
       decimal GetTotalCollectionOfCurrentMonthByBranchAndCompanyId(int branchId, int companyId);
       decimal GetTotalOrderedAmountOfCurrentMonth();
       decimal GetTotalOrderedAmountOfCurrentMonthByCompanyId(int companyId);
       decimal GetTotalOrderedAmountOfCurrentMonthByBranchAndCompanyId(int branchId, int companyId);
       bool ActiveReceivableCheque(ChequeDetails chequeDetails, Receivable aReceivable, Client aClient);
   }
}
