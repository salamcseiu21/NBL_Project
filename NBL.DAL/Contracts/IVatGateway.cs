using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.DAL.Contracts
{
    public interface IVatGateway:IGateway<Vat>
    {
       IEnumerable<Vat> GetAllPendingVats();
       IEnumerable<Vat> GetProductWishVat();
    }
}
