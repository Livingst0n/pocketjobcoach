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
    [Authorize]
    public class TaskCategoryController : Controller
    {
        private pjcEntities db = new pjcEntities();

        //
        // GET: /TaskCategory/

        public ActionResult Index()
        {
            //return View(db.taskcategories.ToList());
            return View();
        }


        //
        // GET: /TaskCategory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TaskCategory/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(taskcategory taskcategory)
        public ActionResult Create(string taskcategory)
        {
            /*db.taskcategories.Add(taskcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
             */
            return View();
        }

        //
        // GET: /TaskCategory/Edit/5

        public ActionResult Edit(int id = 0)
        {
            /*taskcategory taskcategory = db.taskcategories.Find(id);
            if (taskcategory == null)
            {
                return HttpNotFound();
            }
            return View(taskcategory);
             */
            return View();
        }

        //
        // POST: /TaskCategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(taskcategory taskcategory)
        public ActionResult Edit(string taskcategory)
        {
            /*if (ModelState.IsValid)
            {
                db.Entry(taskcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taskcategory);
             */
            return View();
        }

        //
        // GET: /TaskCategory/Delete/5

        public ActionResult Delete(int id = 0)
        {
            /*taskcategory taskcategory = db.taskcategories.Find(id);
            if (taskcategory == null)
            {
                return HttpNotFound();
            }
            return View(taskcategory);
             */
            return View();
        }

        //
        // POST: /TaskCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*taskcategory taskcategory = db.taskcategories.Find(id);
            db.taskcategories.Remove(taskcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
             */
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}