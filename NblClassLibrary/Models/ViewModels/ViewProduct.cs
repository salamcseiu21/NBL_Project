using System.ComponentModel.DataAnnotations;
using NBL.Models;

namespace NblClassLibrary.Models.ViewModels
{
    public class ViewProduct:Product
    {
        public int BranchId { get; set; }
        public string ProductTypeName { get; set; } 
        public string ProductCategoryName { get; set; }
        [Display(Name = "Discount Amount")]
        public new decimal DiscountAmount { get; set; }
        [Display(Name = "Dealer Comision")]
        public decimal DealerComision { get; set; }
        public int TotalRe { get; set; }
        public int TotalSoldQty { get; set; } 
        public int TotalTr { get; set; }
        public decimal CostPrice { get; set; }
        public int StockQuantity { get; set; }


    }
}