using System.Collections.Generic;

namespace NBL.Models.ViewModels
{
   public class ViewReceivableCreateModel
    {
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        public IEnumerable<TransactionType> TransactionTypes { get; set; } 
    }
}
