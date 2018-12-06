
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NblClassLibrary.BLL;

namespace NblClassLibrary.Models.ViewModels
{
   public class ViewSalesSummaryModel
    {
        readonly OrderManager orderManager=new OrderManager();
        [Display(Name = "Branch")]
        public int BranchId { get; set; } 
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Order> Orders { get; set; }

        public ViewSalesSummaryModel()
        {
            Orders = orderManager.GetAll;
        }
    }
}
