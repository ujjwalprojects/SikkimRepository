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
    [Authorize]
    public class ImageAlbumController : BaseController
    {
        dalImageAlbum objAlbum = new dalImageAlbum();
        dalComponent objcomp = new dalComponent();
        dalSchool ObjSch = new dalSchool();
        dalSite objSite = new dalSite();
        dalGeneral ObjGen = new dalGeneral();
        // GET: Admin/ImageAlbum
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            ImageAlbumManageModel model = new ImageAlbumManageModel();
            model.ImgAlbumList = objAlbum.GetImgAlbumPaged(PageNo, PageSize, _sModel.SchoolID, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvImgAlbumList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            ImageAlbumManageModel model = new ImageAlbumManageModel();
            model.Components = objcomp.GetComponentList("");
            ViewBag.SchoolID = _sModel.SchoolID;
            //if (User.IsInRole("DeptAdmin"))
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ImageAlbumManageModel model)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            model.ImageAlbum.IsPublished = false;
            if(model.ImageAlbum.SchoolID==0)
            {
                model.ImageAlbum.SchoolID = _sModel.SchoolID;
            }
            if (ModelState.IsValid)
            {
                model.ImageAlbum.TransDate = DateTime.Now;
                model.ImageAlbum.UserID = User.Identity.Name;
                string result = objAlbum.SaveImgAlbum(model.ImageAlbum);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblImageAlbums", "New Image Album Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "imagealbum", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
            }
            model.Components = objcomp.GetComponentList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            ImageAlbumManageModel model = new ImageAlbumManageModel();
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            model.ImageAlbum = objAlbum.GetImgAlbumByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImageAlbumManageModel model)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            model.ImageAlbum.IsPublished = false;
            if (model.ImageAlbum.SchoolID == 0)
            {
                model.ImageAlbum.SchoolID = _sModel.SchoolID;
            }
            if (ModelState.IsValid)
            {
                model.ImageAlbum.TransDate = DateTime.Now;
                model.ImageAlbum.UserID = User.Identity.Name;
                string result = objAlbum.SaveImgAlbum(model.ImageAlbum);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblImageAlbums", "Image Album Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "imagealbum", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "imagealbum", new { Area = "Admin" });
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
        public ActionResult PublishImgAlbum(long AlbumID, bool IsPublished)
        {
            try
            {
                string result = ObjGen.UpdatePublishedContent(AlbumID, IsPublished, "Image", User.Identity.Name);
                //string url = Url.Action("Index", "imagealbum", new { Area = "Admin"});
                if (!result.Contains("Error"))
                    TempData["ErrMsg"] = result;
                else
                    TempData["ErrMsg"] = result;
                return RedirectToAction("index", "imagealbum", new { Area = "Admin" });
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
            string result = objAlbum.DeleteImgAlbum(id);
            if (!result.ToLower().Contains("Error"))
            {
                objSite.AddAuditLog("utblImageAlbums", "Image Album Removed Succesfully ", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "Success: Data Removed Succesfully !";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "imagealbum", new { Area = "Admin" });
        }
    }
}