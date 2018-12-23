using System.Collections.Generic;
using System.Linq;
using NBL.DAL;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
    public class ReportManager
    {
        readonly ReportGateway _reportGateway=new ReportGateway();
        readonly OrderManager _orderManager=new OrderManager();
        public IEnumerable<ViewClient> GetTopClients()
        {
            return _reportGateway.GetTopClients();
        }
        public IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId)
        {
            return _reportGateway.GetTopClientsByBranchId(branchId);
        }

        public IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year)
        {
            return _reportGateway.GetTopClientsByBranchIdAndYear(branchId, year);
        }
        
        public IEnumerable<ViewProduct> GetPopularBatteries()
        {
            return _reportGateway.GetPopularBatteries();
        }
        public IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId,int companyId)
        {
            return _reportGateway.GetPopularBatteriesByBranchAndCompanyId(branchId,companyId);
        }

        public ViewTotalOrder GetTotalOrderByBranchIdCompanyIdAndYear(int branchId,int companyId, int year)
        { 
           var totalOrders = _orderManager.GetTotalOrdersByBranchIdCompanyIdAndYear(branchId, companyId,year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January =totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February =totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
           };
            return order;
        }

        public ViewTotalOrder GetTotalOrdersByCompanyIdAndYear(int companyId, int year)
        {
            var totalOrders = _orderManager.GetTotalOrdersByCompanyIdAndYear(companyId, year).ToArray();

            ViewTotalOrder order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }
        public ViewTotalOrder GetTotalOrdersByYear(int year)
        {
            var totalOrders = _orderManager.GetTotalOrdersByYear(year).ToArray();

            var order = new ViewTotalOrder
            {
                January = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Jan"))?.Total,
                February = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Feb"))?.Total,
                March = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Mar"))?.Total,
                April = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Ap"))?.Total,
                May = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("May"))?.Total,
                June = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("June"))?.Total,
                July = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("July"))?.Total,
                August = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Aug"))?.Total,
                September = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Sep"))?.Total,
                October = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Oct"))?.Total,
                November = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Nov"))?.Total,
                December = totalOrders?.ToList().Find(n => n.MonthName.StartsWith("Dec"))?.Total
            };
            return order;
        }
    }
}