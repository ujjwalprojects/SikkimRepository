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
    public class ComponentController : BaseController
    {
        dalComponent objcomp = new dalComponent();
        dalSite objSite = new dalSite();
        dalCompCategory dalcategory = new dalCompCategory();
        // GET: Admin/Component
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            MstComponentVM model = new MstComponentVM();
            model.ComponentList = objcomp.GetComponentPaged(PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvComponentList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            MstComponentVM model = new MstComponentVM();
            model.CategoryDDList = dalcategory.GetCategoryDD();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MstComponentVM model)
        {
            if (ModelState.IsValid)
            {
                model.ComponentModel.UserID = User.Identity.Name;
                string result = objcomp.SaveComponent(model.ComponentModel);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstComponents", "New Component Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "component", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
            }
            model.CategoryDDList = dalcategory.GetCategoryDD();
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            MstComponentVM model = new MstComponentVM();
            model.ComponentModel = objcomp.GetComponentByID(id);
            model.CategoryDDList = dalcategory.GetCategoryDD();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MstComponentVM model)
        {
            if (ModelState.IsValid)
            {
                model.ComponentModel.UserID = User.Identity.Name;
                string result = objcomp.SaveComponent(model.ComponentModel);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstComponents", "Components Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "component", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "component", new { Area = "Admin" });
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Delete(long id)
        {
            string result = objcomp.DeleteComponent(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblMstComponent", "Component Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "Component Removed";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "component", new { Area = "Admin" });
        }
    }
}