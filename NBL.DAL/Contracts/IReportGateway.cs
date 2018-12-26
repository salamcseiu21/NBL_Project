using System.Collections.Generic;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
   public interface IReportGateway
   {
       IEnumerable<ViewClient> GetTopClients();

       IEnumerable<ViewClient> GetTopClientsByBranchId(int branchId);

       IEnumerable<ViewClient> GetTopClientsByBranchIdAndYear(int branchId, int year);

       IEnumerable<ViewProduct> GetPopularBatteries();

       IEnumerable<ViewProduct> GetPopularBatteriesByBranchAndCompanyId(int branchId, int companyId);

   }
}
