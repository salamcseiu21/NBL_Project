
using System;
using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class TerritoryController : Controller
    {
        readonly TerritoryGateway _territoryGateway = new TerritoryGateway();
        readonly RegionGateway _regionGateway = new RegionGateway();
        readonly UpazillaGateway _upazillaGateway=new UpazillaGateway();
        // GET: Editor/Territory
        public ActionResult All()
        {
            var territories = _territoryGateway.GetAllTerritory();
            foreach (Territory territory in territories)
            {
                territory.UpazillaList = _upazillaGateway.GetAssignedUpazillaLsitByTerritoryId(territory.TerritoryId)
                    .ToList();
            }
            return View(territories);
        }

        public ActionResult AddNewTerritory()
        {
            var regions = _regionGateway.GetAllRegion();
            return View(regions);
        }
        [HttpPost]
        public ActionResult AddNewTerritory(FormCollection collection)
        {
            var regions = _regionGateway.GetAllRegion();

            try
            {
                int regigionId = Convert.ToInt32(collection["RegionId"]);
                string territoryName = collection["TerritoryName"].ToString();
                User user = (User)Session["user"];

                Territory aTerritory = new Territory
                {
                    RegionId = regigionId,
                    TerritoryName = territoryName,
                    AddedByUserId=user.UserId
                };

                int rowAffected = _territoryGateway.Save(aTerritory);
                if (rowAffected > 0)
                {
                    return RedirectToAction("All");
                }
                return View(regions);
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                return View(regions);
            }


        }
       
    }
}