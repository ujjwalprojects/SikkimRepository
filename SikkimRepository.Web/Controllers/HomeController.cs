using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Controllers
{
    public class HomeController : Controller
    {
        dalBanner ObjBanner = new dalBanner();
        dalImageAlbum ObjImageAlbum = new dalImageAlbum();
        dalVideoAlbum ObjVideoAlbum = new dalVideoAlbum();
        dalTestimonial ObjTestimonial = new dalTestimonial();
        dalCaseStudy ObjCaseStudy = new dalCaseStudy();
        dalGeneral ObjGen = new dalGeneral();
        dalComponent ObjComp = new dalComponent();
        dalSubComponent ObjSubComp = new dalSubComponent();

        public ActionResult Index()
        {
            //if (User.IsInRole("DeptAdmin"))
            //{
            //    return RedirectToAction("Dashboard", "Home");
            //}
            //else if(User.IsInRole("SchoolAdmin"))
            //{
            //    return RedirectToAction("Dashboard", "CitizenHome", new { Area = "Dept" });
            //}
            //else
            //{
            //    return RedirectToAction("index", "home", new { Area = "" });
            //}
            GenCustomModel model = new GenCustomModel();
            ViewBag.Banner = ObjBanner.GetBanners();
            ViewBag.ImageAlbum = ObjImageAlbum.GenImageAlbumlist();
            model.GenImages = ObjImageAlbum.GenImageAlbumlist();
            model.GenVideos = ObjVideoAlbum.GenVideoAlbumlist();
            model.GenTestimonials = ObjTestimonial.GenTestimonialslist();
            model.GenCaseStudies = ObjCaseStudy.GenCaseStudylist();
            model.GenContentCount = ObjGen.GenContenCount();
            //model.GenComponent = ObjComp.GetComponentList("");
            model.GenComponentModelList = ObjComp.GetGenComponentList();//New List
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult GetContentBySubComp(long CID = 0, long SCID = 0, int PageNo = 1, int PageSize = 5, string searchTerm = "")
        {
            GenCustomModel model = new GenCustomModel();
            model.GenImageAlbumList  = ObjImageAlbum.GenImageAlbumSelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.GenVideoAlbumList = ObjVideoAlbum.GenVideoAlbumSelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.GenTestimonialList = ObjTestimonial.GenTestimonialSelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.GenCaseStudyList = ObjCaseStudy.GenCaseStudySelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.Component = ObjComp.GetComponentByID(CID);
            model.SubComponent = ObjSubComp.GetSubComponentByID(SCID);
            model.GenComponent = ObjComp.GetComponentList("");
            return View(model);
        }
        public ActionResult LegalPages(string id="")
        {
            LegalPagesModel model = new LegalPagesModel();
            model.LegalPageName = id;
            return View(model);
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
        public ActionResult Error(int id)
        {
            ViewBag.StatusCode = id;

            return View();
        }
    }
}