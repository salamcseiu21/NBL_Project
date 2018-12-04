using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models.ViewModels
{
   public class ViewReceivableCreateModel
    {
        public IEnumerable<PaymentType> PaymentTypes { get; set; }
        public IEnumerable<TransactionType> TransactionTypes { get; set; } 
    }
}
