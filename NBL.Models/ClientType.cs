using System.Collections.Generic;
namespace NBL.Models 
{
    public class ClientType
    {
        public int ClientTypeId { get; set; }
        public string ClientTypeName { get; set; }
        public decimal DiscountPercent { get; set; }
        public List<Client> Clients { get; set; } 
    }
}