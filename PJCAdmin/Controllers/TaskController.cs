using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJCAdmin.Models;

namespace PJCAdmin.Controllers
{
    public class TaskController : Controller
    {
        private pjcEntities db = new pjcEntities();

        //
        // GET: /Task/

        public ActionResult Index()
        {
            var tasks = db.tasks.Include(t => t.taskcategory);
            return View(tasks.ToList());
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
    }
}