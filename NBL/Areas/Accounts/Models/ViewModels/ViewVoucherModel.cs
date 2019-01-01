using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBL.Areas.Accounts.Models.ViewModels
{
    public class ViewVoucherModel
    {
        public Voucher Voucher { get; set; }
        public IEnumerable<VoucherDetails> VoucherDetails { get; set; }
    }
}