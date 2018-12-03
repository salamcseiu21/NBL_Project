using System.Collections.Generic;
using System.Linq;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NblClassLibrary.BLL
{
  public class RegionManager
    {
        readonly RegionGateway _regionGateway=new RegionGateway();
        readonly TerritoryManager _territoryManager =new TerritoryManager();
        readonly DistrictGateway _districtGateway=new DistrictGateway();
        public IEnumerable<Region> GetAllRegion()
        {
            var regions = _regionGateway.GetAllRegion();
            foreach (var region in regions)
            {
                region.Territories = _territoryManager.GetTerritoryListByRegionId(region.RegionId).ToList();
            }
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

            var regions = _regionGateway.GetAssignedRegionListToBranchByBranchId(branchId).ToList();
            foreach (var region in regions)
            {
                region.Territories = _territoryManager.GetTerritoryListByRegionId(region.RegionId).ToList();
                region.Districts = _districtGateway.GetAllDistrictByRegionId(region.RegionId).ToList();
            }
            return regions;
        }
        public Branch GetBranchInformationByRegionId(int regionId)
        {

            return _regionGateway.GetBranchInformationByRegionId(regionId);
        }

        public int UnAssignDistrictFromRegion(ViewRegion regionDetails, string reason, ViewUser user)
        {
          return  _regionGateway.UnAssignDistrictFromRegion(regionDetails, reason, user);
        }
    }
}
