namespace NBL.Models
{
    public class InvoiceDetails:Invoice
    {
        public int InvoiceDetailsId { get; set; }
        public int InvoicedQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public int StockQuantity { get; set; } 
    }
}