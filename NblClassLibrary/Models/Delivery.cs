using System;

namespace NblClassLibrary.Models
{
    public class Delivery:ProductDetails
    {
        public int DeliveryId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryRef { get; set; }
        public string TransactionRef { get; set; }
        public string InvoiceRef { get; set; }
        public string DeliveryType { get; set; }
        public int InvoiceId { get; set; }
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public int Status { get; set; }
        public char Cancel { get; set; }
        public string Remarks { get; set; }
        public int DeliveredByUserId { get; set; }
        public char EntryStatus { get; set; }
        public DateTime SysDateTime { get; set; }
        public int ToBranchId { get; set; }
        public int FromBranchId { get; set; }
        public int TransportId { get; set; }
        public Transport Transport { get; set; }

        public Delivery()
        {
            Transport=new Transport();
            
        }

    }
}