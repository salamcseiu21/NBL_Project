using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface ICommonManager
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
