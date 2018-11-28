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
    }
}
