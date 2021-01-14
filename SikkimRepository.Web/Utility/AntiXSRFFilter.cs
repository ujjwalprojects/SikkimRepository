using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SikkimRepository.Web.Utility
{
    public class AntiXSRFFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sess_token = filterContext.HttpContext.Session["XSRFToken"].ToString();
            var form_token = filterContext.ActionParameters["sessionXSRFToken"].ToString();

            if (form_token != null)
            {
                if (sess_token != form_token)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "account",
                        action = "login",
                        area = ""
                    }));
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}