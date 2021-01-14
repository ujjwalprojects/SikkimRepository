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
    public class CaseStudyController : BaseController
    {
        dalCaseStudy objCaseStudy = new dalCaseStudy();
        dalComponent objcomp = new dalComponent();
        dalSchool ObjSch = new dalSchool();
        dalSite objSite = new dalSite();
        dalGeneral ObjGen = new dalGeneral();
        private string FileUrl = ConfigurationManager.AppSettings["FileURL"];
        // GET: Admin/CaseStudy
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            CaseStudyManageModel model = new CaseStudyManageModel();
            model.CaseStudyView = objCaseStudy.GetCaseStudyPaged(PageNo, PageSize,_sModel.SchoolID, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvCaseStudyList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            CaseStudyManageModel model = new CaseStudyManageModel();
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CaseStudyManageModel model, HttpPostedFileBase docFile)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            if(model.CaseStudy.SchoolID == 0)
            {
                model.CaseStudy.SchoolID = _sModel.SchoolID;
            }
            var validData = true;
            if (docFile != null)
            {
                string fileResult = FileTypeCheck.IsValidFile(docFile, "File");
                if (!fileResult.Equals("Success"))
                {
                    ModelState.AddModelError("FileErr", fileResult);
                    validData = false;
                }
                else
                {
                    string file_result = SaveFile(docFile);
                    model.CaseStudy.CaseStudyFilePath = FileUrl + "CaseStudy/" + file_result;
                    if (file_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = file_result;
                        return View(model);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("FileErr", "Select Case Study file");
                validData = false;
            }
            if (ModelState.IsValid && validData)
            {
                model.CaseStudy.IsPublished = false;
                model.CaseStudy.TransDate = DateTime.Now;
                model.CaseStudy.UserID = User.Identity.Name;
                string result = objCaseStudy.SaveCaseStudy(model.CaseStudy);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblCaseStudies", "New Case Study Added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "casestudy", new { Area = "Admin" });
                }
                else
                {
                    DeleteFile(model.CaseStudy.CaseStudyFilePath);
                    TempData["ErrMsg"] = result;
                }
               
            }
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            CaseStudyManageModel model = new CaseStudyManageModel();
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            model.CaseStudy = objCaseStudy.GetCaseStudyByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CaseStudyManageModel model, HttpPostedFileBase docFile)
        {
            ViewBag.SchoolID = _sModel.SchoolID;
            if (model.CaseStudy.SchoolID == 0)
            {
                model.CaseStudy.SchoolID = _sModel.SchoolID;
            }
            string PrevFile = model.CaseStudy.CaseStudyFilePath;
            var validData = true;
            if (docFile != null)
            {
                string fileResult = FileTypeCheck.IsValidFile(docFile, "File");
                if (!fileResult.Equals("Success"))
                {
                    ModelState.AddModelError("FileErr", fileResult);
                    validData = false;
                }
                else
                {
                    string file_result = SaveFile(docFile);
                    model.CaseStudy.CaseStudyFilePath = FileUrl + "CaseStudy/" + file_result;
                    if (file_result.Contains("Error"))
                    {
                        TempData["ErrMsg"] = file_result;
                        return View(model);
                    }
                }
            }
            if (ModelState.IsValid && validData)
            {
                model.CaseStudy.IsPublished = false;
                model.CaseStudy.TransDate = DateTime.Now;
                model.CaseStudy.UserID = User.Identity.Name;
                string result = objCaseStudy.SaveCaseStudy(model.CaseStudy);
                if (result.Contains("Success"))
                {
                    DeleteFile(PrevFile);
                    objSite.AddAuditLog("utblCaseStudies", "Case Study Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "casestudy", new { Area = "Admin" });
                }
                else
                {
                    DeleteFile(model.CaseStudy.CaseStudyFilePath);
                    TempData["ErrMsg"] = result;
                }
            }
            model.Components = objcomp.GetComponentList("");
            model.SchoolList = ObjSch.GetSchoolList("");
            return View(model);
        }
        public JsonResult GetSubComp(long id)
        {
            IEnumerable<utblMstSubComponent> SubComp = objCaseStudy.GetSubCompByComp(id);
            return Json(SubComp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PublishCaseStudy(long ID, bool IsPublished)
        {
            try
            {
                string result = ObjGen.UpdatePublishedContent(ID, IsPublished, "CaseStudy", User.Identity.Name);
                //string url = Url.Action("Index", "CaseStudy", new { Area = "Admin" });
                if (!result.Contains("Error"))
                    TempData["ErrMsg"] = result;
                else
                    TempData["ErrMsg"] = result;
                return RedirectToAction("index", "casestudy", new { Area = "Admin" });
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
            utblCaseStudie cs = new utblCaseStudie();
            cs = objCaseStudy.GetCaseStudyByID(id);
            string PrvPath = cs.CaseStudyFilePath;
            utblCaseStudie model = new utblCaseStudie();
            model = objCaseStudy.GetCaseStudyByID(id);
            string result = objCaseStudy.DeleteCaseStudy(id);
            if (!result.ToLower().Contains("Error"))
            {
                objSite.AddAuditLog("utblCaseStudies", "Case Study Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                DeleteFile(model.CaseStudyFilePath);
                TempData["ErrMsg"] = "Success: Data Removed Succesfully !";
            }
            else
            {
                TempData["ErrMsg"] = result;
            }
            return RedirectToAction("index", "casestudy", new { Area = "Admin" });
        }
        #region Helper
        private string SaveFile(HttpPostedFileBase file)
        {
            try
            {
                string filename = file.FileName.Trim();
                string[] nameArr = filename.Split('.');
                string name = nameArr[0];
                string ext = nameArr[1];
                name = nameArr[0].Replace(" ", "_");
                name = name + "_" + DateTime.Now.ToString("yyyyMMdd");
                filename = name + "." + ext;
                var path = Path.Combine(Server.MapPath("~/Uploads/CaseStudy"), filename);
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
                string serverPath = Path.Combine(Server.MapPath("~/Uploads/CaseStudy"), filename);
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
        #endregion
    }

}