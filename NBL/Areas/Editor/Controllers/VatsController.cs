
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class VatsController : Controller
    {
        readonly VatManager _vatManager=new VatManager();
      
        [HttpGet]
        public ActionResult AddVat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddVat(Vat model)
        {
            if (ModelState.IsValid)
            {
                User user = (User) Session["user"];
                model.UpdateByUserId = user.UserId;
                if (_vatManager.AddVat(model))
                {
                    ModelState.Clear();
                    ViewData["Message"] = "Vat info Saved successfully..!";
                }
            }
            return View();
        }

    }
}