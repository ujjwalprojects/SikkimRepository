using Microsoft.AspNet.Identity.Owin;
using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using SikkimRepository.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SikkimRepository.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        dalSite objSite = new dalSite();
        dalGeneral ObjGen = new dalGeneral();
        dalSchool dalschook = new dalSchool();
        dalUser objUser = new dalUser();
        // GET: User
        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public UserController()
        {
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            
            DashModel model = new DashModel();
            model.SchoolDetails = dalschook.GetSchoolByID(_sModel.SchoolID);
            List<DashCountModel> _cmodel = new List<DashCountModel>();
            _cmodel = ObjGen.DashboardContentCount(_sModel.SchoolID);

            model.Content = _cmodel;
            model.DistCountImg = ObjGen.DistWiseImgContentCount();
            model.DistCountVid = ObjGen.DistWiseVidContentCount();
            model.DistCountTstm = ObjGen.DistWiseTstmContentCount();
            model.DistCountCS = ObjGen.DistWiseCSContentCount();
            return View(model);
            //return View();

        }
        public ActionResult UserList(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        {
            int TotalRecords = 0;
            RegisteredUserVM model = new RegisteredUserVM();
            model.UserList = objUser.GetUsersPaged(PageNo, PageSize, searchTerm, out TotalRecords);
            model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
            if (Request.IsAjaxRequest())
            {
                return PartialView("_pvUserList", model);
            }
            return View(model);
        }
    }
}