using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

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
    }
}
