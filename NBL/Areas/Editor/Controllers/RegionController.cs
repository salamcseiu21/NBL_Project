using System;
using System.Web.Mvc;
using NBL.BLL;
using NBL.DAL;
using NBL.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class RegionController : Controller
    {
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly RegionManager _regionManager=new RegionManager();
        // GET: Editor/Region

        public ActionResult All()
        {
            var regions = _regionManager.GetAllRegion();
            return View(regions);
        }
        public ActionResult AddNewRegion()
        {
            ViewBag.DivisionId = new SelectList(_divisionGateway.GetAll, "DivisionId", "DivisionName");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewRegion(Region model)
        {
            ViewBag.DivisionId = new SelectList(_divisionGateway.GetAll, "DivisionId", "DivisionName");

            try
            {
                if (ModelState.IsValid)
                {
                    int rowAffected = _regionManager.Save(model);
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