using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
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