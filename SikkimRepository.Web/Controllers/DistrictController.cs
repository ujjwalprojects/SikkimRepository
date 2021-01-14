using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Controllers
{
    public class DistrictController : Controller
    {
        dalImageAlbum ObjImageAlbum = new dalImageAlbum();
        dalVideoAlbum ObjVideoAlbum = new dalVideoAlbum();
        dalTestimonial ObjTestimonial = new dalTestimonial();
        dalCaseStudy ObjCaseStudy = new dalCaseStudy();
        dalDistrict ObjDist = new dalDistrict();
        dalComponent ObjComp = new dalComponent();
        // GET: District
        public ActionResult Index(string DistCode)
        {
            GenCustomModel model = new GenCustomModel();
            model.District = ObjDist.GetDistByCode(DistCode);
            model.GenImgAlbumByDist = ObjImageAlbum.GetImageAlbumByDist(DistCode).Take(5);
            model.GenVidAlbumByDist = ObjVideoAlbum.GetVideoAlbumByDist(DistCode).Take(5);
            model.GenTestimonialByDist = ObjTestimonial.GetTestimonialsByDist(DistCode).Take(3);
            model.GenCaseStudyByDist = ObjCaseStudy.GetCaseStudyByDist(DistCode).Take(3);
            model.GenComponent = ObjComp.GetComponentList("");
            return View(model);
        }
        public ActionResult VideoListByDistrict(string DistCode, int PageNo = 1, int PageSize = 12, string searchTerm = "")
        {
            
            GenViewListByDistrictCode model = new GenViewListByDistrictCode();
            model.District = ObjDist.GetDistByCode(DistCode);
            ViewBag.DistCode = DistCode;
            ViewBag.DistName = model.District.DistName;
            model = ObjVideoAlbum.GetVideoAlbumByDistCode(DistCode, PageNo, PageSize, searchTerm);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvVideoAlbumListByDistrict", model);
            }
            return View(model);
        }

        public ActionResult ImageListByDistrict(string DistCode, int PageNo = 1, int PageSize = 12, string searchTerm = "")
        {
            GenViewListByDistrictCode model = new GenViewListByDistrictCode();
            model.District = ObjDist.GetDistByCode(DistCode);
            ViewBag.DistCode = DistCode;
            ViewBag.DistName = model.District.DistName;
            model = ObjImageAlbum.GetImageAlbumByDistCode(DistCode, PageNo, PageSize, searchTerm);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvImageAlbumListByDistrict", model);
            }
            return View(model);
        }
        public ActionResult TestimonialListByDistrict(string DistCode, int PageNo = 1, int PageSize = 12, string searchTerm = "")
        {
            GenViewListByDistrictCode model = new GenViewListByDistrictCode();
            model.District = ObjDist.GetDistByCode(DistCode);
            ViewBag.DistCode = DistCode;
            ViewBag.DistName = model.District.DistName;
            model = ObjTestimonial.GetTestimonialByDistCode(DistCode, PageNo, PageSize, searchTerm);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvTestimonialListByDistrict", model);
            }
            return View(model);
        }
        public ActionResult CaseStudyListByDistrict(string DistCode, int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            GenViewListByDistrictCode model = new GenViewListByDistrictCode();
            model.District = ObjDist.GetDistByCode(DistCode);
            ViewBag.DistCode = DistCode;
            ViewBag.DistName = model.District.DistName;
            model = ObjCaseStudy.GetCaseStudyByDistCode(DistCode, PageNo, PageSize, searchTerm);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvCaseStudyListByDistrict", model);
            }
            return View(model);
        }
    }
}