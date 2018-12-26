using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IReportManager
    {
       
        IEnumerable<ViewClient> GetTopClients();
        

        IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId);
      

        IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year);
        

        IEnumerable<ViewProduct> GetPopularBatteries();
      

        IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId, int companyId);
        

        ViewTotalOrder GetTotalOrderByBranchIdCompanyIdAndYear(int branchId, int companyId, int year);
  

        ViewTotalOrder GetTotalOrdersByCompanyIdAndYear(int companyId, int year);
        
        ViewTotalOrder GetTotalOrdersByYear(int year);

    }
}
