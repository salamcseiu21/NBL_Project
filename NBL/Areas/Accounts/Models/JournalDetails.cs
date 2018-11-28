using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NBL.Areas.Accounts.Models
{
    public class JournalDetails
    {

        public int JournalDetailsId { get; set; }
        public int JournalId { get; set; }
        public string AccountCode { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime SysDateTime { get; set; }
        public string DebitOrCredit { get; set; }


    }
}