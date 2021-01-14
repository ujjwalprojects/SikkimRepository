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
    public class SubComponentController : BaseController
    {
        dalSubComponent objSubcomp = new dalSubComponent();
        dalComponent objComp = new dalComponent();
        dalSite objSite = new dalSite();
        // GET: Admin/SubComponent
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            MstSubComponentVM model = new MstSubComponentVM();
            model.SubComponentList = objSubcomp.GetSubComponentPaged(PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvSubComponentList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            MstSubComponentManageModel model = new MstSubComponentManageModel();
            model.ComponentList = objComp.GetComponentList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MstSubComponentManageModel model)
        {
            if (ModelState.IsValid)
            {
                model.SubComponent.TransDate = DateTime.Now;
                model.SubComponent.UserID = User.Identity.Name;
                string result = objSubcomp.SaveSubComponent(model.SubComponent);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstSubComponent", "New SubComponent Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "subcomponent", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "subcomponent", new { Area = "Admin" });
            }
            model.ComponentList = objComp.GetComponentList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            MstSubComponentManageModel model = new MstSubComponentManageModel();
            model.ComponentList = objComp.GetComponentList("");
            model.SubComponent = objSubcomp.GetSubComponentByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MstSubComponentManageModel model)
        {
            if (ModelState.IsValid)
            {
                model.SubComponent.TransDate = DateTime.Now;
                model.SubComponent.UserID = User.Identity.Name;
                string result = objSubcomp.SaveSubComponent(model.SubComponent);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstSubComponents", "New SubComponent Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "subcomponent", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "subcomponent", new { Area = "Admin" });
            }
            model.ComponentList = objComp.GetComponentList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Delete(long id)
        {
            string result = objSubcomp.DeleteSubComponent(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblMstSubComponent", "SubComponent Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "SubComponent Removed";
            }
            else
                TempData["ErrMsg"] = result;

            return RedirectToAction("index", "subcomponent", new { Area = "Admin" });
        }
    }
}