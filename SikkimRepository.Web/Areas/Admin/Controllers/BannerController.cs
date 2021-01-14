//using CMO.Web.Utility;
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
    [Authorize(Roles = "DeptAdmin")]
    public class BannerController : BaseController
    {
        dalBanner objBanner = new dalBanner();
        dalSite objSite = new dalSite();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];
        // GET: Admin/Banner
        public ActionResult Index()
        {
            IEnumerable<utblBanner> model = objBanner.GetBanners();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvBannerList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Add(BannerSaveModel model, string sessionXSRFToken)
        {
            var validData = true;
            if (model.PhotoStrs.PhotoNormal == null)
            {
                ModelState.AddModelError("PhotoStrs.PhotoNormal", "Select a Photo");
                validData = false;
            }
            //else if (!IsBase64(model.PhotoStrs.PhotoNormal))
            //{
            //    ModelState.AddModelError("PhotoStrs.PhotoNormal", "Invalid Photo");
            //    validData = false;
            //}
            if (ModelState.IsValid && validData)
            {
                Random rand = new Random();
                string name = "Banner_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                string normal_result = SaveImage(model.PhotoStrs.PhotoNormal, name);
                if (normal_result.Contains("Error"))
                {
                    TempData["ErrMsg"] = normal_result;
                    return View(model);
                }
                model.Banner.BannerPath = "/Uploads/Banner/" + normal_result;
                model.Banner.TransDate = DateTime.Now;
                model.Banner.UserID = User.Identity.Name;
                model.Banner.BannerThumb = model.PhotoStrs.PhotoThumb;
                string result = objBanner.SaveBanner(model.Banner);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblBanners", "New Banner Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "banner", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                DeleteFile(name);
            }
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            BannerSaveModel model = new BannerSaveModel();
            model.Banner = objBanner.GetBannerByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Edit(BannerSaveModel model, string sessionXSRFToken)
        {
            utblBanner banner = new utblBanner();
            banner = objBanner.GetBannerByID(model.Banner.BannerID);
            string PrvPath = banner.BannerPath;
            if (ModelState.IsValid)
            {
                string name = "";
                if (model.PhotoStrs.PhotoNormal != null)
                {
                    //if (!IsBase64(model.PhotoStrs.PhotoNormal))
                    //{
                    //    ModelState.AddModelError("PhotoStrs.PhotoNormal", "Invalid Photo");
                    //    return View(model);
                    //}
                    Random rand = new Random();
                    name = "Banner_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string normal_result = SaveImage(model.PhotoStrs.PhotoNormal, name);
                    if (normal_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = normal_result;
                        return View(model);
                    }
                    else
                    {
                        DeleteFile(PrvPath);
                    }
                    model.Banner.BannerPath = "/Uploads/Banner/" + normal_result;
                    model.Banner.BannerThumb = model.PhotoStrs.PhotoThumb;
                }
                model.Banner.TransDate = DateTime.Now;
                model.Banner.UserID = User.Identity.Name;
                //model.Banner.IPAddress = IPAddressGetter.GetIPAddress();

                string result = objBanner.SaveBanner(model.Banner);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblBanners", "Banner Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name, model.Banner.BannerID);
                    
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "banner", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                DeleteFile(name);
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            utblBanner banner = new utblBanner();
            banner = objBanner.GetBannerByID(id);
            string PrvPath = banner.BannerPath;
            string result = objBanner.DeleteBanner(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblBanners", "Banner Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                DeleteFile(PrvPath);
                TempData["ErrMsg"] = "Success: Data Removed Succesfully !";
            }
            else
                TempData["ErrMsg"] = result;

            return RedirectToAction("index", "banner", new { Area = "Admin" });
        }
        #region Helper
        private string SaveImage(string imageStr, string name)
        {
            try
            {
                var path = ""; var folderpath = "";
                path = Path.Combine(Server.MapPath("~/Uploads/Banner"), name);
                folderpath = Server.MapPath("~/Uploads/Banner");

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
                serverPath = Path.Combine(Server.MapPath("~/Uploads/Banner"), filename);
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