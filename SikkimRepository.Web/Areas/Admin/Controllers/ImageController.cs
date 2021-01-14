using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using SikkimRepository.Web.Controllers;
using SikkimRepository.Web.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Areas.Admin.Controllers
{
    public class ImageController : BaseController
    {
        dalImage objImg = new dalImage();
        dalImageAlbum objImgAlbum = new dalImageAlbum();
        dalSite objSite = new dalSite();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];
        // GET: Admin/Image
        public ActionResult Index(long AlbumID,int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            ImageManageModel model = new ImageManageModel();
            model.ImageAlbum = objImgAlbum.GetImgAlbumByID(AlbumID);
            model.ImageList = objImg.GetImagePaged(AlbumID,PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvImageList", model);
            }
            return View(model);
        }
        public ActionResult Add(long id)
        {
            ImageSaveModel model = new ImageSaveModel();
            model.ImageAlbum = objImgAlbum.GetImgAlbumByID(id);
            model.Images = new utblImage();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ImageSaveModel model)
        {
            var validData = true;
            if (model.PhotoStrs.PhotoNormal == null)
            {
                ModelState.AddModelError("PhotoStrs.PhotoNormal", "Please Upload the Image field Cannot be empty!");
                validData = false;
            }
            if (ModelState.IsValid && validData)
            {
                Random rand = new Random();
                string name = "Image_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                string normal_result = SaveImage(model.PhotoStrs.PhotoNormal, name);
                if (normal_result.Contains("Error"))
                {
                    TempData["ErrMsg"] = normal_result;
                    return View(model);
                }
                model.Images.ImageAlbumID = model.ImageAlbum.ImageAlbumID;
                model.Images.ImageFilePathNormal = "/Uploads/Images/" + normal_result;
                model.Images.TransDate = DateTime.Now;
                model.Images.UserID = User.Identity.Name;
                model.Images.ImageFilePathThumb = model.PhotoStrs.PhotoThumb;
               
                string result = objImg.SaveImage(model.Images);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblImages", "New Image Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "image", new { Area = "Admin", AlbumID = model.ImageAlbum.ImageAlbumID });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "image", new { Area = "Admin", AlbumID = model.ImageAlbum.ImageAlbumID });
            }
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            ImageSaveModel model = new ImageSaveModel();
            model.Images = objImg.GetImageByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ImageSaveModel model)
        {
            utblImage img = new utblImage();
            img = objImg.GetImageByID(model.Images.ImageID);
            string PrvPath = img.ImageFilePathNormal;
            if (ModelState.IsValid)
            {
                string name = "";
                if (model.PhotoStrs.PhotoNormal != null)
                {
                    Random rand = new Random();
                    name = "Images_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string normal_result = SaveImage(model.PhotoStrs.PhotoNormal, name);
                    if (normal_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = normal_result;
                        return View(model);
                    }
                    model.Images.ImageFilePathNormal = "/Uploads/Images/" + normal_result;
                    model.Images.ImageFilePathThumb = model.PhotoStrs.PhotoThumb;
                }
                model.Images.TransDate = DateTime.Now;
                model.Images.UserID = User.Identity.Name;
                string result = objImg.SaveImage(model.Images);
                if (result.Contains("Success"))
                {
                    DeleteFile(PrvPath);
                    objSite.AddAuditLog("utblImages", "Image Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "image", new { Area = "Admin", AlbumID = model.Images.ImageAlbumID });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "image", new { Area = "Admin", AlbumID = model.Images.ImageAlbumID });
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Delete(long id,long AlbumID)
        {
            utblImage model = new utblImage();
            model = objImg.GetImageByID(id);
            string PrvPath = model.ImageFilePathNormal;
            string result = objImg.DeleteImage(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblImages", "Image Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                DeleteFile(PrvPath);
                TempData["ErrMsg"] = "Success: Data Removed Succesfully !";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "image", new { Area = "Admin", AlbumID = AlbumID });
        }

        #region Helper
        private string SaveImage(string imageStr, string name)
        {
            try
            {
                var path = ""; var folderpath = "";
                path = Path.Combine(Server.MapPath("~/Uploads/Images"), name);
                folderpath = Server.MapPath("~/Uploads/Images");

                //Check if directory exist
                if (!System.IO.Directory.Exists(folderpath))
                {
                    System.IO.Directory.CreateDirectory(folderpath); //Create directory if it doesn't exist
                }
                string x = imageStr.Replace("data:image/jpeg;base64,", "");
                byte[] imageBytes = Convert.FromBase64String(x);

                System.IO.File.WriteAllBytes(path, imageBytes);
                return name;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        private void DeleteFile(string filepath)
        {
            try
            {
                string[] fileArr = filepath.Split('/');
                string filename = fileArr[fileArr.Length - 1];
                string serverPath = "";
                serverPath = Path.Combine(Server.MapPath("~/Uploads/Images"), filename);
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }
        public static bool IsBase64(string base64String)
        {
            base64String = base64String.Replace("data:image/jpeg;base64,/", "");
            //if (base64String.Replace(" ", "").Length % 4 != 0)
            //{
            //    return false;
            //}

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        #endregion
    }
}