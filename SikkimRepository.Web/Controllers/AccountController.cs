using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.HtmlHelpers;
//using CMO.Web.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SikkimRepository.Entities.CustomModels;
using SikkimRepository.Entities.DataAccess;
using SikkimRepository.Entities.Models;
using SikkimRepository.Web.Models;
using SikkimRepository.Web.Utility;

namespace SikkimRepository.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        dalSite objSite = new dalSite();
        dalSchool objschool = new dalSchool();
        dalUser objUser = new dalUser();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //Session["LoggedIN"] = null;
            //Session["LoginSalt"] = SHA256.CreateSalt(5);
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrMsg = "";
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            string key = "8080808080808080";
            string iv = "8080808080808080";
            if (model.Password != null)
            {
                model.Password = AES.Decrypt(model.Password, key, iv);
            }
            if (!this.IsCaptchaValid("Invalid Captcha Text"))
            {
                Session["LoggedIN"] = null;
                ModelState.AddModelError("", "Invalid Captcha Text");
                ModelState.Clear();
                ViewBag.ErrMsg = "Captcha is required...";
                return View(model);
            }
            //if (ModelState.IsValid)
            //{

                var user = new ApplicationUser();
                user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        Session["LoggedIN"] = null;
                        ModelState.AddModelError("", "Account has been disabled. Please contact website admin for more details.");
                        ModelState.Clear();
                        ViewBag.ErrMsg = "Account has been disabled. Please contact website admin for more details...";
                        return View(model);
                    }
                    else
                    {
                        SessionModel session_model = new SessionModel()
                        {
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            UserName = user.UserName,
                            Designation = user.Designation,
                            Gender = user.Gender,
                            IsActive = user.IsActive,
                            SchoolID = user.SchoolID
                        };
                        Session["SessionVar"] = session_model;
                        Session["LoggedIN"] = user.UserName;
                        await SignInManager.SignInAsync(user, model.RememberMe, false);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToLocal(returnUrl, user.Email);
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "User");
                        }
                    }
                }
                else
                {
                    Session["LoggedIN"] = null;
                    ModelState.AddModelError("", "Invalid username or password.");
                    ModelState.Clear();
                    ViewBag.ErrMsg = "Invalid username or password...";
                }
            //}
            // If we got this far, something failed, redisplay form
            //model.Password = "";
            return View(model);

        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> MyProfile()
        {
            string Id = this.User.Identity.GetUserId();
            //var user = UserManager.FindById(Id);
            RegisteredUserVM model = new RegisteredUserVM();
            model.MyProfileView = objUser.GetProfileDetails(Id);
            //model.SchoolList = objschool.GetSchoolList("");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MyProfile(RegisteredUserVM Item)
        {
            var user = await UserManager.FindByIdAsync(Item.MyProfileView.id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.PhoneNumber = Item.MyProfileView.PhoneNumber;
            user.UserName = Item.MyProfileView.Name;
            user.Designation = Item.MyProfileView.Designation;
            user.Gender = Item.MyProfileView.Gender;
            if ((string.IsNullOrEmpty(Item.MyProfileView.PhoneNumber)) || (string.IsNullOrEmpty(Item.MyProfileView.Name)) || (string.IsNullOrEmpty(Item.MyProfileView.Designation)) || (string.IsNullOrEmpty(Item.MyProfileView.Gender)))
            {
                if(string.IsNullOrEmpty(Item.MyProfileView.PhoneNumber))
                {
                    ModelState.AddModelError("MyProfileView.PhoneNumber", "PhoneNumber cannot be empty");
                }
                if (string.IsNullOrEmpty(Item.MyProfileView.Name))
                {
                    ModelState.AddModelError("MyProfileView.Name", "User Name cannot be empty");
                }
                if (string.IsNullOrEmpty(Item.MyProfileView.Designation))
                {
                    ModelState.AddModelError("MyProfileView.Designation", "Designation cannot be empty");
                }
                if (string.IsNullOrEmpty(Item.MyProfileView.Gender))
                {
                    ModelState.AddModelError("MyProfileView.Gender", "Gender cannot be empty");
                }
                return View(Item);
            }
            if (ModelState.IsValid)
            {
                var appDbContext = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                try
                {
                    var result = await UserManager.UpdateAsync(user);
                    TempData["ErrMsg"] = "Success: Profile record Updated Succesfully.";
                    return RedirectToAction("MyProfile", "Account");
                }
                catch (Exception)
                {
                    TempData["ErrMsg"] = "Error: Profile record could not be Updated, please contact Administrator.";
                }
            }
            return View(Item);
        }
        //
        // GET: /Account/Register
        [Authorize(Roles = "DeptAdmin")]
        public ActionResult Register()
        {
            RegisterUserManageModel model = new RegisterUserManageModel();
            model.SchoolList = objschool.GetSchoolList("");
            return View(model);
        }
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "DeptAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUserManageModel model)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Register.Name,
                    Email = model.Register.Email,
                    PhoneNumber = model.Register.PhoneNumber,
                    Designation = model.Register.Designation,
                    Gender = model.Register.Gender,
                    SchoolID = model.Register.SchoolID,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    IsActive = true,
                    RegDate = DateTime.Now
                };
                var result = await UserManager.CreateAsync(user,model.Register.Email);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //update in audit table
                    objSite.AddAuditLog("AspNetUsers", user.UserName + " record added", IPAddressGetter.GetIPAddress(), User.Identity.Name);
                    var result1 = UserManager.AddToRole(user.Id, "SchoolAdmin");
                    if (result1.Succeeded)
                    {
                        objSite.AddAuditLog("AspNetUserRoles", user.UserName + " role record added", IPAddressGetter.GetIPAddress(), User.Identity.Name);


                        //send registration email
                        utblTrnMessage Item = new utblTrnMessage();
                        Item.IsNew = true;
                        Item.MessageDate = DateTime.Now;
                        Item.MessageSubject = "School Admin Registration";
                        Item.MessageBody = "Dear " + model.Register.Name+"," + "\nYou have been successfully registered as a school admin for the Sikkim Shagun Portal.\nYour Login credientials:\nUsername: " + model.Register.Name + "\nPassword: " + model.Register.Email + "\n\nRegards,\n" + "Sikkim Shagun\nGovernment of Sikkim.";
                        Item.SMSContent = "";
                        Item.SentTo = model.Register.Email;
                        Item.ServiceDtlID = 0;
                        Item.RefTable = "";
                        Item.RefTransID = "";
                        Item.SentBy = "SikkimShagun";
                        Item.TransStatus = "";
                        Item.TransRemarks = "Registered on" + DateTime.Now;

                        SendMail objMailDAL = new SendMail();
                        bool res = objMailDAL.SendEmail(Item);

                     
                        //int MailStatus = SendEmailConfirmationToken(user.Id, "Confirm your account", user.Email, TempPassword);
                        //return RedirectToAction("adminuserlist");
                    }
                    
                    TempData["ErrMsg"] = user.UserName + " record updated";
                    return RedirectToAction("UserList", "User", new { Area = "" });

                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
     
        [Authorize(Roles = "DeptAdmin")]
        public ActionResult EditRegister(string id)
        {
            var user = UserManager.FindById(id);
            RegisterUserManageModel model = new RegisterUserManageModel();
            model.EditRegister = new RegisterEditViewModel()
            {
                id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = user.UserName,
                Designation = user.Designation,
                Gender = user.Gender,
                SchoolID = user.SchoolID
            };
            model.SchoolList = objschool.GetSchoolList("");
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "DeptAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRegister(RegisterUserManageModel model)
        {
            var user = await UserManager.FindByIdAsync(model.EditRegister.id);
            if (ModelState.IsValid)
            {
                user.UserName = model.EditRegister.Name;
                user.PhoneNumber = model.EditRegister.PhoneNumber;
                user.Designation = model.EditRegister.Designation;
                user.Gender = model.EditRegister.Gender;
                user.SchoolID = model.EditRegister.SchoolID;

                var Result = await UserManager.UpdateAsync(user);
                if (Result.Succeeded)
                {
                    objSite.AddAuditLog("AspNetUsers", user.UserName + " record updated", IPAddressGetter.GetIPAddress(), User.Identity.Name);

                    TempData["ErrMsg"] = user.UserName + " record updated";
                    return RedirectToAction("UserList", "account");
                }
                AddErrors(Result);
            }
            return View(model);
        }
        //list for displaying registered School Admin
        //[Authorize(Roles = "DeptAdmin")]
        //public ActionResult UserList(int PageNo = 1, int PageSize = 10, string searchTerm = "")
        //{
        //    int TotalRecords = 0;
        //    RegisteredUserVM model = new RegisteredUserVM();
        //    model.UserList = objUser.GetUsersPaged(PageNo, PageSize, searchTerm, out TotalRecords);
        //    model.PagingInfo = new PagingInfo { CurrentPage = PageNo, ItemsPerPage = PageSize, TotalItems = TotalRecords };
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_pvUserList", model);
        //    }
        //    return View(model);
        //}
        //delete school admin
        [Authorize(Roles = "DeptAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    var user = await UserManager.FindByIdAsync(id);
                    var logins = user.Logins;
                    var rolesForUser = await UserManager.GetRolesAsync(id);

                    if (rolesForUser.Count() > 0)
                    {
                        foreach (var item in rolesForUser.ToList())
                        {
                            // item should be the name of the role
                            var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                        }
                    }

                    await UserManager.DeleteAsync(user);
                    TempData["ErrMsg"] = "Record deleted successfully.";
                    return RedirectToAction("UserList", "user");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }
            else
            {
                return View();
            }
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl, string user = "")
        {
            Session["LoggedIN"] = true;
            objSite.AddAuditLog("", "User Login", IPAddressGetter.GetIPAddress(), user);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Dashboard", "User");
            }
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}