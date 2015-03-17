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

       
        //Wage Calculator Page
        public ActionResult WageCalculator()
        {
            return View();
        }

   


        
        //Notes Page
        public ActionResult Notes()
        {
            return View();
        }

    

        //Calendar Page
        public ActionResult Calendar()
        {

            return View();
        }
    }
}
