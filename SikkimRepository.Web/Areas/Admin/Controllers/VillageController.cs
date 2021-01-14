using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Areas.Admin.Controllers
{
    public class VillageController : BaseController
    {
        dalVillage objVill = new dalVillage();
        dalDistrict objDist = new dalDistrict();
        // GET: Admin/Village
        public ActionResult Index()
        {
            return View();
        }
    }
}