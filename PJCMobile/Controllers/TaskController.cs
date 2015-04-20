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
        private pjcEntities db = new pjcEntities();

        //
        // GET: /Task/

        public ActionResult Index()
        {
            var tasks = db.tasks.Include(t => t.taskcategory);
            var userTasks = db.Users.Find(System.Web.Security.Membership.GetUser().ProviderUserKey).usertasks;
            List<DateTime> array = new List<DateTime>();

            foreach (usertask ut in userTasks)
            {
                if(ut.lastCompleted == null)
                    array.Add(DateTime.Parse("01/01/2000"));
                else
                    array.Add((DateTime)ut.lastCompleted);
            }
            ViewData["taskDate"] = array;
            //if (tasks.Count() == 0)
            //{
            //    List<usertask> temp = userTasks.ToList();
            //    temp.Add(new task { taskName = "No tasks have been assigned.", description = "Oops!" });
            //    return View(temp);
            //}
            return View(userTasks.ToList());
        }

        public ActionResult Finish(int taskID)
        {
            db.usertasks.Find(System.Web.Security.Membership.GetUser().ProviderUserKey, taskID).lastCompleted = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
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

        //
        // GET: /Task/Create

        public ActionResult Create()
        {
            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName");
            return View();
        }

        //
        // POST: /Task/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(task task)
        {
            if (ModelState.IsValid)
            {
                db.tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            return View(task);
        }

        //
        // GET: /Task/Edit/5

        public ActionResult Edit(int id = 0)
        {
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            return View(task);
        }

        //
        // POST: /Task/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            return View(task);
        }

        //
        // GET: /Task/Delete/5

        public ActionResult Delete(int id = 0)
        {
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // POST: /Task/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task task = db.tasks.Find(id);
            db.tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: /Task/Alert

        public ActionResult AlertsIndex()
        {

            //var userTasks = db.tasks.Include(t => t.usertasks);
            var userTasks = db.Users.Find(System.Web.Security.Membership.GetUser().ProviderUserKey).usertasks;
            if (userTasks.Count() == 0)
            {
                List<task> tasks = new List<task>();
                List<task> temp = tasks.ToList();
                temp.Add(new task { taskName = "No tasks have been assigned.", description = "Oops!" });
                return View(temp);
            }

            List<usertask> alertTasks = new List<usertask>();
            string dayOfWeek = System.DateTime.Now.DayOfWeek.ToString();

            foreach(var item in userTasks)
            {
                foreach(char c in item.daysOfWeek)
                {
                    if(c == dayOfWeek[0])
                        alertTasks.Add(item);
                }
            }
            
            return View("Index", alertTasks);
        }

        //
        // POST: /Task/Alert

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlertsIndex(task task)
        {
            if (ModelState.IsValid)
            {
                db.tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            return View(task);
        }
    }
}