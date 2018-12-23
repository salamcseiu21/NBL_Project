using System.Collections.Generic;
using NBL.DAL;
using NBL.Models;

namespace NBL.BLL
{

    public class VatManager
    {

        readonly  VatGateway _vatGateway=new VatGateway();

        public bool AddVat(Vat vat)
        {
            return _vatGateway.AddVat(vat)>0;
        }

        public IEnumerable<Vat> GetAllPendingVats()
        {
            return _vatGateway.GetAllPendingVats();
        }

        public IEnumerable<Vat> GetProductWishVat()
        {
            return _vatGateway.GetProductWishVat();
        }
    }
}