using System.Collections.Generic;
using System.Linq;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.BLL
{
    public class BranchManager
    {
        readonly BranchGateway _branchGateway = new BranchGateway();
        readonly  ClientManager _clientManager=new ClientManager();
        readonly RegionGateway _regionGateway=new RegionGateway();
        public Branch GetBranchById(int branchId)
        {
            return _branchGateway.GetBranchById(branchId);
        }

        public IEnumerable<ViewBranch> GetAll()
        {
            var branches = _branchGateway.GetAll().ToList();
            foreach (var branch in branches)
            {
                branch.Clients = _clientManager.GetClientByBranchId(branch.BranchId).ToList();
            }
            foreach (ViewBranch branch in branches)
            {
                branch.RegionList = _regionGateway.GetAssignedRegionListToBranchByBranchId(branch.BranchId);
            }
            return branches;
        }

        public bool Save(Branch branch)
        {
            branch.SubSubSubAccountCode = GenerateSubSubSubAccount("1101");
            int rowAffected = _branchGateway.Save(branch);
            return rowAffected > 0;
        }

        private string GenerateSubSubSubAccount(string prefix)
        {
            int maxSlNo = _branchGateway.GetMaxBranchSubSubSubAccountCode();
            return prefix + (maxSlNo+1);
        }

        public bool Update(Branch branch)
        {
            int rowAffected = _branchGateway.Update(branch);
            return rowAffected > 0;
        }

        public List<ViewAssignedRegion> GetAssignedRegionToBranchList()
        {
           
            var items= _branchGateway.GetAssignedRegionToBranchList();
            return items.ToList();
        }

     
    }
}