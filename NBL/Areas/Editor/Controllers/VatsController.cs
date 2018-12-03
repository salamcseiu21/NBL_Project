
using System.Web.Mvc;
using NblClassLibrary.BLL;
using NblClassLibrary.Models;
using NblClassLibrary.Models.ViewModels;

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
                var user = (ViewUser) Session["user"];
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