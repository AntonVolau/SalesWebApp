using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SalesUpdater.Web.Data.Models;
using System.Web.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using ViewResult = Microsoft.AspNetCore.Mvc.ViewResult;

namespace SalesUpdater.Web.Data.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (IsAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View();
            }

            ViewBag.Name = "Not Logged In";
            return View();
        }

        public bool IsAdminUser()
        {
            if (!User.Identity.IsAuthenticated) return false;

            var user = User.Identity;
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = userManager.GetRoles(user.GetUserId());

            return s[0] == "Admin";
        }
    }
}