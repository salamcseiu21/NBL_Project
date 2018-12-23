using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class HomeController : Controller
    {

        readonly ClientManager _clientManager = new ClientManager();
        readonly EmployeeManager _employeeManager = new EmployeeManager();
        readonly ProductManager _productManager = new ProductManager();
        readonly DepartmentManager _departmentManager=new DepartmentManager();
        readonly BranchManager _branchManager=new BranchManager();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        // GET: Editor/Home
        public ActionResult Home() 
        {
            SummaryModel model = new SummaryModel
            {
                Clients = _clientManager.GetAllClientDetails().ToList(),
                Employees = _employeeManager.GetAllEmployeeWithFullInfo(),
                Departments = _departmentManager.GetAll,
                Branches = _branchManager.GetAll(),
                Regions = _regionManager.GetAllRegion(),
                Territories = _territoryManager.GetAllTerritory()
            };
            return View(model);
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