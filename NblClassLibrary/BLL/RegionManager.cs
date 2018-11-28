using System.Collections.Generic;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.BLL
{
  public class RegionManager
    {
        readonly RegionGateway _regionGateway=new RegionGateway();
        public IEnumerable<Region> GetAllRegion()
        {
            var regions = _regionGateway.GetAllRegion();
            return regions;
        }

        public int Save(Region aRegion)
        {
           return  _regionGateway.Save(aRegion);
        }

        public IEnumerable<ViewRegion> GetRegionListWithDistrictInfo()
        {
           return _regionGateway.GetRegionListWithDistrictInfo();
        }

        public ViewRegion GetRegionDetailsById(int regionDetailsId)
        {
            return _regionGateway.GetRegionDetailsById(regionDetailsId);
        }

        public int AssignDristrictToRegion(Region aRegion)
        {
            return _regionGateway.AssignDristrictToRegion(aRegion);
        }

        public IEnumerable<Region> GetUnAssignedRegionList()
        {
            return _regionGateway.GetUnAssignedRegionList();
        }

        public int AssignRegionToBranch(Branch branch, User user)
        {
            return _regionGateway.AssignRegionToBranch(branch, user);
        }

        public List<Region> GetAssignedRegionListToBranchByBranchId(int branchId)
        {

            return _regionGateway.GetAssignedRegionListToBranchByBranchId(branchId);
        }
        public Branch GetBranchInformationByRegionId(int regionId)
        {

            return _regionGateway.GetBranchInformationByRegionId(regionId);
        }

        public int UnAssignDistrictFromRegion(ViewRegion regionDetails, string reason, User user)
        {
          return  _regionGateway.UnAssignDistrictFromRegion(regionDetails, reason, user);
        }
    }
}
