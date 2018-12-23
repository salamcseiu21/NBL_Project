using System.Collections.Generic;

namespace NBL.Models.ViewModels
{
   public class ViewInvoiceModel
    {
        public IEnumerable<InvoiceDetails> InvoiceDetailses { get; set; }
        public ViewClient Client { get; set; }
        public Order Order { get; set; }
        public Invoice Invoice { get; set; } 
    }
}
