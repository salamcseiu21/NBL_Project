using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class EmployeeController : Controller
    {
        readonly IEmployeeManager _iEmployeeManager;
        readonly IDesignationManager _iDesignationManager;
        readonly IDepartmentManager _iDepartmentManager;
        readonly EmployeeTypeManager _employeeTypeManager=new EmployeeTypeManager();
        readonly IBranchManager _iBranchManager;

        public EmployeeController(IBranchManager iBranchManager,IDepartmentManager iDepartmentManager,IDesignationManager iDesignationManager,IEmployeeManager iEmployeeManager)
        {
            _iBranchManager = iBranchManager;
            _iDepartmentManager = iDepartmentManager;
            _iDesignationManager = iDesignationManager;
            _iEmployeeManager = iEmployeeManager;
        }
        public ActionResult AddEmployee() 
        {
            var departments = _iDepartmentManager.GetAll();
            var designations = _iDesignationManager.GetAll();
            var empTypes = _employeeTypeManager.GetAll;
            var branches = _iBranchManager.GetAll();
            ViewBag.EmployeeTypes = empTypes;
            ViewBag.Designations = designations;
            ViewBag.Departments = departments;
            ViewBag.Branches = branches;

            ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
            ViewBag.BranchId=new SelectList(branches,"BranchId","BranchName");
            ViewBag.DepartmentId =new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.DesignationId = new SelectList(new List<Designation>(), "DesignationId", "DesignationName");
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(Employee anEmployee, HttpPostedFileBase EmployeeImage,HttpPostedFileBase EmployeeSignature)
        {
            try
            {

                var user = (ViewUser)Session["user"];
                anEmployee.UserId = user.UserId;
                if (EmployeeImage != null)
                {
                    string ext = Path.GetExtension(EmployeeImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Employees"), image);
                    // file is uploaded
                    EmployeeImage.SaveAs(path);
                    anEmployee.EmployeeImage = "Images/Employees/" + image;
                }
                if (EmployeeSignature != null)
                {
                    string ext = Path.GetExtension(EmployeeSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Signatures"), sign);
                    // file is uploaded
                    EmployeeSignature.SaveAs(path);
                    anEmployee.EmployeeSignature = "Images/Signatures/"+sign;
                }

                string result = _iEmployeeManager.Save(anEmployee);
                TempData["Message"] = result;
                //if(result.Contains("successfully"))
               // return RedirectToAction("ViewEmployee", "Home");
                var departments = _iDepartmentManager.GetAll();
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _iBranchManager.GetAll();
                ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(new List<Designation>(), "DesignationId", "DesignationName");
                return View();
            }
            catch (Exception e)
            {

                var departments = _iDepartmentManager.GetAll();
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _iBranchManager.GetAll();
                ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(new List<Designation>(), "DesignationId", "DesignationName");
                TempData["Error"] = e.Message;
                return View();
              }
        }


        public ActionResult GetAllEmployee()
        {
            var employees = _iEmployeeManager.GetAllEmployeeWithFullInfo();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = _iEmployeeManager.EmployeeById(id);
            var departments = _iDepartmentManager.GetAll();
            var empTypes = _employeeTypeManager.GetAll;
            var branches = _iBranchManager.GetAll();
            var designations = _iDesignationManager.GetAll();
            ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
            ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
            ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
            ViewBag.DesignationId = new SelectList(designations, "DesignationId", "DesignationName");

            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase EmployeeImage,HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _iEmployeeManager.EmployeeById(id);
                anEmployee.EmployeeTypeId = Convert.ToInt32(collection["EmployeeTypeId"]);
                anEmployee.DesignationId = Convert.ToInt32(collection["DesignationId"]);
                anEmployee.DepartmentId = Convert.ToInt32(collection["DepartmentId"]);
                anEmployee.BranchId = Convert.ToInt32(collection["BranchId"]);
                anEmployee.EmployeeName = collection["EmployeeName"];
                anEmployee.PresentAddress = collection["PresentAddress"];
                anEmployee.Phone = collection["Phone"];
                anEmployee.AlternatePhone = collection["AlternatePhone"];
                anEmployee.Gender = collection["Gender"];
                anEmployee.Email = collection["Email"];
                anEmployee.NationalIdNo = collection["NationalIdNo"];
                anEmployee.JoiningDate = Convert.ToDateTime(collection["JoiningDate"]);
                anEmployee.UserId = user.UserId;

                if (EmployeeImage != null)
                {
                    string ext = Path.GetExtension(EmployeeImage.FileName);
                    string image = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Employees"), image);
                    // file is uploaded
                    EmployeeImage.SaveAs(path);
                    anEmployee.EmployeeImage = "Images/Employees/" + image;
                }
                if (EmployeeSignature != null)
                {
                    string ext = Path.GetExtension(EmployeeSignature.FileName);
                    string sign = Guid.NewGuid().ToString().Replace("-", "").ToLower().Substring(2, 10) + ext;
                    string path = Path.Combine(
                        Server.MapPath("~/Images/Signatures"), sign);
                    // file is uploaded
                    EmployeeSignature.SaveAs(path);
                    anEmployee.EmployeeSignature = "Images/Signatures/" + sign;
                }

                string result = _iEmployeeManager.Update(anEmployee);
                TempData["Message"] = result;
                if (result.Contains("successfully"))
                    return RedirectToAction("ViewEmployee", "Home");
                var departments = _iDepartmentManager.GetAll();
                var designations = _iDesignationManager.GetAll();
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _iBranchManager.GetAll();
                ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(designations, "DesignationId", "DesignationName");
                return View();

            }
            catch (Exception exception)
            {
                Employee employee = _iEmployeeManager.EmployeeById(id);
                var departments = _iDepartmentManager.GetAll();
                var designations = _iDesignationManager.GetAll();
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _iBranchManager.GetAll();
                ViewBag.EmployeeTypeId = new SelectList(empTypes, "EmployeeTypeId", "EmployeeTypeName");
                ViewBag.BranchId = new SelectList(branches, "BranchId", "BranchName");
                ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.DesignationId = new SelectList(designations, "DesignationId", "DesignationName");
                TempData["Error"] = exception.Message;
                return View(employee);
            }
        }

        public ActionResult Details(int id)
        {
            ViewEmployee employee = _iEmployeeManager.GetEmployeeById(id);
            return View(employee);
        }
    }
}