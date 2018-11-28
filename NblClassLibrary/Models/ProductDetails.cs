using System;

namespace NblClassLibrary.Models
{
    public class ProductDetails:Product
    {
      
        public int IsCurrentRate { get; set; }
        public DateTime UpdatedDate { get; set; }


    }
}