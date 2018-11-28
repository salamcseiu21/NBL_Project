
namespace NBL.Areas.Accounts.Models
{
    public class VoucherDetails:Voucher
    {
        public int VoucherDetailsId { get; set; }
        public string DebitOrCredit { get; set; } 
    }
}