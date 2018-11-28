using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class DesignationController : Controller
    {
        readonly DesignationManager _designationManager=new DesignationManager();
        readonly DepartmentManager _departmentManager = new DepartmentManager();
        public ActionResult DesignationList()
        {
            return View(_designationManager.GetAll);
        }

        public ActionResult AddNewDesignation() 
        {
            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll, "DepartmentId", "DepartmentName");
            return View();
        }
        [HttpPost]
        public ActionResult AddNewDesignation(Designation model)
        {

            if (ModelState.IsValid)
            {
                if (_designationManager.Save(model))
                {
                    return RedirectToAction("DesignationList");
                }
                ViewBag.Message = "Failed to Save!";
                return View();
            }
            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll, "DepartmentId", "DepartmentName");
            return View();
        }

        public ActionResult Edit(int id)
        {
          
            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll, "DepartmentId", "DepartmentName");
            Designation aDesignation = _designationManager.GetDesignationById(id);
            return View(aDesignation);
        }
        [HttpPost]
        public ActionResult Edit(int id, Designation model) 
        {

            if (ModelState.IsValid)
            {
                Designation aDesignation = _designationManager.GetDesignationById(id);
                aDesignation.DepartmentId = model.DepartmentId;
                aDesignation.DesignationName = model.DesignationName;
                aDesignation.DesignationCode = model.DesignationCode;
                if (_designationManager.Update(aDesignation))
                {
                    return RedirectToAction("DesignationList");
                }
            }
            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll, "DepartmentId", "DepartmentName");
            return View(model);
        }

        public JsonResult DesignationCodeExists(string code)
        {

            Designation aDesignation = _designationManager.GetDesignationByCode(code);
            if (aDesignation.DesignationCode != null)
            {
                aDesignation.DesignationCodeInUse = true;
            }
            else
            {
                aDesignation.DesignationCodeInUse = false;
                aDesignation.DesignationCode = code;
            }
            return Json(aDesignation);
        }
    }
}