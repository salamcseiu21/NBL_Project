using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.BLL.Contracts
{
   public interface IVatManager:IManager<Vat>
   {
         IEnumerable<Vat> GetAllPendingVats();
         IEnumerable<Vat> GetProductWishVat();

   }
}
