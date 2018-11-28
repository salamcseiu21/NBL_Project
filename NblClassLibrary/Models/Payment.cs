
using System;

namespace NblClassLibrary.Models
{
    public class Payment:PaymentType
    {
        public int PaymentId { get; set; }
        public string SourceBankName { get; set; }
        public string BankAccountNo { get; set; }  
        public decimal ChequeAmount { get; set; }
        public DateTime ChequeDate { get; set; } 
        public string ChequeNo { get; set; }
        public string TransactionId { get; set; }
        public string BankBranchName { get; set; }
        public string Remarks { get; set; }

    }
}