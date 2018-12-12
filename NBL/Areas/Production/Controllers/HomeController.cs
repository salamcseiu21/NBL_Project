using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NBL.Areas.Production.Controllers
{
    public class HomeController : Controller
    {
        // GET: Production/Home
        public ActionResult Home() 
        {
            return View();
        }
    }
}