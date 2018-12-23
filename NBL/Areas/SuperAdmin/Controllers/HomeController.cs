using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NBL.Areas.SuperAdmin.BLL;
using System.Web.Helpers;
using NBL.Areas.Accounts.BLL;
using NBL.BLL;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles ="Super")]
    public class HomeController : Controller
    {
        // GET: SuperAdmin/Home

        readonly ClientManager _clientManager=new ClientManager();
        readonly EmployeeManager _employeeManager=new EmployeeManager();
        readonly ProductManager _productManager=new ProductManager();
        readonly BranchManager _branchManager=new BranchManager();
        readonly UserManager _userManager=new UserManager();
        readonly OrderManager _orderManager=new OrderManager();
        readonly CommonGateway _commonGateway = new CommonGateway();
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        readonly RegionManager _regionManager=new RegionManager();
        readonly TerritoryManager _territoryManager=new TerritoryManager();
        readonly SuperAdminUserManager _superAdminUserManager = new SuperAdminUserManager();
        readonly AccountsManager _accountsManager=new AccountsManager();
        readonly VatManager _vatManager=new VatManager();
        readonly ReportManager _reportManager=new ReportManager();
        public ActionResult Home()
        {
            Session["BranchId"] = null;
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var branches = _branchManager.GetAll();
            ViewTotalOrder totalOrder = _reportManager.GetTotalOrdersByCompanyIdAndYear(companyId,DateTime.Now.Year);
            var sales = _accountsManager.GetTotalSaleValueOfCurrentMonthByCompanyId(companyId)* -1;
            var collection = _accountsManager.GetTotalCollectionOfCurrentMonthByCompanyId(companyId);
            var orderedAmount = _accountsManager.GetTotalOrderedAmountOfCurrentMonthByCompanyId(companyId);
            var clients = _reportManager.GetTopClients().ToList();
            var batteries = _reportManager.GetPopularBatteries().ToList();

            SummaryModel summary = new SummaryModel
            {
                Branches = branches.ToList(),
                CompanyId = companyId,
                TotalOrder = totalOrder,
                TotalSale = sales,
                TotalCollection = collection,
                OrderedAmount = orderedAmount,
                Clients = clients,
                Products = batteries

            };
            return View(summary); 

        }
        public PartialViewResult ViewClient() 
        {
            var clients = _clientManager.GetAll.ToList();
            return PartialView("_ViewClientPartialPage",clients);

        }
        public PartialViewResult ViewClientProfile(int id)
        {
            var client = _clientManager.GetClientDeailsById(id);
            return PartialView("_ViewClientProfilePartialPage",client);

        }
        public PartialViewResult ViewEmployee()
        {
            var employees = _employeeManager.GetAllEmployeeWithFullInfo().ToList();
            return PartialView("_ViewEmployeePartialPage",employees);

        }
        public PartialViewResult ViewEmployeeProfile(int id)
        {
            var employee = _employeeManager.GetEmployeeById(id);
            return PartialView("_ViewEmployeeProfilePartialPage",employee);

        }
        public PartialViewResult ViewProduct() 
        {
            var products = _productManager.GetAll.ToList();
            return PartialView("_ViewProductPartialPage", products);

        }
        public PartialViewResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return PartialView("_ViewBranchPartialPage", branches);
        }
        public ActionResult ViewUserDetails(int userId)
        {
            var userById = _userManager.GetAll.ToList().Find(n => n.UserId == userId);
            var branches = _superAdminUserManager.GetAssignedBranchByUserId(userId);
            ViewBag.BranchList = branches;
            return View(userById);
        }
        public PartialViewResult AllOrders()
        {
            var companyId = Convert.ToInt32(Session["CompanyId"]);
            var orders = _orderManager.GetOrdersByCompanyId(companyId).ToList();
            ViewBag.Heading = "All Orders";
            return PartialView("_ViewOrdersPartialPage",orders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _orderManager.GetOrderByOrderId(id);
            return View(order);
        }
       
        public PartialViewResult ViewDivision() 
        {
            var divisions = _divisionGateway.GetAll.ToList();
            return PartialView("_ViewDivisionPartialPage",divisions);

        }
        public PartialViewResult ViewRegion()
        {
            var regions = _regionManager.GetAllRegion().ToList();
            return PartialView("_ViewRegionPartialPage",regions);
        }
        public PartialViewResult ViewTerritory()
        {
            var territories = _territoryManager.GetAllTerritory().ToList();
            return PartialView("_ViewTerritoryPartialPage",territories);

        }
        public PartialViewResult Supplier()
        {
            var suppliers = _commonGateway.GetAllSupplier().ToList();
            return PartialView("_ViewSupplierPartialPage",suppliers);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            var roles = _commonGateway.GetAllUserRoles.ToList();
            ViewBag.Roles = roles;
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            try
            {
                var empId = Convert.ToInt32(collection["EmployeeId"]);
                var uName = collection["UserName"];
                var pass = collection["ConfirmPassword"];
                var roleId = Convert.ToInt32(collection["RoleId"]);
                User anUser = new User
                {
                    EmployeeId = empId,
                    UserName = uName,
                    Password = pass,
                    UserRoleId = roleId,
                    AddedByUserId = ((ViewUser) Session["user"]).UserId
                };
                string result = _userManager.AddNewUser(anUser);
                TempData["Message"] = result;
                var roles = _commonGateway.GetAllUserRoles.ToList().OrderBy(n => n.RoleName);
                ViewBag.Roles = roles;
                return View();
            }
            catch (Exception e)
            {

                TempData["Error"] = e.Message+"</br>System Error:"+ e.InnerException?.Message;
                var roles = _commonGateway.GetAllUserRoles.ToList().OrderBy(n => n.RoleName);
                ViewBag.Roles = roles;
                return View();
            }
        }

    

        public JsonResult EmployeeAutoComplete(string prefix)
        {

            var employeeNameList = (from e in _employeeManager.GetAll.ToList()
                              where e.EmployeeName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = e.EmployeeName,
                                  val = e.EmployeeId
                              }).ToList();

            return Json(employeeNameList);
        }

        public JsonResult UserAutoComplete(string prefix) 
        {

            var userNameList = (from e in _userManager.GetAllUserForAutoComplete().ToList() 
                              where e.UserName.ToLower().Contains(prefix.ToLower())
                              select new
                              {
                                  label = e.UserName,
                                  val = e.UserId
                              }).ToList();

            return Json(userNameList);
        }
        public JsonResult UserNameExists(string userName)
        {

           var user= _userManager.GetUserByUserName(userName);
            if(user.UserName !=null)
            {
                user.UserNameInUse = true; 
            }
            else
            {
                user.UserNameInUse = false;
                user.UserName = userName;
            }
            return Json(user);
        }
        public ActionResult MyChart()
        {
            new Chart(width: 900, height: 400).AddSeries(chartType: "Column",
                   xValue: new[] { "Jan", "Feb", "Mar", "April", "May", "June", "July" },
                   yValues: new[] { 1000, 5000, 4000, 9000, 3000, 6000, 7000 }
                   ).Write("png");
            return null;
        }

        public JsonResult GetPendingOrders()
        {
            int branchId = Convert.ToInt32(Session["BranchId"]);
            int companyId = Convert.ToInt32(Session["CompanyId"]);
            var stock = 500;
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Test()
        {
            
            List<Order> model = _orderManager.GetAll.ToList();
            return View(model);
        }
       
        public PartialViewResult OrderSummary(int branchId)
        {
            List<Order> model = _orderManager.GetOrdersByBranchId(branchId).ToList();
            return PartialView("_ViewOrdersPartialPage", model);
        }
        public PartialViewResult All()
        {
            List<Order> model = _orderManager.GetAll.ToList();
            return PartialView("_ViewOrdersPartialPage", model);
        }

        public PartialViewResult Vouchers()
        {
            var vouchers = _accountsManager.GetVoucherList();
            return PartialView("_ViewVouchersPartialPage",vouchers);
        }

        public PartialViewResult VoucherPreview(int id)
        {
            var voucher = _accountsManager.GetVoucherByVoucherId(id);
            var voucherDetails = _accountsManager.GetVoucherDetailsByVoucherId(id);
            ViewBag.VoucherDetails = voucherDetails;
            return PartialView("_VoucherPreviewPartialPage",voucher);
        }

        public PartialViewResult ProductWishVat()
        {
            IEnumerable<Vat> vats = _vatManager.GetProductWishVat();
            return PartialView("_ViewProductWishVatPartialPage", vats);
        }


        public ActionResult ViewStatus()
        {
            return View(_commonGateway.GetAllStatus());
        }

        public ActionResult ViewSubReference()
        {
            return View(_commonGateway.GetAllSubReferenceAccounts());
        }
    }

}
