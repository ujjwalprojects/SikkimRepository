//using CMO.Web.Utility;
using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using SikkimRepository.Web.Controllers;
using SikkimRepository.Web.Utility;
using System;
using System.Web.Mvc;

namespace SikkimRepository.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "DeptAdmin")]
    public class BlockController : BaseController
    {
        dalBlock objBlock = new dalBlock();
        dalDistrict objDist = new dalDistrict();
        dalSite objSite = new dalSite();
        // GET: Admin/Block
        public ActionResult Index(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            MstBlockVM model = new MstBlockVM();
            model.BlockList = objBlock.GetBlocksPaged(PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvBlockList", model);
            }
            return View(model);
        }
        public ActionResult Add()
        {
            MstBlockManageModel model = new MstBlockManageModel();
            model.DistList = objDist.GetDistrictList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Add(MstBlockManageModel model)
        {
            if (ModelState.IsValid)
            {
                model.Block.TransDate = DateTime.Now;
                model.Block.UserID = User.Identity.Name;
                string result = objBlock.SaveBlock(model.Block);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstBlocks", "New Block Added",IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "block", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "block", new { Area = "Admin" });
            }
            model.DistList = objDist.GetDistrictList("");
            return View(model);
        }
        public ActionResult Edit(long id)
        {
            MstBlockManageModel model = new MstBlockManageModel();
            model.DistList = objDist.GetDistrictList("");
            model.Block = objBlock.GetBlockByID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Edit(MstBlockManageModel model)
        {
            if (ModelState.IsValid)
            {
                model.Block.TransDate = DateTime.Now;
                model.Block.UserID = User.Identity.Name;
                string result = objBlock.SaveBlock(model.Block);
                if (result.Contains("Success"))
                {
                    objSite.AddAuditLog("utblMstBlocks", "Block Updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = result;
                    return RedirectToAction("index", "block", new { Area = "Admin" });
                }
                TempData["ErrMsg"] = result;
                //return RedirectToAction("index", "block", new { Area = "Admin" });
            }
            model.DistList = objDist.GetDistrictList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AntiXSRFFilter]
        public ActionResult Delete(long id)
        {
            string result = objBlock.DeleteBlock(id);
            if (!result.ToLower().Contains("error"))
            {
                objSite.AddAuditLog("utblMstBlocks", "Block Removed", IPAddressGetter.GetIPAddress(), User.Identity.Name, id);
                TempData["ErrMsg"] = "Block Removed";
            }
            else
                TempData["ErrMsg"] = result;

            return RedirectToAction("index", "block", new { Area = "Admin" });
        }
    }
}