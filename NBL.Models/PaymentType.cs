using System.Collections.Generic;

namespace NBL.Models
{
    public class PaymentType
    {
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public string PaymentTypeAccountCode  { get; set; }
        public List<Payment> Payments { get; set; } 

    }
}