using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Controllers
{
    public class CaseStudiesController : Controller
    {
        dalCaseStudy ObjCaseStudies = new dalCaseStudy();
        dalComponent ObjComp = new dalComponent();
        // GET: CaseStudies
        public ActionResult Index(long SCID = 0, int PageNo = 1, int PageSize = 5, string searchTerm = "")
        {
            CaseStudyManageModel model = new CaseStudyManageModel();
            CaseStudyGenViewModel _csmodel = new CaseStudyGenViewModel();
            _csmodel = ObjCaseStudies.GenCaseStudySelectPaged(SCID,PageNo, PageSize, searchTerm);
            model.GenCaseStudies = _csmodel;
            model.Components = ObjComp.GetComponentList("");
            model.GenComponentModelList = ObjComp.GetGenComponentList();//Component List
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvCaseStudiesList", model);
            }
            return View(model);
        }
    }
}