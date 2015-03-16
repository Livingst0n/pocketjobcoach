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
    public class PromptController : Controller
    {
        private pjcEntities db = new pjcEntities();


        [ChildActionOnly]
        public ActionResult _Prompt(string parameter)
        {
            var promptTypes = db.prompttypes.Include(t => t.prompts);
            return PartialView("_Prompt", promptTypes);
        }

        //
        // GET: /Prompt/

        public ActionResult Index()
        {
            var prompts = db.prompts.Include(p => p.task).Include(p => p.prompttype);
            return View(prompts.ToList());
        }

        //
        // GET: /Prompt/Details/5

        public ActionResult Details(int id = 0)
        {
            prompt prompt = db.prompts.Find(id);
            if (prompt == null)
            {
                return HttpNotFound();
            }
            return View(prompt);
        }

        //
        // GET: /Prompt/Create

        public ActionResult Create()
        {
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName");
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName");
            return View();
        }

        //
        // POST: /Prompt/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(prompt prompt)
        {
            if (ModelState.IsValid)
            {
                db.prompts.Add(prompt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", prompt.taskID);
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName", prompt.typeID);
            return View(prompt);
        }

        //
        // GET: /Prompt/Edit/5

        public ActionResult Edit(int id = 0)
        {
            prompt prompt = db.prompts.Find(id);
            if (prompt == null)
            {
                return HttpNotFound();
            }
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", prompt.taskID);
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName", prompt.typeID);
            return View(prompt);
        }

        //
        // POST: /Prompt/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(prompt prompt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prompt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", prompt.taskID);
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName", prompt.typeID);
            return View(prompt);
        }

        //
        // GET: /Prompt/Delete/5

        public ActionResult Delete(int id = 0)
        {
            prompt prompt = db.prompts.Find(id);
            if (prompt == null)
            {
                return HttpNotFound();
            }
            return View(prompt);
        }

        //
        // POST: /Prompt/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            prompt prompt = db.prompts.Find(id);
            db.prompts.Remove(prompt);
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