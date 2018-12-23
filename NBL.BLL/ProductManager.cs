using System;
using System.Collections.Generic;
using System.Linq;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class ProductManager
    {
        readonly ProductGateway _productGateway=new ProductGateway(); 
        readonly CommonGateway _commonGateway=new CommonGateway();
        public IEnumerable<Product> GetAll => _productGateway.GetAll;
        public IEnumerable<ViewProduct> GetAllProductByBranchAndCompanyId(int branchId, int companyId)
        {
          return  _productGateway.GetAllProductByBranchAndCompanyId(branchId,companyId);
        }
        private int GetProductMaxSerialNo()
        {
            return _productGateway.GetProductMaxSerialNo();
        }

        public IEnumerable<TransferIssue> GetDeliverableTransferIssueList()
        {
            return _productGateway.GetDeliverableTransferIssueList();
        }

        public IEnumerable<ViewProduct> GetAllProductsByProductCategoryId(int productCategoryId)
        {
            
            return _productGateway.GetAllProductsByProductCategoryId(productCategoryId); 
        }

        public int TransferProduct(List<TransactionModel> transactionModels, TransactionModel model)
        {
            model.TransactionRef = GenerateInvoiceNo(model);
            int rowAffected = _productGateway.TransferProduct(transactionModels, model);
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
            int rowAffected = _productGateway.Save(aProduct);
            if (rowAffected > 0)
            {
                return "Saved Successfully!";
            }

            return "Failed To Save";
        }

        public bool ApproveTransferIssue(TransferIssue transferIssue)
        {
            int rowAffected = _productGateway.ApproveTransferIssue(transferIssue);
            if(rowAffected>0)
            return true;
            return false;

        }

        public int IssueProductToTransfer(TransferIssue aTransferIssue)
        {
            int maxTrNo = _productGateway.GetMaxTransferIssueNoOfCurrentYear();
            aTransferIssue.TransferIssueRef = GenerateTransferIssueRef(maxTrNo);
            int rowAffected = _productGateway.IssueProductToTransfer(aTransferIssue);
            return rowAffected;
        }
        /// <summary>
        /// id=3 stands for transfer issue from factory ...
        /// </summary>
        /// <param name="maxTrNo"></param>
        /// <returns></returns>
        private string GenerateTransferIssueRef(int maxTrNo)
        {

           
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 3).Code;
            string temp = (maxTrNo + 1).ToString();
            string reference=DateTime.Now.Year.ToString().Substring(2,2)+ refCode+temp;
            return reference;
        }

        public ProductDetails GetProductDetailsByProductId(int productId)
        {
            return _productGateway.GetProductDetailsByProductId(productId);
        }

        public IEnumerable<TransferIssue> GetTransferIssueList() 
        {
            return _productGateway.GetTransferIssueList();
        }

        public IEnumerable<TransferIssueDetails> GetTransferIssueDetailsById(int id)
        {
            return _productGateway.GetTransferIssueDetailsById(id); 
        }

        public Product GetProductByProductAndClientTypeId(int productId, int clientTypeId)
        {
            return _productGateway.GetProductByProductAndClientTypeId(productId,clientTypeId);
        }

        public Product GetProductByProductId(int productId)
        {
            return _productGateway.GetProductByProductId(productId);
        }

        public bool SaveProductionNote(ProductionNote productionNote)
        {

            productionNote.ProductionNoteNo =DateTime.Now.Year.ToString().Substring(2,2)+GetMaxProductNoteNo(DateTime.Now.Year);
            productionNote.ProductionNoteRef= GenerateProductNoteRef(DateTime.Now.Year);
            int rowAffected = _productGateway.SaveProductionNote(productionNote);
            return rowAffected>0;
        }

        private string GenerateProductNoteRef(int year)
        {
            int maxNoteNo = _productGateway.GetMaxProductionNoteNoByYear(year);
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 7).Code;
            return $"{year.ToString().Substring(2, 2)}{refCode}{maxNoteNo+1}";
        }

        private int GetMaxProductNoteNo(int year) 
        {
            int maxNoteNo = _productGateway.GetMaxProductionNoteNoByYear(year)+1;
            return maxNoteNo;
        }

        public IEnumerable<ViewProductionNoteModel> PendingProductionNote()
        {
            return _productGateway.PendingProductionNote();
        }
    }
}