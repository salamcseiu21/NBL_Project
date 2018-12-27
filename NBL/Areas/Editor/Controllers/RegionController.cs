using System;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class RegionController : Controller
    {
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly IRegionManager _iRegionManager;
        // GET: Editor/Region
        public RegionController(IRegionManager iRegionManager)
        {
            _iRegionManager = iRegionManager;
        }
        public ActionResult All()
        {
            var regions = _iRegionManager.GetAll();
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
                    bool rowAffected = _iRegionManager.Add(model);
                    if (rowAffected)
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