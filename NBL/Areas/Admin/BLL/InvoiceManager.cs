﻿
using System;
using System.Collections.Generic;
using System.Linq;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.Areas.Admin.DAL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.Areas.Admin.BLL
{
    public class InvoiceManager:IInvoiceManager
    {

        private readonly IInvoiceGateway _iInvoiceGateway;
        private readonly ICommonGateway _iCommonGateway;

        public InvoiceManager(ICommonGateway iCommonGateway, IInvoiceGateway iInvoiceGateway)  
        {
            _iCommonGateway = iCommonGateway;
            _iInvoiceGateway = iInvoiceGateway;
        }
        //-----------13-Sep-2018-----------
        public string Save(IEnumerable<OrderItem> orderItems, Invoice anInvoice)
        {
            //------------- Id==1 means order ref....
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 1).Code;
            int maxSl = _iInvoiceGateway.GetMaxInvoiceNoOfCurrentYear();
            anInvoice.InvoiceNo = _iInvoiceGateway.GetMaxInvoiceNo() + 1;
            anInvoice.InvoiceRef = GenerateInvoiceRef(maxSl);
            anInvoice.VoucherNo = GetMaxVoucherNoByTransactionInfix(refCode);

            int rowAffected = _iInvoiceGateway.Save(orderItems, anInvoice);
            if (rowAffected > 0)
                return "Saved Invoice information Successfully!";
            return "Failed to Save";
        }

        private int GetMaxVoucherNoByTransactionInfix(string infix)
        {
            int temp = _iInvoiceGateway.GetMaxVoucherNoByTransactionInfix(infix);
            return temp + 1;
        }

        private string GenerateInvoiceRef(int maxSl)
        {
            //------------- Id==2 means invoice ref....
            string refCode = _iCommonGateway.GetAllSubReferenceAccounts().ToList().Find(n => n.Id == 2).Code;
            int sN = 1 + maxSl;
            string invoiceRef = DateTime.Now.Date.Year.ToString().Substring(2, 2) + refCode + sN;
            return invoiceRef;
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByBranchAndCompanyId(int branchId,int companyId)
        {
            var invoices = _iInvoiceGateway.GetAllInvoicedOrdersByBranchAndCompanyId(branchId, companyId);
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
            return _iInvoiceGateway.GetAllInvoicedOrdersByBranchCompanyAndUserId(branchId, companyId, invoiceByUserId);
        }

        public IEnumerable<Invoice> GetAllInvoicedOrdersByUserId(int invoiceByUserId)
        {
            return _iInvoiceGateway.GetAllInvoicedOrdersByUserId(invoiceByUserId);
        }
        public IEnumerable<Invoice> GetInvoicedRefferencesByClientId(int clientId)
        {
            return _iInvoiceGateway.GetInvoicedRefferencesByClientId(clientId);
        }
        public IEnumerable<Invoice> GetAllInvoicedOrdersByCompanyId(int companyId)
        {
            return _iInvoiceGateway.GetAllInvoicedOrdersByCompanyId(companyId);
        }
        public IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceId(int invoiceId) 
        {
            return _iInvoiceGateway.GetInvoicedOrderDetailsByInvoiceId(invoiceId);
        }
        public IEnumerable<InvoiceDetails> GetInvoicedOrderDetailsByInvoiceRef(string invoiceRef)
        {
            return _iInvoiceGateway.GetInvoicedOrderDetailsByInvoiceRef(invoiceRef);
        }
        public Invoice GetInvoicedOrderByInvoiceId(int invoiceId)
        {
            return _iInvoiceGateway.GetInvoicedOrderByInvoiceId(invoiceId); 
        }
    }
}