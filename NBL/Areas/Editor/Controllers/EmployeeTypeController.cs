using System.Web.Mvc;
using NblClassLibrary.BLL;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class EmployeeTypeController : Controller
    {

        readonly  EmployeeTypeManager _employeeTypeManager=new EmployeeTypeManager();
    
        public ActionResult EmployeeTypeList() 
        {
            return View(_employeeTypeManager.GetAll);
        }
    }
}