using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class DiscountsController : Controller
    {
        readonly DiscountManager _discountManager=new DiscountManager();
        readonly CommonGateway _commonGateway=new CommonGateway();
     
        public ActionResult AddDiscount()
        {
            ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddDiscount(Discount model)
        {

            if (ModelState.IsValid)
            {
                var anUser = (ViewUser)Session["user"];
                model.UpdateByUserId = anUser.UserId;
                bool result = _discountManager.AddDiscount(model);
                ViewData["Message"] = result ? "Discount info Saved Successfully!!" : "Failed to Save!!";
                ModelState.Clear();
            }
            ViewBag.ClientTypes = _commonGateway.GetAllClientType.ToList();
            return View();

        }

    }
}