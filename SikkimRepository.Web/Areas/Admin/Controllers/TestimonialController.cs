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
    public class TestimonialController : BaseController
    {
        dalTestimonial objTesti = new dalTestimonial();
        dalSchool ObjSch = new dalSchool();
        dalComponent objcomp = new dalComponent();
        dalSite objSite = new dalSite();
        dalGeneral ObjGen = new dalGeneral();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];
        // GET: Admin/Testimonial
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            TestimonialManageModel model = new TestimonialManageModel();
            model.TestimonialView = objTesti.GetTestimonialPaged(PageNo, PageSize,_sModel.SchoolID, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvTetimonialList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            TestimonialManageModel model = new TestimonialManageModel();
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TestimonialManageModel model)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            if (model.Testimonial.SchoolID == 0)
            {
                model.Testimonial.SchoolID = _sModel.SchoolID;
            }
            var validData = true;
            if (model.PhotoStrs.PhotoNormal == null)
            {
                ModelState.AddModelError("PhotoStrs.PhotoNormal", "Select a Photo");
                validData = false;
            }
            if (ModelState.IsValid && validData)
            {
                Random rand = new Random();
                string name = "T_Image_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                string normal_result = SaveImage(model.PhotoStrs.PhotoNormal, name);
                if (normal_result.Contains("Error"))
                {
                    TempData["ErrMsg"] = normal_result;
                    return View(model);
                }
                model.Testimonial.ImagePath = FileUrl + "TestimonialImage/" + normal_result;
                model.Testimonial.IsPublished = false;
                model.Testimonial.TransDate = DateTime.Now;
                model.Testimonial.UserID = User.Identity.Name;
                string result = objTesti.SaveTestimonial(model.Testimonial);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblTestimonials", "New Tetimonial Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "testimonial", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                return RedirectToAction("index", "testimonial", new { Area = "Admin" });
            }
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            TestimonialManageModel model = new TestimonialManageModel();
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            model.Testimonial = objTesti.GetTestimonialByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TestimonialManageModel model)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            if (model.Testimonial.SchoolID == 0)
            {
                model.Testimonial.SchoolID = _sModel.SchoolID;
            }
            if (ModelState.IsValid)
            {
                string name = "";
                if (model.PhotoStrs.PhotoNormal != null)
                {
                    Random rand = new Random();
                    name = "T_Images_" + DateTime.Now.ToString("yyyyMMdd") + "_" + rand.Next(50) + ".jpg";
                    string normal_result = SaveImage(model.PhotoStrs.PhotoNormal, name);
                    if (normal_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = normal_result;
                        return View(model);
                    }
                    model.Testimonial.ImagePath = FileUrl + "TestimonialImage/" + normal_result;
                }
                model.Testimonial.IsPublished = false;
                model.Testimonial.TransDate = DateTime.Now;
                model.Testimonial.UserID = User.Identity.Name;
                string result = objTesti.SaveTestimonial(model.Testimonial);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblTestimonials", "New Tetimonial Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "testimonial", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                return RedirectToAction("index", "testimonial", new { Area = "Admin" });
            }
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        public JsonResult GetSubComp(long id)
        {
            IEnumerable<utblMstSubComponent> SubComp = objTesti.GetSubCompByComp(id);
            return Json(SubComp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublishTestimonial(long ID, bool IsPublished)
        {
            try
            {
                string result = ObjGen.UpdatePublishedContent(ID, IsPublished, "Testimonial", User.Identity.Name);
                //string url = Url.Action("Index", "Testimonial", new { Area = "Admin" });
                if (!result.Contains("Error"))
                    TempData["ErrMsg"] = "0";
                else
                    TempData["ErrMsg"] = result;
                return RedirectToAction("index", "testimonial", new { Area = "Admin" });
                //return Json(new { success = true, url = url, }, JsonRequestBehavior.AllowGet);
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
            string result = objTesti.DeleteTestimonial(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblTestimonials", "Testimonial Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                DeleteFile(result);
                TempData["ErrMsg"] = "Testimonial Removed";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "testimonial", new { Area = "Admin" });
        }
        #region Helper
        private string SaveImage(string imageStr, string name)
        {
            try
            {
                var path = ""; var folderpath = "";
                path = Path.Combine(Server.MapPath("~/Uploads/TestimonialImage"), name);
                folderpath = Server.MapPath("~/Uploads/TestimonialImage");

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
                serverPath = Path.Combine(Server.MapPath("~/Uploads/TestimonialImage"), filename);
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