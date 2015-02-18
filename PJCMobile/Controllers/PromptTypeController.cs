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
    public class PromptTypeController : Controller
    {
        private pjcEntities1 db = new pjcEntities1();

        //
        // GET: /PromptType/

        public ActionResult Index()
        {
            return View(db.prompttypes.ToList());
        }

        //
        // GET: /PromptType/Details/5

        public ActionResult Details(int id = 0)
        {
            prompttype prompttype = db.prompttypes.Find(id);
            if (prompttype == null)
            {
                return HttpNotFound();
            }
            return View(prompttype);
        }

        //
        // GET: /PromptType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PromptType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(prompttype prompttype)
        {
            if (ModelState.IsValid)
            {
                db.prompttypes.Add(prompttype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(prompttype);
        }

        //
        // GET: /PromptType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            prompttype prompttype = db.prompttypes.Find(id);
            if (prompttype == null)
            {
                return HttpNotFound();
            }
            return View(prompttype);
        }

        //
        // POST: /PromptType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(prompttype prompttype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prompttype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prompttype);
        }

        //
        // GET: /PromptType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            prompttype prompttype = db.prompttypes.Find(id);
            if (prompttype == null)
            {
                return HttpNotFound();
            }
            return View(prompttype);
        }

        //
        // POST: /PromptType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            prompttype prompttype = db.prompttypes.Find(id);
            db.prompttypes.Remove(prompttype);
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