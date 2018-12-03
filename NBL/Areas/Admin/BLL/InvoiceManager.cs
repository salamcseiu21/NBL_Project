using NBL.Areas.Admin.DAL;
using System;
using System.Collections.Generic;
using NblClassLibrary.Models;

namespace NBL.Areas.Admin.BLL
{
    public class InvoiceManager
    {

        readonly InvoiceGateway _invoiceGateway = new InvoiceGateway();
        //-----------13-Sep-2018-----------
        internal string Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice)
        {

           
            int maxSl = _invoiceGateway.GetMaxInvoiceNoOfCurrentYear();
            anInvoice.InvoiceNo = _invoiceGateway.GetMaxInvoiceNo() + 1;
            anInvoice.InvoiceRef = GenerateInvoiceRef(maxSl);
            anInvoice.VoucherNo = GetMaxVoucherNoByTransactionInfix("OR");

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
            int sN = 1 + maxSl;
            string invoiceRef = DateTime.Now.Date.Year.ToString().Substring(2, 2) + "IN" + sN;
            return invoiceRef;
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
           return _invoiceGateway.GetAllInvoicedOrdersByBranchAndCompanyId(branchId,companyId);
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