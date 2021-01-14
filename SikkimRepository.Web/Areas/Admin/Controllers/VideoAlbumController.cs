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
    public class VideoAlbumController : BaseController
    {
        // GET: Admin/VideoAlbum
        dalVideoAlbum objAlbum = new dalVideoAlbum();
        dalComponent objcomp = new dalComponent();
        dalSchool ObjSch = new dalSchool();
        dalSite objSite = new dalSite();
        dalGeneral ObjGen = new dalGeneral();
        // GET: Admin/VideoAlbum
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            VideoAlbumManageModel model = new VideoAlbumManageModel();
            model.VideoAlbumList = objAlbum.GetVideoAlbumPaged(PageNo, PageSize,_sModel.SchoolID, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvVideoAlbumList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            VideoAlbumManageModel model = new VideoAlbumManageModel();
            model.Components = objcomp.GetComponentList("");
            ViewBag.SchoolID = _sModel.SchoolID;
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VideoAlbumManageModel model)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            model.VideoAlbum.IsPublished = false;
            if (model.VideoAlbum.SchoolID == 0)
            {
                model.VideoAlbum.SchoolID = _sModel.SchoolID;
            }
            if (ModelState.IsValid)
            {
                model.VideoAlbum.TransDate = DateTime.Now;
                model.VideoAlbum.UserID = User.Identity.Name;
                string result = objAlbum.SaveVideoAlbum(model.VideoAlbum);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblVideoAlbums", "New Video Album Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "Videoalbum", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "Videoalbum", new { Area = "Admin" });
            }
            model.Components = objcomp.GetComponentList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            VideoAlbumManageModel model = new VideoAlbumManageModel();
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            model.VideoAlbum = objAlbum.GetVideoAlbumByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VideoAlbumManageModel model)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            model.VideoAlbum.IsPublished = false;
            if (model.VideoAlbum.SchoolID == 0)
            {
                model.VideoAlbum.SchoolID = _sModel.SchoolID;
            }
            if (ModelState.IsValid)
            {
                model.VideoAlbum.TransDate = DateTime.Now;
                model.VideoAlbum.UserID = User.Identity.Name;
                string result = objAlbum.SaveVideoAlbum(model.VideoAlbum);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblVideoAlbums", "Video Album Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "Videoalbum", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
            }
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        public JsonResult GetSubComp(long id)
        {
            IEnumerable<utblMstSubComponent> SubComp = objAlbum.GetSubCompByComp(id);
            return Json(SubComp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublishVideoAlbum(long AlbumID, bool IsPublished)
        {
            try
            {
                string result = ObjGen.UpdatePublishedContent(AlbumID, IsPublished, "Video", User.Identity.Name);
                if (!result.Contains("Error"))
                    TempData["ErrMsg"] = result;
                else
                    TempData["ErrMsg"] = result;

                return RedirectToAction("index", "Videoalbum", new { Area = "Admin" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            string result = objAlbum.DeleteVideoAlbum(id);
            if (!result.ToLower().Contains("Error"))
            {
                objSite.AddAuditLog("utblVideoAlbums", "Video Album Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "Success: Data Removed Succesfully !";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "Videoalbum", new { Area = "Admin" });
        }
    }
}