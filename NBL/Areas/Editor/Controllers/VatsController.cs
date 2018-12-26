
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class VatsController : Controller
    {
        private readonly IVatManager _iVatManager;

        public VatsController(IVatManager iVatManager)
        {
            _iVatManager = iVatManager;
        }
      
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
                if (_iVatManager.AddVat(model))
                {
                    ModelState.Clear();
                    ViewData["Message"] = "Vat info Saved successfully..!";
                }
            }
            return View();
        }

    }
}