﻿using NBL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.Models.ViewModels;
using NBL.DAL;

namespace NBL.BLL
{
    public class BranchManager
    {
        readonly BranchGateway _branchGateway = new BranchGateway();
        public Branch GetBranchById(int branchId)
        {
            return _branchGateway.GetBranchById(branchId);
        }

        public IEnumerable<ViewBranch> GetAll()
        {
            var branches = _branchGateway.GetAll().ToList();
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