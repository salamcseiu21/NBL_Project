
using System;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class TerritoryController : Controller
    {

        readonly TerritoryManager _territoryManager=new TerritoryManager();
        readonly RegionManager _regionManager=new RegionManager();
        // GET: Editor/Territory
        public ActionResult All()
        {
            var territories = _territoryManager.GetAllTerritory();
            return View(territories);
        }

        public ActionResult AddNewTerritory()
        {
            ViewBag.RegionId = new SelectList(_regionManager.GetAllRegion(), "RegionId", "RegionName");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewTerritory(Territory model)
        {
            ViewBag.RegionId = new SelectList(_regionManager.GetAllRegion(), "RegionId", "RegionName");
            try
            {
                if (ModelState.IsValid)
                {
                    User user = (User)Session["user"];
                    model.AddedByUserId = user.UserId;
                    int rowAffected = _territoryManager.Save(model);
                    if (rowAffected > 0)
                    {
                        ModelState.Clear();
                        return RedirectToAction("All");
                    }
                }
                return View();
            }
            catch (Exception exception)
            {
                string message = exception.Message;
                return View();
            }
        }
       
    }
}