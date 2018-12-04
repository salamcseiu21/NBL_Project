using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models.ViewModels
{
   public class ViewReceivableCreateModel
    {
        public List<PaymentType> PaymentTypes { get; set; }
        public List<TransactionType> TransactionTypes { get; set; } 
    }
}
