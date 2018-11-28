
using NblClassLibrary.Models;

namespace NBL.Areas.Manager.Models
{
    public class DeliveryModel:Inventory
    {
        public int ClientId { get; set; }
        public string Invoice { get; set; }
        public string OrderSlipNo { get; set; }
        public int OrderByUserId { get; set; } 
        public string CategoryName { get; set; } 
        public decimal SubTotal
        {
            get
            {
               return  Quantity * UnitPrice;
            }
        }
    }
}