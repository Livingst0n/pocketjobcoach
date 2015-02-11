using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJCMobile.Models;

namespace PJCMobile.Controllers
{
    public class TaskController : Controller
    {
        private PJCModel db = new PJCModel();

        // GET: /Task/
        public ActionResult Index()
        {
            var tasks = db.tasks.Include(t => t.taskcategory);
            if (tasks.Count() == 0)
            {
                List<task> temp = tasks.ToList();
                temp.Add(new task {  taskName = "No tasks have been assigned.", description="Oops!" });
                return View(temp);
            }
           return View(tasks.ToList());
        }

        // GET: /Task/Details/5
        public ActionResult Details(int id = 0)
        {
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}