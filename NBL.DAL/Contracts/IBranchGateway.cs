using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.DAL.Contracts
{
    public interface IBranchGateway
    {
        Branch GetBranchById(int branchId);
        IEnumerable<ViewBranch> GetAll();
        int Save(Branch branch);
        int Update(Branch branch);
        IEnumerable<ViewAssignedRegion> GetAssignedRegionToBranchList();
        int GetMaxBranchSubSubSubAccountCode();
    }
}
