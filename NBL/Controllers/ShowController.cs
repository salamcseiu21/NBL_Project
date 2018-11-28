using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;
namespace NBL.Controllers
{
    [Authorize]
    public class ShowController : Controller
    {
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        public ActionResult ViewEmployee()
        {
            var employees = _employeeManager.GetAllEmployeeWithFullInfo().ToList();
            return View(employees);
        }
        public ActionResult EmployeeProfile(int id)
        {
            var anEmployee = _employeeManager.GetEmployeeById(id);
            return View(anEmployee);
        }
    }
}