
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IBranchGateway:IGateway<Branch>
    {
        IEnumerable<ViewBranch> GetAllBranches();
        IEnumerable<ViewAssignedRegion> GetAssignedRegionToBranchList();
        int GetMaxBranchSubSubSubAccountCode();
    }
}
