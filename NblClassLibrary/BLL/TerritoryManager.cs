using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NblClassLibrary.BLL
{
   public class TerritoryManager
    {
        readonly  TerritoryGateway _territoryGateway=new TerritoryGateway();
        public IEnumerable<Territory> GetAllTerritory()
        {
            return _territoryGateway.GetAllTerritory();
        }

        public int Save(Territory aTerritory)
        {
           return _territoryGateway.Save(aTerritory);
        }

        public IEnumerable<Territory> GetTerritoryListByBranchId(int branchId)
        {
            return _territoryGateway.GetTerritoryListByBranchId(branchId);
        }
        public IEnumerable<Territory> GetTerritoryListByRegionId(int regionId)
        {
            return _territoryGateway.GetTerritoryListByRegionId(regionId);
        }

        public int AssignUpazillaToTerritory(Territory aTerritory)
        {
            return _territoryGateway.AssignUpazillaToTerritory(aTerritory);
        }

        public int UnAssignUpazillaFromTerritory(int territoryDetailsId, string reason, User user)
        {
            return _territoryGateway.UnAssignUpazillaFromTerritory(territoryDetailsId, reason, user);
        }
    }
}
