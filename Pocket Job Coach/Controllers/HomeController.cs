using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pocket_Job_Coach.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            string format = "h:mm tt MMM d yyyy";
            ViewBag.date = date.ToString(format);

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
    }
}
