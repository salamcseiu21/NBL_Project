﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NblClassLibrary.Models
{
    public class ProductDetails
    {

        public int ProductDetailsId { get; set; }   
        public DateTime UpdatedDate { get; set; }
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DealerPrice { get; set; }
    }
}