using System;
using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NBL.Areas.Factory.DAL;

namespace NBL.Areas.Factory.BLL
{
    public class DeliveryManager
    {
        DeliveryGateway _deliveryGateway = new DeliveryGateway();
        InventoryGateway _inventoryGateway = new InventoryGateway();
        public string SaveDeliveryInformation(Delivery aDelivery, IEnumerable<TransferIssueDetails> issueDetails)
        {
            int maxDeliveryNo=  _inventoryGateway.GetMaxDeliveryRefNoOfCurrentYear();
            aDelivery.DeliveryRef = GenerateDeliveryReference(maxDeliveryNo);
            int rowAffected= _deliveryGateway.SaveDeliveryInformation(aDelivery, issueDetails);
            if (rowAffected > 0)
                return "Saved Successfully!";
            return "Failed to Save";
        }

        private string GenerateDeliveryReference(int maxDeliveryNo)
        {
            string temp = (maxDeliveryNo + 1).ToString();
            string reference =DateTime.Now.Year.ToString().Substring(2,2)+"DE"+ temp;
            return reference;
        }
    }
}