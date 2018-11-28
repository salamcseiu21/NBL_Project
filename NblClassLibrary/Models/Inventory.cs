using System;
using NBL.Models;

namespace NblClassLibrary.Models
{
    public class Inventory: ProductDetails
    {
    public int InventoryId { get; set; }
    public int BranchId { get; set; }
    public DateTime TransactionDate { get; set; } 
    public string TransactionType { get; set; }
    public int Transactionid { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public int Status { get; set; }
    }
}