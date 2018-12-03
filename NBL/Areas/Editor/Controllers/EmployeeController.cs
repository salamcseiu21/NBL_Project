using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.DAL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class EmployeeController : Controller
    {
        readonly EmployeeManager _employeeManager=new EmployeeManager();
        readonly DesignationManager _designationManager=new DesignationManager();
        readonly DepartmentManager _departmentManager=new DepartmentManager();
        readonly EmployeeTypeManager _employeeTypeManager=new EmployeeTypeManager();
        readonly BranchManager _branchManager=new BranchManager();
        public ActionResult AddEmployee() 
        {
            var departments = _departmentManager.GetAll;
            var designations = _designationManager.GetAll;
            var empTypes = _employeeTypeManager.GetAll;
            var branches = _branchManager.GetAll();
            ViewBag.EmployeeTypes = empTypes;
            ViewBag.Designations = designations;
            ViewBag.Departments = departments;
            ViewBag.Branches = branches;
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(FormCollection collection,HttpPostedFileBase EmployeeImage,HttpPostedFileBase EmployeeSignature)
        {
            try
            {

                var user = (ViewUser)Session["user"];
                Employee anEmployee = new Employee
                {
                    EmployeeTypeId = Convert.ToInt32(collection["EmployeeTypeId"]),
                    DesignationId = Convert.ToInt32(collection["DesignationId"]),
                    DepartmentId = Convert.ToInt32(collection["DepartmentId"]),
                    BranchId = Convert.ToInt32(collection["BranchId"]),
                    EmployeeName = collection["EmployeeName"],
                    PresentAddress = collection["PresentAddress"],
                    Phone = collection["Phone"],
                    AlternatePhone = collection["AlternatePhone"],
                    Gender = collection["Gender"],
                    Email = collection["Email"],
                    NationalIdNo = collection["NationalIdNo"],
                    JoiningDate = Convert.ToDateTime(collection["JoiningDate"]),
                    UserId = user.UserId 
                };
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

                string result = _employeeManager.Save(anEmployee);
                TempData["Message"] = result;
                if(result.Contains("successfully"))
                return RedirectToAction("ViewEmployee", "Home");
                var departments = _departmentManager.GetAll;
                var designations = _designationManager.GetAll;
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _branchManager.GetAll();
                ViewBag.EmployeeTypes = empTypes;
                ViewBag.Designations = designations;
                ViewBag.Departments = departments;
                ViewBag.Branches = branches;
                return View();
            }
            catch (Exception e)
            {

                var departments = _departmentManager.GetAll;
                var designations = _designationManager.GetAll;
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _branchManager.GetAll();
                ViewBag.EmployeeTypes = empTypes;
                ViewBag.Designations = designations;
                ViewBag.Departments = departments;
                ViewBag.Branches = branches;
                TempData["Error"] = e.Message;
                return View();
              }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Employee employee = _employeeManager.EmployeeById(id);
            var departments = _departmentManager.GetAll;
            var designations = _designationManager.GetAll;
            var empTypes = _employeeTypeManager.GetAll;
            var branches = _branchManager.GetAll();
            ViewBag.EmployeeTypes = empTypes;
            ViewBag.Designations = designations;
            ViewBag.Departments = departments;
            ViewBag.Branches = branches;
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection, HttpPostedFileBase EmployeeImage,HttpPostedFileBase EmployeeSignature)
        {
            try
            {
                var user = (ViewUser)Session["user"];
                var anEmployee = _employeeManager.EmployeeById(id);
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

                string result = _employeeManager.Update(anEmployee);
                TempData["Message"] = result;
                if (result.Contains("successfully"))
                    return RedirectToAction("ViewEmployee", "Home");
                var departments = _departmentManager.GetAll;
                var designations = _designationManager.GetAll;
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _branchManager.GetAll();
                ViewBag.EmployeeTypes = empTypes;
                ViewBag.Designations = designations;
                ViewBag.Departments = departments;
                ViewBag.Branches = branches;
                return View();

            }
            catch (Exception e)
            {
                Employee employee = _employeeManager.EmployeeById(id);
                var departments = _departmentManager.GetAll;
                var designations = _designationManager.GetAll;
                var empTypes = _employeeTypeManager.GetAll;
                var branches = _branchManager.GetAll();
                ViewBag.EmployeeTypes = empTypes;
                ViewBag.Designations = designations;
                ViewBag.Departments = departments;
                ViewBag.Branches = branches;
                TempData["Error"] = e.Message;
                return View(employee);
            }
        }

        public ActionResult Details(int id)
        {
            ViewEmployee employee = _employeeManager.GetEmployeeById(id);
            return View(employee);
        }
    }
}