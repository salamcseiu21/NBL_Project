using NBL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models.ViewModels;
using NBL.DAL.Contracts;

namespace NBL.BLL
{
    public class BranchManager:IBranchManager
    {

        readonly IBranchGateway _iBranchGateway;
        public BranchManager(IBranchGateway iBranchGateway)
        {
            _iBranchGateway = iBranchGateway;
        }
       
        public Branch GetBranchById(int branchId)
        {
            return _iBranchGateway.GetBranchById(branchId);
        }

        public IEnumerable<ViewBranch> GetAll()
        {
            var branches = _iBranchGateway.GetAll().ToList();
            return branches;
        }

        public bool Save(Branch branch)
        {
            branch.SubSubSubAccountCode = GenerateSubSubSubAccount("1101");
            int rowAffected = _iBranchGateway.Save(branch);
            return rowAffected > 0;
        }

        private string GenerateSubSubSubAccount(string prefix)
        {
            int maxSlNo = _iBranchGateway.GetMaxBranchSubSubSubAccountCode();
            return prefix + (maxSlNo+1);
        }

        public bool Update(Branch branch)
        {
            int rowAffected = _iBranchGateway.Update(branch);
            return rowAffected > 0;
        }

        public List<ViewAssignedRegion> GetAssignedRegionToBranchList()
        {
           
            var items= _iBranchGateway.GetAssignedRegionToBranchList();
            return items.ToList();
        }

        public SelectList GetBranchSelectList()
        {
            var branches = from branch in GetAll().ToList().Where(i => !i.BranchName.Contains("Corporate"))
                select new Branch
                {
                    BranchId = branch.BranchId,
                    BranchName = branch.BranchName,
                    BranchAddress = branch.BranchAddress
                };

            return new SelectList(branches, "BranchId", "BranchName");
        }

    }
}