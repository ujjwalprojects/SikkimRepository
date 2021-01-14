using SikkimRepository.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SikkimRepository.Web.Controllers
{
    public class BaseController : Controller
    {
        protected SessionModel _sModel;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _sModel = Session["SessionVar"] as SessionModel;

            if (Session["LoggedIN"] == null)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();

                TempData["ErrMsg"] = "Your login session has expired. Please login again to continue.";
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "account",
                    action = "login",
                    area = ""
                }));
            }
            else
            {
                ViewBag.SchoolID = _sModel.SchoolID;
            }
        }
    }
}