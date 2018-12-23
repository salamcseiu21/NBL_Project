using System.Web.Mvc;
using NBL.DAL;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles = "Editor")]
    public class DivisionController : Controller
    {
        readonly DivisionGateway _divisionGateway = new DivisionGateway();
        // GET: Editor/Division
        public ActionResult All()
        {
            var divisions = _divisionGateway.GetAll;
            return View(divisions);
        }
    }
}