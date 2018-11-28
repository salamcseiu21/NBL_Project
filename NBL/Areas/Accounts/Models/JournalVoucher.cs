
namespace NBL.Areas.Accounts.Models
{
    public class JournalVoucher:Voucher
    {

        public int JournalId { get; set; }
        public string PurposeName { get; set; }
        public string DebitOrCredit { get; set; }   
        public string PurposeCode { get; set; }
    }
}