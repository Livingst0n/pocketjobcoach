using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PJCMobile.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            string format = "h:mm tt MMM d yyyy";
            ViewBag.date = date.ToString(format);

            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
