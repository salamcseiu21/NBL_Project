using System;
using System.Linq;
using System.Web.Mvc;
using NBL.Models;
using NBL.BLL;

namespace NBL.Areas.Editor.Controllers
{
    [Authorize(Roles ="Editor")]
    public class BranchController : Controller
    {
        // GET: Editor/Branch
        readonly BranchManager _branchManager=new BranchManager();
        public ActionResult ViewBranch()
        {
            var branches = _branchManager.GetAll().ToList();
            return View(branches);
        }

        public ActionResult AddBranch()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddBranch(Branch model)
        {


            try
            {
              
                if(ModelState.IsValid)
                {
                    bool result = _branchManager.Save(model);
                    TempData["message"] = result;
                    ModelState.Clear();
                }
              
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = e?.Message;
                return View();
            }

        }

        public ActionResult Edit(int id)
        {
            var branch = _branchManager.GetBranchById(id);
            return View(branch);
        }

        [HttpPost]
        public ActionResult Edit(int id, Branch model)
        {
            Branch branch = _branchManager.GetBranchById(id);
            if (ModelState.IsValid)
            {
               
                branch.BranchAddress = model.BranchAddress;
                branch.BranchEmail = model.BranchEmail;
                branch.BranchName = model.BranchName;
                branch.Title = model.Title;
                branch.BranchOpenigDate = Convert.ToDateTime(model.BranchOpenigDate);
                branch.BranchPhone = model.BranchPhone;
                bool result = _branchManager.Update(branch);
                if (result)
                    return RedirectToAction("ViewBranch");
                return View(branch);
            }
            return View(branch);
        }
    }
}