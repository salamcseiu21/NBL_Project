using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.BLL.Contracts
{
   public interface IDiscountManager:IManager<Discount>
   {
       IEnumerable<Discount> GetAllDiscountsByClientTypeId(int clientTypeId);
       IEnumerable<Discount> GetAllPendingDiscounts();

   }
}
