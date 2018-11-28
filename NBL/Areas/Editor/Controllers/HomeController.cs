using System.Linq;
using System.Web.Mvc;
using NblClassLibrary.BLL;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class HomeController : Controller
    {

        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly ProductManager _productManager = new ProductManager();
        // GET: Editor/Home
        public ActionResult Home() 
        {

            return View();
        }

        public ActionResult ViewClient()
        {
            var clients = _clientManager.GetAll.ToList();
            return View(clients);

        }

        public ActionResult ViewEmployee()
        {
            var employees = _employeeManager.GetAllEmployeeWithFullInfo().ToList();
            return View(employees);

        }

        public ActionResult ViewProduct()
        {
            var products = _productManager.GetAll.ToList();
            return View(products);

        }

        public ActionResult BusinessArea()
        {
            return View();
        }
    }
}