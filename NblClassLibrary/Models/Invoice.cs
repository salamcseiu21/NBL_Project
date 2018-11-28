using System;
using NblClassLibrary.Models;

namespace NBL.Models
{
    public class Invoice:Order
    {
        public int InvoiceId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public int ProductId { get; set; } 
        public DateTime InvoiceDateTime { get; set; }
        public string InvoiceRef { get; set; }
        public string TransactionRef { get; set; }
        public string TransactionType { get; set; }  
        public int InvoiceNo { get; set; }
        public int InvoiceStatus { get; set; }
        public int InvoiceByUserId { get; set; }
        public DateTime SysDateTime { get; set; }
        public int VoucherNo { get; set; }
        public string Explanation { get; set; }
        public int ChequeDetailsId { get; set; }
        public string ClientAccountCode { get; set; }
        public string DiscountAccountCode { get; set; }
        public string SubSubSubAccountCode { get; set; }
        public decimal SubTotal => SalePrice * Quantity; 
    }
}