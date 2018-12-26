using NBL.Areas.Admin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using NBL.DAL;
using NBL.Models;

namespace NBL.Areas.Admin.BLL
{
    public class InvoiceManager
    {

        readonly InvoiceGateway _invoiceGateway = new InvoiceGateway();
        readonly CommonGateway _commonGateway=new CommonGateway();
        //-----------13-Sep-2018-----------
        internal string Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice)
        {
            //------------- Id==1 means order ref....
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 1).Code;
            int maxSl = _invoiceGateway.GetMaxInvoiceNoOfCurrentYear();
            anInvoice.InvoiceNo = _invoiceGateway.GetMaxInvoiceNo() + 1;
            anInvoice.InvoiceRef = GenerateInvoiceRef(maxSl);
            anInvoice.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);

            int rowAffected = _invoiceGateway.Save(orderItems, anInvoice);
            if (rowAffected > 0)
                return "Saved Invoice information Successfully!";
            return "Failed to Save";
        }

        private int GetMaxVoucherNoByTransactionInfix(string infix)
        {
            int temp = _invoiceGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        private string GenerateInvoiceRef(int maxSl)
        {
            //------------- Id==2 means invoice ref....
            string refCode = _commonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 2).Code;
            int sN = 1 + maxSl;
            string invoiceRef = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
            return invoiceRef;
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            var invoices = _invoiceGateway.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId);
            //foreach (Invoice invoice in invoices)
            //{
            //    //var order = orderManager.GetOrderInfoByTransactionRef(invoice.TransactionRef);
            //    //var orderBy = userManager.GetUserInformationByUserId(order.UserId);
            //    //var client = clientManager.GetClientById(order.ClientId);
            //}
           return invoices;
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchCompanyAndUserId(int branchId, int companyId,int invoiceByUserId)
        {
            return _invoiceGateway.GetAllInvoicedOrdersByBranchCompanyAndUserId(branchId, companyId, invoiceByUserId);
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByUserId(int invoiceByUserId)
        {
            return _invoiceGateway.GetAllInvoicedOrdersByUserId(invoiceByUserId);
        }
        public IEnumerable<Invoice> GetInvoicedRefferencesByClientId(int clientId)
        {
            return _invoiceGateway.GetInvoicedRefferencesByClientId(clientId);
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByCompanyId(int companyId)
        {
            return _invoiceGateway.GetAllInvoicedOrdersByCompanyId(companyId);
        }
        internal IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceId(int invoiceId) 
        {
            return _invoiceGateway.GetInvoicedOrderDetailsByInvoiceId(invoiceId);
        }
        internal IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceRef(string invoiceRef)
        {
            return _invoiceGateway.GetInvoicedOrderDetailsByInvoiceRef(invoiceRef);
        }
        public Invoice GetInvoicedOrderByInvoiceId(int invoiceId)
        {
            return _invoiceGateway.GetInvoicedOrderByInvoiceId(invoiceId); 
        }
    }
}