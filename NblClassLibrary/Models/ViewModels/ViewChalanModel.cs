using System.Collections.Generic;

namespace NblClassLibrary.Models.ViewModels
{
    public class ViewChalanModel
    {
        public IEnumerable<DeliveryDetails> DeliveryDetailses { get; set; }
        public ViewClient ViewClient { get; set; }  

    }
}
