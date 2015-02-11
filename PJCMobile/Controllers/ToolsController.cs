using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PJCMobile.Controllers
{
    public class ToolsController : Controller
    {
        //
        // GET: /Tools/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Tools/Details/5

        public ActionResult WageCalculator()
        {
            return View();
        }

        //
        // GET: /Tools/Create

        public ActionResult Notes()
        {
            return View();
        }

        //
        // POST: /Tools/Create


        public ActionResult Calendar()
        {
            return View();
        }
    }
}
