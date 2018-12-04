using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NblClassLibrary.Models
{
   public class TransactionType
    {
       public int TransactionTypeId { set; get; }
        [Display(Name = "Transaction Type Name")]
       public string TransactionTypeName { set; get; }
    }
}
