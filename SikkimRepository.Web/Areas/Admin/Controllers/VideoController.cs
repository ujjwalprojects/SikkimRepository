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
    public class VideoController : BaseController
    {
        dalVideo objVideo = new dalVideo();
        dalVideoAlbum objVideoAlbum = new dalVideoAlbum();
        dalSite objSite = new dalSite();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];
        // GET: Admin/Video
        public ActionResult Index(long AlbumID, int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            VideoManageModel model = new VideoManageModel();
            model.VideoAlbum = objVideoAlbum.GetVideoAlbumByID(AlbumID);
            model.VideoList = objVideo.GetVideoPaged(AlbumID, PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvVideoList", model);
            }
            return View(model);
        }
        public ActionResult Add(long id)
        {
            VideoSaveModel model = new VideoSaveModel();
            utblVideo _vmodel = new utblVideo();
            model.VideoAlbum = objVideoAlbum.GetVideoAlbumByID(id);
            model.Videos = _vmodel;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VideoSaveModel model, HttpPostedFileBase docFile)
        {
            var validData = true;
            if (docFile != null)
            {
                string fileResult = FileTypeCheck.IsValidFile(docFile, "Video");
                if (!fileResult.Equals("Success"))
                {
                    ModelState.AddModelError("FileErr", fileResult);
                    validData = false;
                }
                else
                {
                    string file_result = SaveFile(docFile, model.VideoAlbum.VideoAlbumID);
                    model.Videos.VideoFilePathDraft = FileUrl + "Videos/" + file_result;
                    if (file_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = file_result;
                        return View(model);
                    }
                }
            }
            if (ModelState.IsValid && validData)
            {
                model.Videos.VideoAlbumID = model.VideoAlbum.VideoAlbumID;
                model.Videos.TransDate = DateTime.Now;
                model.Videos.UserID = User.Identity.Name;
                string result = objVideo.SaveVideo(model.Videos);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblVideos", "New Videos Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "Video", new { Area = "Admin", AlbumID = model.VideoAlbum.VideoAlbumID });
                }
                TempData["ErrMsg"] = result;
            }
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            VideoSaveModel model = new VideoSaveModel();
            model.Videos = objVideo.GetVideoByID(id);
            model.VideoAlbum = objVideoAlbum.GetVideoAlbumByID(model.Videos.VideoAlbumID);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VideoSaveModel model, HttpPostedFileBase docFile)
        {
            utblVideo vid = new utblVideo();
            vid = objVideo.GetVideoByID(model.Videos.VideoID);
            string PrvPath = vid.VideoFilePathDraft;
            string file_result = "";
            var validData = true;
            if (docFile != null)
            {
                string fileResult = FileTypeCheck.IsValidFile(docFile, "Video");
                if (!fileResult.Equals("Success"))
                {
                    ModelState.AddModelError("FileErr", fileResult);
                    validData = false;
                }
                else
                {
                    file_result = SaveFile(docFile, model.Videos.VideoAlbumID);
                    model.Videos.VideoFilePathDraft = FileUrl + "/Videos/" + file_result;
                    if (file_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = file_result;
                        return View(model);
                    }

                }
            }
            if (ModelState.IsValid && validData)
            {
                //model.Videos.VideoAlbumID = model.VideoAlbum.VideoAlbumID;
                model.Videos.TransDate = DateTime.Now;
                model.Videos.UserID = User.Identity.Name;
                string result = objVideo.SaveVideo(model.Videos);
                if (result.Contains("Success"))
                {
                    if (file_result!="")
                    {
                        DeleteFile(PrvPath);
                    }
                    objSite.AddAuditLog("utblVideos", "Videos Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "Video", new { Area = "Admin", AlbumID = model.Videos.VideoAlbumID });
                }
                TempData["ErrMsg"] = result;
            }
            model.VideoAlbum = objVideoAlbum.GetVideoAlbumByID(model.Videos.VideoAlbumID);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Delete(long id, long AlbumID)
        {
            utblVideo model = new utblVideo();
            model = objVideo.GetVideoByID(id);
            string PrvPath = model.VideoFilePathDraft;
            string result = objVideo.DeleteVideo(id);
            if (!result.ToLower().Contains("Error"))
            {
                objSite.AddAuditLog("utblVideos", "Video Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                DeleteFile(PrvPath);
                TempData["ErrMsg"] = "Success: Data Removed Succesfully !";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "Video", new { Area = "Admin", AlbumID = AlbumID });
        }
        #region Helper
        private string SaveFile(HttpPostedFileBase file, long AlbumID)
        {
            try
            {
                string filename = file.FileName.Trim();
                string[] nameArr = filename.Split('.');
                string name = nameArr[0];
                string ext = nameArr[1];
                name = nameArr[0].Replace(" ", "_");
                name = name + "_" + AlbumID + "_" + DateTime.Now.ToString("yyyyMMdd");
                filename = name + "." + ext;
                var path = Path.Combine(Server.MapPath("~/Uploads/Videos"), filename);
                file.SaveAs(path);
                return filename;
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
                string serverPath = Path.Combine(Server.MapPath("~/Uploads/Videos"), filename);
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
        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~/Uploads/Videos/" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        #endregion
    }
}