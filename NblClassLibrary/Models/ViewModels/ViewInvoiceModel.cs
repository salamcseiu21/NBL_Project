using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models.ViewModels
{
   public class ViewInvoiceModel
    {
        public IEnumerable<InvoiceDetails> InvoiceDetailses { get; set; }
        public ViewClient Client { get; set; }
        public Order Order { get; set; }
        public Invoice Invoice { get; set; } 
    }
}
