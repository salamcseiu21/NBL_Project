using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.BLL.Contracts
{
   public interface IVatManager
   {
         bool  AddVat(Vat vat);
         IEnumerable<Vat> GetAllPendingVats();
         IEnumerable<Vat> GetProductWishVat();

   }
}
