﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NblClassLibrary.Models
{
    public class Invoice
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

        public int OrderId { get; set; }
        [Display(Name = "Client Id")]
        [Required]
        public int ClientId { get; set; }
        [Display(Name = "Order Slip No")]
        public string OrderSlipNo { get; set; }
        [Display(Name = "User")]
        public int UserId { get; set; }
        [Display(Name = "Nsm User Id")]
        public int NsmUserId { get; set; }
        [Display(Name = "Delivered Or Receive User Id")]
        public int DeliveredOrReceiveUserId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public decimal Amounts { get; set; }
        public decimal NetAmounts { get; set; }
        public decimal Discount { get; set; }
        public decimal SpecialDiscount { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }

        public DateTime SysDate { get; set; }
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Approved By Nsm")]
        public DateTime ApprovedByNsmDateTime { get; set; }
        [Display(Name = "Admin User Id")]
        public int AdminUserId { get; set; }
        [Display(Name = "Approved By Admin")]
        public DateTime ApprovedByAdminDateTime { get; set; }

        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDateTime { get; set; }
        public int DeliveredByUserId { get; set; }
        public string OrederRef { get; set; }
        public char Cancel { get; set; }
        public string ResonOfCancel { get; set; }
        public int CancelByUserId { get; set; }
        public decimal Vat { get; set; }
        public DateTime CancelDateTime { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public Client Client { get; set; }
        public User User { get; set; }
    }
}