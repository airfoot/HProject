using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HProject.Infrastructure;
using HProject.Domain;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using HProject.Models;
using System.IO;
using X.PagedList;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HProject.Controllers
{
    public class AccountController : Controller
    {
        private ILogger _logger;
        private HProjectDbContext _context;
        public AccountController(ILogger logger, HProjectDbContext hProjectDbContext)
        {
            this._context = hProjectDbContext;
            this._logger = logger;
        }

        // GET: Account
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Errors", new string[] { "Access Denied" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //User user = UserManager.Find(login.Username, login.Password);
                User user = UserManager.Find(login.Username, login.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid name or password.");
                }
                else
                {
                    ClaimsIdentity ident = UserManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = login.RememberMe
                    }, ident);

                   
                    return RedirectToLocal(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(login);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResetPassword(Password password)
        {
            if (ModelState.IsValid)
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), password.OldPassword, password.NewPassword);
                if (result.Succeeded)
                {
                    var returnSuccess = new { isSuccess = true, ModelStateErrors = "", };
                    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnSuccess), "application/json");
                }
                else
                {
                    AddErrors(result);
                }

            }
            var returnFail = new
            {
                isSuccess = false,
                // ModelState错误信息
                ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                  .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
            };
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnFail), "application/json");

        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private UserManagers UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<UserManagers>();
            }
        }
    }
}