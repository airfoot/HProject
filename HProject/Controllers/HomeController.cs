using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HProject.Application;
using HProject.Infrastructure;
using Microsoft.AspNet.Identity;
using HProject.Domain;
using System.Security.Principal;

namespace HProject.Controllers
{
    public class HomeController : Controller
    {
        private ILogger _logger;
        private HProjectDbContext _context;
      

        public HomeController(ILogger logger, HProjectDbContext hProjectDbContext)
        {
            this._logger = logger;
            this._context = hProjectDbContext;

        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            List<Menu> menuList = new List<Menu>();
            ProduceMenu menu = new ProduceMenu();
            IPrincipal User = HttpContext.User;
            if (User.IsInRole("Admin"))
            {
                menuList = menu.GetAdminMenu();
            }
            else
            {
                menuList = menu.GetUserMenu();
            }

            return PartialView(menuList);
        }
}