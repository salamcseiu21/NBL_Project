using System.Collections.Generic;
using System.Linq;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.BLL
{
   public class TerritoryManager:ITerritoryManager
    {
       private readonly  TerritoryGateway _territoryGateway=new TerritoryGateway();
       private readonly UpazillaGateway _upazillaGateway = new UpazillaGateway();
        public IEnumerable<Territory> GetAllTerritory()
        {
            var territories = _territoryGateway.GetAllTerritory();
            foreach (var territory in territories)
            {
                territory.UpazillaList = _upazillaGateway.GetAssignedUpazillaLsitByTerritoryId(territory.TerritoryId)
                    .ToList();
            }
            return territories;
        }

        public int Save(Territory aTerritory)
        {
           return _territoryGateway.Save(aTerritory);
        }

        public IEnumerable<Territory> GetTerritoryListByBranchId(int branchId)
        {
            var territories = _territoryGateway.GetTerritoryListByBranchId(branchId);
            foreach (var territory in territories)
            {
                territory.UpazillaList = _upazillaGateway.GetAssignedUpazillaLsitByTerritoryId(territory.TerritoryId)
                    .ToList();
            }
            return territories;
        }
        public IEnumerable<Territory> GetTerritoryListByRegionId(int regionId)
        {
            return _territoryGateway.GetTerritoryListByRegionId(regionId);
        }

        public int AssignUpazillaToTerritory(Territory aTerritory)
        {
            return _territoryGateway.AssignUpazillaToTerritory(aTerritory);
        }

        public int UnAssignUpazillaFromTerritory(int territoryDetailsId, string reason, ViewUser user)
        {
            return _territoryGateway.UnAssignUpazillaFromTerritory(territoryDetailsId, reason, user);
        }
    }
}
