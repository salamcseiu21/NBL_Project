using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IProductGateway
    {
        IEnumerable<Product> GetAll();
        IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId);
        Product GetProductByProductAndClientTypeId(int productId, int clientTypeId);
        int GetMaxTransferIssueNoOfCurrentYear();
        IEnumerable<TransferIssue> GetDeliverableTransferIssueList();
        int ApproveTransferIssue(TransferIssue transferIssue);
        int IssueProductToTransfer(TransferIssue aTransferIssue);
        IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id);
        IEnumerable<TransferIssue> GetTransferIssueList();
        int SaveTransferIssueDetails(List<Product> products, int transferIssueId);
        int GetProductMaxSerialNo();
        IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId);
        int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model);
        int SaveTransferDetails(List<TransactionModel> transactionModels, int inventoryMasterId);
        int Save(Product aProduct);
        ProductDetails GetProductDetailsByProductId(int productId);
        Product GetProductByProductId(int productId);
        int GetMaxProductionNoteNoByYear(int year);
        int SaveProductionNote(ProductionNote productionNote);
        IEnumerable<ViewProductionNoteModel> PendingProductionNote();
    }
}
