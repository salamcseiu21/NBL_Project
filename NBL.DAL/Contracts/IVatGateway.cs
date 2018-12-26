using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.DAL.Contracts
{
    public interface IVatGateway
    {
       int  AddVat(Vat vat);
       IEnumerable<Vat> GetAllPendingVats();
       IEnumerable<Vat> GetProductWishVat();
    }
}
