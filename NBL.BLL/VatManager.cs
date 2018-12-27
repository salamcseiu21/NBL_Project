﻿using System.Collections.Generic;
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

        public IEnumerable<Vat> GetAllPendingVats()
        {
            return _iVatGateway.GetAllPendingVats();
        }

        public IEnumerable<Vat> GetProductWishVat()
        {
            return _iVatGateway.GetProductWishVat();
        }

        public bool Add(Vat model)
        {
            return _iVatGateway.Add(model) > 0;
        }

        public bool Update(Vat model)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(Vat model)
        {
            throw new System.NotImplementedException();
        }

        public Vat GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Vat> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}