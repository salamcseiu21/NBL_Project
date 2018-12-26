using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
    public interface IBranchManager
    {
        Branch GetBranchById(int branchId);
        IEnumerable<ViewBranch> GetAll();
        bool Save(Branch branch);
        bool Update(Branch branch);
        List<ViewAssignedRegion> GetAssignedRegionToBranchList();
        SelectList GetBranchSelectList();

    }
}
