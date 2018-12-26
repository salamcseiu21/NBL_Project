﻿using NBL.Areas.SuperAdmin.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.DAL;
using NBL.Models;
using NBL.Models.ViewModels;

namespace NBL.Areas.SuperAdmin.Controllers
{
    [Authorize(Roles = "Super")]
    public class AssignController : Controller
    {
      
        private readonly IBranchManager _iBranchManager;
        private readonly UserManager _userManager = new UserManager();
        private readonly ICommonManager _iCommonManager;
        private readonly RegionManager _regionManager=new RegionManager();
        private readonly TerritoryManager _territoryManager=new TerritoryManager();
        private readonly SuperAdminUserManager _superAdminUserManager = new SuperAdminUserManager();

        public AssignController(IBranchManager iBranchManager,ICommonManager iCommonManager)
        {
            _iBranchManager = iBranchManager;
            _iCommonManager = iCommonManager;
        }
        [HttpGet]
        public ActionResult AssignRegionToBranch()
        {

            var branches = _iBranchManager.GetAll();
            return View(branches);
        }

        
        [HttpPost]
        public ActionResult AssignRegionToBranch(FormCollection collection)
        {
            User user = (User)Session["user"];
            Branch branch = new Branch {BranchId = Convert.ToInt32(collection["BranchId"])};

            List<Region> regionList = new List<Region>();
            string[] tokens = collection["RegionId"].Split(',');
            for (int i = 0; i < tokens.Length; i++) 
            {
                regionList.Add(new Region { RegionId = Convert.ToInt32(tokens[i]) });
            }
            branch.RegionList = regionList;
            int rowAffected = _regionManager.AssignRegionToBranch(branch, user);
            var branches = _iBranchManager.GetAll();
            return View(branches);
        }


        [HttpGet]
        public ActionResult AssignDistrictToRegion()
        {

            var regions = _regionManager.GetAllRegion();
            return View(regions);
        }
        [HttpPost]
        public ActionResult AssignDistrictToRegion(FormCollection collection)
        {
            Region aRegion = new Region {RegionId = Convert.ToInt32(collection["RegionId"])};

            List<District> districtList = new List<District>();
            string[] tokens = collection["DistrictId"].Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                districtList.Add(new District { DistrictId = Convert.ToInt32(tokens[i]) });
            }
            aRegion.Districts = districtList;
            int rowAffected = _regionManager.AssignDristrictToRegion(aRegion);
            var regions = _regionManager.GetAllRegion();
            return View(regions);
        }


        [HttpGet]
        public ActionResult AssignUpazillaToTerritory()
        {

            var territories = _territoryManager.GetAllTerritory();
            return View(territories);
        }
        [HttpPost]
        public ActionResult AssignUpazillaToTerritory(FormCollection collection)
        {
            Territory aTerritory = new Territory
            {
                TerritoryId = Convert.ToInt32(collection["TerritoryId"])
            };

            List<Upazilla> upazillaLsit = new List<Upazilla>();
            string[] tokens = collection["UpazillaId"].Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                upazillaLsit.Add(new Upazilla { UpazillaId = Convert.ToInt32(tokens[i]) });
            }
            aTerritory.UpazillaList = upazillaLsit;
            int rowAffected = _territoryManager.AssignUpazillaToTerritory(aTerritory);
            if (rowAffected > 0)
            {
                
                return RedirectToAction("ViewTerritory", "Home", new { area = "SuperAdmin" });
            }
            var territories = _territoryManager.GetAllTerritory();
            return View(territories);
        }

        [HttpGet]
        public ActionResult AssignBranchToUser()
        {
            var branches = _iBranchManager.GetAll();
            return View(branches);
        }

        [HttpPost]
        public ActionResult AssignBranchToUser(FormCollection collection)
        {
            User anUser = new User {UserId = Convert.ToInt32(collection["UserId"])};
            List<Branch> branchList = new List<Branch>();
            string[] tokens = collection["SelectedBranch"].Split(',');
            for (int i = 0; i < tokens.Length; i++)
            {
                branchList.Add(new Branch { BranchId = Convert.ToInt32(tokens[i]) });
            }
            TempData["Message"] = _superAdminUserManager.AssignBranchToUser(anUser, branchList);
            var branches = _iBranchManager.GetAll();
            var roles = _iCommonManager.GetAllUserRoles().ToList();
            ViewBag.Roles = roles;
            return View(branches);
        }
        public ActionResult ViewUser()
        {
            var users = _userManager.GetAll.ToList();
            return View(users);

        }


        public ActionResult UnAssignDistrict()
        {

            return View(_regionManager.GetRegionListWithDistrictInfo());
        }
        [HttpPost]
        public JsonResult UnAssignDistrict(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int regionDetailsId = Convert.ToInt32(collection["RegionDetailsId"]);
                var regionDetails=_regionManager.GetRegionDetailsById(regionDetailsId);
                string reason = collection["Reason"];
                var anUser = (ViewUser)Session["user"];
                int result = _regionManager.UnAssignDistrictFromRegion(regionDetails, reason, anUser);
                aModel.Message = result > 0 ? "<p style='color:green'>UnAssigned Successfull! </p>" : "<p style='color:red'>Failed to Un Assign</p>";
            }
            catch (Exception e)
            {
                aModel.Message = "<p style='color:red'>" + e.InnerException?.Message + "</p>";
            }
           
            return Json(aModel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UnAssignUpazilla() 
        {

            return View();
        }
        [HttpPost]
        public JsonResult UnAssignUpazilla(FormCollection collection)
        {
            SuccessErrorModel aModel = new SuccessErrorModel();
            try
            {
                int territoryDetailsId = Convert.ToInt32(collection["TerritoryDetailsId"]); 
                string reason = collection["Reason"];
                var anUser = (ViewUser)Session["user"];
                int result = _territoryManager.UnAssignUpazillaFromTerritory(territoryDetailsId, reason, anUser);
                aModel.Message = result > 0 ? "<p style='color:green'>UnAssigned Successfull! </p>" : "<p style='color:red'>Failed to Un Assign</p>";
            }
            catch (Exception e)
            {
                aModel.Message = "<p style='color:red'>" + e.InnerException?.Message + "</p>";
            }

            return Json(aModel, JsonRequestBehavior.AllowGet);
        }
    }
}