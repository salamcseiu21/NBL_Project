﻿
namespace NBL.Models
{
    public class MailingAddress
    {
        public string HouseNo { get; set; }
        public PostOffice PostOffice { get; set; }
        public District District { get; set; }
        public Division Division { get; set; }
        public Upazilla Upazilla { get; set; }
    }
}