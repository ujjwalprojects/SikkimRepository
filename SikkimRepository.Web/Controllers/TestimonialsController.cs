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
    public class TestimonialsController : Controller
    {
        dalTestimonial ObjTestimonial = new dalTestimonial();
        dalComponent ObjComp = new dalComponent();
        // GET: Testimonials
        public ActionResult Index(long SCID =0,int PageNo = 1, int PageSize = 3, string searchTerm = "")
        {
           
            TestimonialManageModel model = new TestimonialManageModel();
            TestimonialGenViewModel _tmodel = new TestimonialGenViewModel();
            _tmodel = ObjTestimonial.GenTestimonialSelectPaged(SCID, PageNo, PageSize, searchTerm);
            model.GenTestimonials = _tmodel;
            model.Components = ObjComp.GetComponentList("");
            model.GenComponentModelList = ObjComp.GetGenComponentList();//Component List
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvTetimonialList", model);
            }
            return View(model);
        }
    }
}