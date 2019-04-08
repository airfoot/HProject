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


     
        [HttpGet]
        public ActionResult CreateUser()
        {
            UserViewModel userViewModel = new UserViewModel();
            var departmentList = _context.Departments;
            var departmentSelectList = new SelectList(departmentList, "Id", "Name");
            ViewData["departmentDropdown"] = departmentSelectList;
            return View(userViewModel);
        }
        [HttpPost]
        public ActionResult CreateUser(UserViewModel userViewModel, HttpPostedFileBase userPicture = null)
        {
            string userPictureFilePath = null;
            IdentityResult result = null;
            if (ModelState.IsValid)
            {

                if (userPicture != null)
                {
                    //Save uploaded file to path of "~/Image/UserPicture"
                    int splitIndex = userPicture.ContentType.LastIndexOf(@"/");
                    string newFileName = Guid.NewGuid().ToString() + "." + userPicture.ContentType.Substring(++splitIndex);
                    userPictureFilePath = Path.Combine(Server.MapPath("/Image/UserPicture"), newFileName);
                    if (!Directory.Exists(Server.MapPath("/Images/UserPicture")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/Images/UserPicture"));
                    }
                    userPicture.SaveAs(userPictureFilePath);

                }
                else
                {
                    userPictureFilePath = Path.Combine(Server.MapPath("/Images"), "defaultUser.jpg");
                }
                if (!ValidateEmail.IsValidEmail(userViewModel.Email))
                {
                    ModelState.AddModelError("Email", "邮箱地址无效");
                    if (!string.IsNullOrEmpty(userPictureFilePath) && !userPictureFilePath.Contains("defaultUser.jpg"))
                    {
                        System.IO.File.Delete(userPictureFilePath);
                    }
                    return View(userViewModel);
                }
  
                User user = new User();
                user.FullName = userViewModel.FullName;
                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;

               
                user.PhoneNumber = userViewModel.ContactPhone;
                user.IconUrl = userPictureFilePath;
                if (string.IsNullOrEmpty(userViewModel.Password))
                {
                    result = UserManager.Create(user);
                    AddDepartmentToUser(user.Id.ToString(), userViewModel.Department);
                }
                else
                {
                    result = UserManager.Create(user, userViewModel.Password);
                    AddDepartmentToUser(user.Id.ToString(), userViewModel.Department);
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("GetUsers", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                }
                //Clean before created file if encounter any errors
                if (!string.IsNullOrEmpty(userPictureFilePath) && !userPictureFilePath.Contains("defaultUser.jpg"))
                {
                    System.IO.File.Delete(userPictureFilePath);
                }

            }

            return View(userViewModel);
        }



        [HttpGet]
        public ActionResult GetUsers(int? page)
        {
            var allUsers = from u in UserManager.Users
                           where u.LockoutEnabled == false
                           orderby u.UserName descending
                           select new UserViewModel
                           {
                               Id = u.Id,
                               FullName = u.FullName,
                               UserName = u.UserName,
                               Email = u.Email,
                              
                               Department = u.UserDepartment.Name,

                               ContactPhone = u.PhoneNumber
                           };

            var pageNumber = page ?? 1;
            var onePageOfUsers = allUsers.ToPagedList(pageNumber, 15);
            return View(onePageOfUsers);
        }

        /// <summary>
        /// Gets the user picture.
        /// </summary>
        /// <param name="Id">The Id of User object.</param>
        /// <returns></returns>
        [HttpGet]
        public FilePathResult GetUserPicture(string id)
        {
            User user = UserManager.FindById(id);
            if (string.IsNullOrEmpty(user.IconUrl))
            {
                return File(Path.Combine(Server.MapPath("/Image"), "defaultUser.jpg"), "image/jpeg");
            }
            string contentType = System.Web.MimeMapping.GetMimeMapping(user.IconUrl);
            return File(user.IconUrl, contentType);

        }

        public ActionResult DeleteUser(string id)
        {
            User user = UserManager.FindById(id);
            var deleteResult = UserManager.SetLockoutEnabled(user.Id, true);
            if (deleteResult.Succeeded)
            {
                return RedirectToAction("GetUsers");
            }
            ViewBag.ErrorMessages = deleteResult.Errors;
            return View("Errors");

        }

        ////////////////////////////////


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

        private void AddDepartmentToUser(string userId, string departmentId)
        {
            try
            {

          
            var user = _context.Users.Where(u => u.Id.ToString() == userId).FirstOrDefault();
           
            var department = _context.Departments.Find(new Guid(departmentId));
       
                user.UserDepartment = department;
         
           
            _context.SaveChanges();
            }
            catch(Exception e)
            {
                throw;
            }
        }
    }
}