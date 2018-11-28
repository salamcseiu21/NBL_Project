using System;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class DepartmentController : Controller
    {
        
        readonly  DepartmentManager _departmentManager=new DepartmentManager();
        public ActionResult DepartmentList()
        {
            return View(_departmentManager.GetAll);
        }
        [HttpGet]
        public ActionResult AddNewDepartment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewDepartment(Department model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string result = _departmentManager.Save(model);
                    TempData["Message"] = result;
                }
               
                return View();

            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message+"</br>System Error : "+exception?.InnerException?.Message;
                return View();
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Department aDepartment = _departmentManager.GetDepartmentById(id);
            return View(aDepartment);
        }
        [HttpPost]
        public ActionResult Edit(int id,Department model)
        {
            Department aDepartment = _departmentManager.GetDepartmentById(id);
            try
            {
                if (ModelState.IsValid)
                {
                    aDepartment.DepartmentCode = model.DepartmentCode;
                    aDepartment.DepartmentName = model.DepartmentName;
                    string result = _departmentManager.Update(aDepartment);
                }
                
                return RedirectToAction("DepartmentList");

            }
            catch (Exception exception)
            {
                TempData["Error"] = exception.Message + "<br>System Error:" + exception?.InnerException?.Message;
                return View(aDepartment);
            }
        }


        public JsonResult DepartmentCodeExists(string code)
        {

            Department aDepartment = _departmentManager.GetDepartmentByCode(code);
            if (aDepartment.DepartmentCode!= null)
            {
                aDepartment.DepartmentCodeInUse = true;
            }
            else
            {
                aDepartment.DepartmentCodeInUse = false;
                aDepartment.DepartmentCode = code;
            }
            return Json(aDepartment);
        }
    }
}