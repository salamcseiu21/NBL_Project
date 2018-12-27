﻿
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL.Contracts
{
   public interface IRegionManager:IManager<Region>
   {
       IEnumerable<ViewRegion> GetRegionListWithDistrictInfo();
       ViewRegion GetRegionDetailsById(int regionDetailsId);
       int AssignDristrictToRegion(Region aRegion);
       IEnumerable<Region> GetUnAssignedRegionList();
       int AssignRegionToBranch(Branch branch, User user);
       List<Region> GetAssignedRegionListToBranchByBranchId(int branchId);
       Branch GetBranchInformationByRegionId(int regionId);
       int UnAssignDistrictFromRegion(ViewRegion regionDetails, string reason, ViewUser user);
   }
}
