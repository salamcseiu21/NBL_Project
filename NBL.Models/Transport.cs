﻿
using NBL.Contracts;
namespace NBL.Models
{
    public class Transport:IGetInformation
    {
        public int TransportId { get; set; }  
        public string Transportation { set; get; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public decimal TransportationCost { get; set; }
        public string VehicleNo { get; set; }
        public string GetBasicInformation()
        {
            return Transportation;
        }

        public string GetFullInformation()
        {
            return $"<strong>Transporation:</strong> {Transportation} <br/><strong>Driver Name:</strong> {DriverName} <br/><strong>Driver Phone:</strong> {DriverPhone} <br/><strong>Veichel No:</strong> {VehicleNo} <br/><strong>Cost:</strong> {TransportationCost}";
        }
    }
}