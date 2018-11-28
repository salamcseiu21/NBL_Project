
using NBL.Models;

namespace NblClassLibrary.Models.ViewModels
{
    public class ViewOrder:Order
    {

        public string BranchName { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string SubSubSubAccountCode  { get; set; }

    }
}