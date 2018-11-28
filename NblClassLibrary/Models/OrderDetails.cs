using System;
namespace NblClassLibrary.Models
{
    public class OrderDetails:Product
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int DeletionStatus { get; set; }
        public string ProductCategoryName { get; set; } 
        public int SlNo { get; set; }
        public DateTime SysDateTime { get; set; }
  

    }
}