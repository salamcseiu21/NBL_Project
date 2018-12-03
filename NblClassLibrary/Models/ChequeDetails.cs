using System;

namespace NblClassLibrary.Models
{
    public class ChequeDetails:Payment
    {
        public int ChequeDetailsId { get; set; }
        public int ReceivableId { get; set; }
        public DateTime SysDateTime { get; set; }
        public string ReceivableRef { get; set; }
        public int ClientId { get; set; }


    }
}