using System.Linq;
using System.Web.Mvc;
using NBL.BLL.Contracts;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class DiscountsController : Controller
    {
        
        private readonly ICommonManager _iCommonManager;
        private readonly IDiscountManager _iDiscountManager;

        public DiscountsController(ICommonManager iCommonManager,IDiscountManager iDiscountManager)
        {
            _iCommonManager = iCommonManager;
            _iDiscountManager = iDiscountManager;
        }
     
        public ActionResult AddDiscount()
        {
            ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddDiscount(Discount model)
        {

            if (ModelState.IsValid)
            {
                var anUser = (ViewUser)Session["user"];
                model.UpdateByUserId = anUser.UserId;
                bool result = _iDiscountManager.Add(model);
                ViewData["Message"] = result ? "Discount info Saved Successfully!!" : "Failed to Save!!";
                ModelState.Clear();
            }
            ViewBag.ClientTypes = _iCommonManager.GetAllClientType().ToList();
            return View();

        }

    }
}