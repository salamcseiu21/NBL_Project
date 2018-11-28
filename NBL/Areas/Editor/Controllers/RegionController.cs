using System;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class RegionController : Controller
    {

        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly DistrictGateway _districtGateway = new DistrictGateway();
        readonly RegionGateway _regionGateway = new RegionGateway();
        readonly TerritoryGateway _territoryGateway=new TerritoryGateway();
        // GET: Editor/Region

        public ActionResult All()
        {
            var regions = _regionGateway.GetAllRegion();
            foreach (Region region in regions)
            {
                region.Territories = _territoryGateway.GetTerritoryListByRegionId(region.RegionId).ToList();
            }
            return View(regions);
        }
        public ActionResult AddNewRegion()
        {
            var divisions = _divisionGateway.GetAll;
            return View(divisions);
        }
        [HttpPost]
        public ActionResult AddNewRegion(FormCollection collection)
        {
            var divisions = _divisionGateway.GetAll;
          
            try
            {
                int divisiionId = Convert.ToInt32(collection["DivisionId"]);
                string regionName = collection["RegionName"].ToString();
                Region aRegion = new Region
                {
                    DivisionId = divisiionId,
                    RegionName = regionName
                };

                int rowAffected=_regionGateway.Save(aRegion);
                if(rowAffected>0)
                {
                    return RedirectToAction("All");
                }
                return View(divisions);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                return View(divisions);
            }

            
        }
       
    }
}