
using System.Collections.Generic;
namespace NBL.Areas.Accounts.Models.ViewModels
{
    public class ViewVoucherModel
    {
        public Voucher Voucher { get; set; }
        public IEnumerable<VoucherDetails> VoucherDetails { get; set; }
    }
}