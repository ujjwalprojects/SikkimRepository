using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using SikkimRepository.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "DeptAdmin")]
    public class CompCategoryController : Controller
    {
        // GET: Admin/CompCategory
        dalSite objSite = new dalSite();
        dalCompCategory objdal = new dalCompCategory();
        public ActionResult Categorylist(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            MstCategoryVM model = new MstCategoryVM();
            model.CategoryList = objdal.GetCategoryPaged(PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvCategoryList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(utblMstCompCategorie model)
        {
            if (ModelState.IsValid)
            {
                model.UserID = User.Identity.Name;
                string result = objdal.SaveCategory(model);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstCompCategories", "New Component Category Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("Categorylist", "CompCategory", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
            }
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            utblMstCompCategorie model = new utblMstCompCategorie();
            model = objdal.GetCategoryByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(utblMstCompCategorie model)
        {
            if (ModelState.IsValid)
            {
                model.UserID = User.Identity.Name;
                string result = objdal.SaveCategory(model);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstCompCategories", "New Component Categorys Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("Categorylist", "CompCategory", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            string result = objdal.DeleteCategory(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblMstCompCategories", "Component Category Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "Component Category Removed";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("Categorylist", "CompCategory", new { Area = "Admin" });
        }
    }
}