﻿using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
{
    public class DiscountManager
    {
        readonly DiscountGateway _discountGateway=new DiscountGateway();

        public IEnumerable<Discount> GetAllDiscounts()
        {
           return _discountGateway.GetAllDiscounts();
        }

        public IEnumerable<Discount> GetAllDiscountsByClientTypeId(int clientTypeId) 
        {
            return _discountGateway.GetAllDiscountsByClientTypeId(clientTypeId);
        }

        public bool AddDiscount(Discount discount)
        {
            return _discountGateway.AddDiscount(discount) > 0;
        }

        public IEnumerable<Discount> GetAllPendingDiscounts()
        {
            return _discountGateway.GetAllPendingDiscounts();
        }
    }
}