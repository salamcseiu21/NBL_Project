using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.Models
{
    public class ViewChalanModel
    {
        public IEnumerable<DeliveryDetails> DeliveryDetailses { get; set; }
        public ViewClient ViewClient { get; set; }  

    }
}
