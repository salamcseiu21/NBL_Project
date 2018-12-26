
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface ICommonGateway
   {
       IEnumerable<ClientType> GetAllClientType();
       IEnumerable<ProductCategory> GetAllProductCategory();
       IEnumerable<ProductType> GetAllProductType();
       IEnumerable<Branch> GetAssignedBranchesToUserByUserId(int userId);
       IEnumerable<UserRole> GetAllUserRoles();
       IEnumerable<PaymentType> GetAllPaymentTypes();
       IEnumerable<TransactionType> GetAllTransactionTypes();
       IEnumerable<Supplier> GetAllSupplier();
       IEnumerable<Bank> GetAllBank();
       IEnumerable<BankBranch> GetAllBankBranch();
       IEnumerable<MobileBanking> GetAllMobileBankingAccount();
       IEnumerable<SubSubSubAccount> GetAllSubSubSubAccounts();
       SubSubSubAccount GetSubSubSubAccountByCode(string accountCode);
       Vat GetCurrentVatByProductId(int productId);
       Discount GetCurrentDiscountByClientTypeId(int clientTypeId);
       IEnumerable<ReferenceAccount> GetAllReferenceAccounts();
       IEnumerable<ViewReferenceAccountModel> GetAllSubReferenceAccounts();
       IEnumerable<Status> GetAllStatus();
   }
}
