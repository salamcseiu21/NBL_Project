using System;

namespace NblClassLibrary.Models
{
    public class TransactionModel:ProductDetails
    {
        public int TransactionId { get; set; }
        public string TransactionType { get; set; }
        public string TransactionRef { get; set; }
        public string DeliveryRef { get; set; }
        public int DeliveryId { get; set; }
        public DateTime TransactionDate { get; set; }   
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int StockQuantity { get; set; }
        public int UserId { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public decimal CostPrice { set; get; }

    }
}