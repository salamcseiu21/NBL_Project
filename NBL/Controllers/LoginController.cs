
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web.Mvc;
using System.Web.Security;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Controllers
{
    public class LoginController : Controller
    {
        readonly UserManager _userManager = new UserManager();
        readonly CompanyManager _companyManager = new CompanyManager();
        readonly CommonGateway _commonGateway = new CommonGateway();

        private readonly IBranchManager _iBranchManager;

        public LoginController(IBranchManager iBranchManager)
        {
            _iBranchManager = iBranchManager;
        }
        // GET: LogIn
        public ActionResult LogIn()
        {
            Session["user"] = null;
            Session["Branches"] = null;
            ViewBag.Companies = _companyManager.GetAll.ToList().OrderBy(n => n.CompanyId).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(FormCollection collection, string ReturnUrl)
        {
            User user = new User();
            string userName = collection["userName"];
            string password = collection["Password"];
            user.Password = password;
            user.UserName = userName;
            if (IsValid(user))
            {
                int companyId = Convert.ToInt32(collection["companyId"]);
                var company=_companyManager.GetCompanyById(companyId);

                Session["CompanyId"] = companyId;
                Session["Company"] = company;
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                var anUser = _userManager.GetUserByUserNameAndPassword(user.UserName, user.Password);
                anUser.IpAddress = GetLocalIPAddress();
                anUser.MacAddress = GetMacAddress().ToString();
                anUser.LogInDateTime = DateTime.Now;

                bool result = _userManager.ChangeLoginStatus(anUser, 1);

                Session["user"] = anUser;
                switch (anUser.Roles)
                {
                    case "Super":
                        return RedirectToAction("Home", "Home", new { area = "SuperAdmin" });
                    case "Factory":
                        return RedirectToAction("Home", "Home", new { area = "Factory" });
                    case "Editor":
                        return RedirectToAction("Home", "Home", new { area = "Editor" });
                    case "Corporate":
                        return RedirectToAction("Home", "Home", new { area = "Corporate" });
                    default:
                        return RedirectToAction("Goto", "LogIn", new { area = "" });
                }
                

            }
            ViewBag.Message = "Invalid Login";
            ViewBag.Companies = _companyManager.GetAll.ToList().OrderBy(n => n.CompanyId).ToList();
            return View();
        }

        private bool IsValid(User user)
        {
            bool isExits = false;
            var anUser=_userManager.GetUserByUserNameAndPassword(user.UserName,user.Password);
            if(anUser.UserName !=null)
            {
                Session["Branches"] = _commonGateway.GetAssignedBranchesToUserByUserId(anUser.UserId).ToList();
                Session["user"] = anUser;
                isExits = true;
            }
            return (isExits);
        }
        public ActionResult GoTo()
        {
            var user = Session["user"];
            if (user != null)
            {
                Session["user"] = user;
                return View();
            }
            return RedirectToAction("LogIn", "LogIn", new { area = "" });
        }
        [HttpPost]
        public ActionResult GoTo(FormCollection collection)
        {
            int branchId = Convert.ToInt32(collection["BranchId"]);
            var branch= _iBranchManager.GetBranchById(branchId);
            Session["BranchId"] = branchId;
            Session["Branch"] = branch;
            var user = (ViewUser)Session["user"];
            switch (user.Roles)
            {
                case "Admin":
                    return RedirectToAction("Home", "Home", new { area = "Admin" });
                   
                case "User":
                    return RedirectToAction("Home", "Home", new { area = "Sales" });
                case "Super":
                    return RedirectToAction("Home", "Home", new { area = "SuperAdmin" });
                case "Nsm":
                    return RedirectToAction("Home", "Home", new { area = "Nsm" });
                case "Distributor":
                    return RedirectToAction("Home", "Home", new { area = "Manager" });
                case "Accounts":
                    return RedirectToAction("Home", "Home", new { area = "Accounts" });
                case "AccountExecutive":
                    return RedirectToAction("Home", "Home", new { area = "AccountExecutive" });
                case "Management":
                    return RedirectToAction("Home", "Home", new { area = "Management" });
                default:
                    return RedirectToAction("LogIn", "LogIn", new { area = "" });
            }
           
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var user = (ViewUser)Session["user"];
            user.LogOutDateTime = DateTime.Now;
            _userManager.ChangeLoginStatus(user, 0);
            return Redirect("~/Home/Index");

        }

        public static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                    nic.OperationalStatus == OperationalStatus.Up)
                {

                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}