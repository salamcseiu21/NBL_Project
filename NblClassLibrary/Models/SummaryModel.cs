using System.Collections.Generic;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.Models
{
    public class SummaryModel
    {
        public decimal TotalSale { get; set; }
        public decimal TotalCollection { get; set; }
        public decimal OrderedAmount { get; set; }
        public ViewTotalOrder TotalOrder { get; set; }
        public int CompanyId { get; set; }
        public int BranchId { get; set; }

        public decimal CollectionPercentageOfSale
        {
            get
            {
               decimal percentage= TotalSale != 0 ? TotalCollection * 100 / TotalSale : 0;
                return percentage;
            }
        }

        public IEnumerable<ViewClient> Clients { get; set; }
        public IEnumerable<ViewClient> TopClients { get; set; }
        public IEnumerable<ViewProduct> TopProducts { get; set; }   
        public IEnumerable<ViewProduct> Products { get; set; }
        public IEnumerable<ViewBranch> Branches { get; set; }
        public IEnumerable<Invoice> InvoicedOrderList { get; set; }
        public IEnumerable<ViewOrder> Orders { get; set; }
        public IEnumerable<ViewOrder> DelayedOrders { get; set; }
        public IEnumerable<ViewOrder> PendingOrders { set; get; }
        public IEnumerable<ViewEmployee> Employees { get; set; }    
        public IEnumerable<ViewVerifiedOrderModel> VerifiedOrders { set; get; }

        public SummaryModel()
        {
            TotalOrder=new ViewTotalOrder();
            Clients=new List<ViewClient>();
            Branches=new List<ViewBranch>();
            Products=new List<ViewProduct>();
            InvoicedOrderList=new List<Invoice>();
            Orders=new List<ViewOrder>();
            DelayedOrders=new List<ViewOrder>();
            PendingOrders=new List<ViewOrder>();
            TopClients=new List<ViewClient>();
            Employees=new List<ViewEmployee>();
            VerifiedOrders=new List<ViewVerifiedOrderModel>();
        }
    }
}