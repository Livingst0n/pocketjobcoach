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
    public class JobController : Controller
    {
        private pjcEntities db = new pjcEntities();

        //
        // GET: /Job/

        public ActionResult Index()
        {
            return View(db.jobs.ToList());
        }

        //
        // GET: /Job/Details/5

        public ActionResult Details(int id = 0)
        {
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        //
        // GET: /Job/Create

        public ActionResult Create()
        {
            ViewData["Tasks"] =  db.tasks.ToList();
            return View();
        }

        //
        // POST: /Job/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(job job)
        {
            if (ModelState.IsValid)
            {
                db.jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(job);
        }

        //
        // GET: /Job/Edit/5

        public ActionResult Edit(int id = 0)
        {
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        //
        // POST: /Job/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(job);
        }

        //
        // GET: /Job/Delete/5

        public ActionResult Delete(int id = 0)
        {
            job job = db.jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        //
        // POST: /Job/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            job job = db.jobs.Find(id);
            db.jobs.Remove(job);
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