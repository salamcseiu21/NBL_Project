using System;
using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class ProductManager:IProductManager
    {
        private readonly  IProductGateway _iProductGateway;
        private readonly ICommonGateway _iCommonGateway;

        public ProductManager(IProductGateway iProductGateway,ICommonGateway iCommonGateway)
        {
            _iProductGateway = iProductGateway;
            _iCommonGateway = iCommonGateway;
        }

        public IEnumerable<Product> GetAll()
        {
           return _iProductGateway.GetAll().ToList();

        }
        public IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId)
        {
          return  _iProductGateway.GetAllProductByBranchAndCompanyId(branchId,companyId);
        }
        public int GetProductMaxSerialNo()
        {
            return _iProductGateway.GetProductMaxSerialNo();
        }

        public IEnumerable<TransferIssue> GetDeliverableTransferIssueList()
        {
            return _iProductGateway.GetDeliverableTransferIssueList();
        }

        public IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId)
        {
            
            return _iProductGateway.GetAllProductsByProductCategoryId(productCategoryId); 
        }

        public int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model)
        {
            model.TransactionRef = GenerateInvoiceNo(model);
            int rowAffected = _iProductGateway.TransferProduct(transactionModels, model);
            return rowAffected;
        }

        private string GenerateInvoiceNo(TransactionModel model)
        {
            string temp = Guid.NewGuid().ToString().ToUpper().Replace("-", "").Substring(0,5);
            string invoice = "TR" + model.FromBranchId+model.ToBranchId + DateTime.Now.Date.ToString("yyyy MMM dd").Replace(" ", "").ToUpper() + temp;
            return invoice;
        }
        public string Save(Product aProduct)
        {
            int lastSlNo = GetProductMaxSerialNo();
            string subSubSubAccountCode = Generator.GenerateAccountCode("2201",lastSlNo);
            aProduct.SubSubSubAccountCode = subSubSubAccountCode;
            int rowAffected = _iProductGateway.Save(aProduct);
            if (rowAffected > 0)
            {
                return "Saved Successfully!";
            }

            return "Failed To Save";
        }

        public bool ApproveTransferIssue(TransferIssue transferIssue)
        {
            int rowAffected = _iProductGateway.ApproveTransferIssue(transferIssue);
            if(rowAffected>0)
            return true;
            return false;

        }

        public int IssueProductToTransfer(TransferIssue aTransferIssue)
        {
            int maxTrNo = _iProductGateway.GetMaxTransferIssueNoOfCurrentYear();
            aTransferIssue.TransferIssueRef = GenerateTransferIssueRef(maxTrNo);
            int rowAffected = _iProductGateway.IssueProductToTransfer(aTransferIssue);
            return rowAffected;
        }
        /// <summary>
        /// id=3 stands for transfer issue from factory ...
        /// </summary>
        /// <param name="maxTrNo"></param>
        /// <returns></returns>
        private string GenerateTransferIssueRef(int maxTrNo)
        {

            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 3).Code;
            string temp = (maxTrNo + 1).ToString();
            string reference=DateTime.Now.Year.ToString().Substring(2,2)+ refCode+temp;
            return reference;
        }

        public ProductDetails GetProductDetailsByProductId(int productId)
        {
            return _iProductGateway.GetProductDetailsByProductId(productId);
        }

        public IEnumerable<TransferIssue> GetTransferIssueList() 
        {
            return _iProductGateway.GetTransferIssueList();
        }

        public IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id)
        {
            return _iProductGateway.GetTransferIssueDetailsById(id); 
        }

        public Product GetProductByProductAndClientTypeId(int productId, int clientTypeId)
        {
            return _iProductGateway.GetProductByProductAndClientTypeId(productId,clientTypeId);
        }

        public Product GetProductByProductId(int productId)
        {
            return _iProductGateway.GetProductByProductId(productId);
        }

        public bool SaveProductionNote(ProductionNote productionNote)
        {

            productionNote.ProductionNoteNo =DateTime.Now.Year.ToString().Substring(2,2)+GetMaxProductNoteNo(DateTime.Now.Year);
            productionNote.ProductionNoteRef= GenerateProductNoteRef(DateTime.Now.Year);
            int rowAffected = _iProductGateway.SaveProductionNote(productionNote);
            return rowAffected>0;
        }

        private string GenerateProductNoteRef(int year)
        {
            int maxNoteNo = _iProductGateway.GetMaxProductionNoteNoByYear(year);
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 7).Code;
            return $"{year.ToString().Substring(2, 2)}{refCode}{maxNoteNo+1}";
        }

        private int GetMaxProductNoteNo(int year) 
        {
            int maxNoteNo = _iProductGateway.GetMaxProductionNoteNoByYear(year)+1;
            return maxNoteNo;
        }

        public IEnumerable<ViewProductionNoteModel> PendingProductionNote()
        {
            return _iProductGateway.PendingProductionNote();
        }
    }
}