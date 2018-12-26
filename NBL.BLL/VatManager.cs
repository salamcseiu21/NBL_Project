using System.Collections.Generic;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;

namespace NBL.BLL
{

    public class VatManager:IVatManager
    {

        readonly  IVatGateway _iVatGateway;

        public VatManager(IVatGateway iVatGateway)
        {
            _iVatGateway = iVatGateway;
        }
       
        

        public bool AddVat(Vat vat)
        {
            return _iVatGateway.AddVat(vat)>0;
        }

        public IEnumerable<Vat> GetAllPendingVats()
        {
            return _iVatGateway.GetAllPendingVats();
        }

        public IEnumerable<Vat> GetProductWishVat()
        {
            return _iVatGateway.GetProductWishVat();
        }
    }
}