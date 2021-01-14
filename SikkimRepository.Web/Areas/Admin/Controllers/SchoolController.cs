using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using SikkimRepository.Web.Controllers;
using SikkimRepository.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "DeptAdmin")]
    public class SchoolController : BaseController
    {
        dalBlock objBlock = new dalBlock();
        dalDistrict objDist = new dalDistrict();
        dalSchool objSchool = new dalSchool();
        dalSite objSite = new dalSite();
        // GET: Admin/School
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            MstSchoolVM model = new MstSchoolVM();
            model.SchoolList = objSchool.GetSchoolsPaged(PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvSchoolList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            MstSchoolManageModel model = new MstSchoolManageModel();
            model.DistList = objDist.GetDistrictList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MstSchoolManageModel model)
        {
            if (ModelState.IsValid)
            {
                model.School.TransDate = DateTime.Now;
                model.School.UserID = User.Identity.Name;
                string result = objSchool.SaveSchool(model.School);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstSchools", "New School Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "school", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "school", new { Area = "Admin" });
            }
            model.DistList = objDist.GetDistrictList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            MstSchoolManageModel model = new MstSchoolManageModel();
            model.DistList = objDist.GetDistrictList("");
            model.School = objSchool.GetSchoolByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MstSchoolManageModel model)
        {
            if (ModelState.IsValid)
            {
                model.School.TransDate = DateTime.Now;
                model.School.UserID = User.Identity.Name;
                string result = objSchool.SaveSchool(model.School);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstSchools", "New School Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "School", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "School", new { Area = "Admin" });
            }
            model.DistList = objDist.GetDistrictList("");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Delete(long id)
        {
            string result = objSchool.DeleteSchool(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblMstSchools", "School Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "School Removed";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "School", new { Area = "Admin" });
        }
        public JsonResult GetBlocks(string code)
        {
            IEnumerable<utblMstBlock> blocks = objSchool.GetBlockByDistrict(code);
            return Json(blocks, JsonRequestBehavior.AllowGet);
        }
    }
}