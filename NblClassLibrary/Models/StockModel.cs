
namespace NblClassLibrary.Models
{
    public class StockModel:Product
    {
        public int StockQty { get; set; }
        public int BranchId { get; set; }
        public int TotalRe { get; set; }
        public int TotalSe { get; set; }
        public int TotalTr { get; set; }

    }
}