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
        public ActionResult Index(int id = 0)
        {
                var prompts = db.prompts.Include(p => p.task).Include(p => p.prompttype).Where(p => p.taskID == id);
                task associatedTask = db.tasks.Find(id);
                ViewBag.Task = associatedTask;
                return View(prompts.ToList());
        }

        // GET: /Prompt/Details/5
        public ActionResult Details(int id = 0)
        {
            prompt prompt = db.prompts.Find(id);
            if (prompt == null)
            {
                return HttpNotFound();
            }

            else if (Request.IsAjaxRequest())
            {
                return PartialView("_Prompt", prompt);
            }
            return View(prompt); //default code
        }

        // GET: /Prompt/Create
        public ActionResult Create(int id = 0)
        {
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName");
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName");

            if (id > 0)
            {
                task associatedTask = db.tasks.Find(id);
                ViewBag.Task = associatedTask;
            }
            return View();
        }

        // POST: /Prompt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(prompt prompt)
        {
            if (ModelState.IsValid)
            {
                db.prompts.Add(prompt);
                db.SaveChanges();
                //return RedirectToAction("Index"); 
                return Redirect("/Prompt/Index/" + prompt.taskID);
            }
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", prompt.taskID);
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName", prompt.typeID);
            return View(prompt);
        }

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

        // POST: /Prompt/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(prompt prompt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prompt).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", prompt.taskID);
                return Redirect("/Prompt/Index/" + prompt.taskID);
            }
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "taskName", prompt.taskID);
            ViewBag.typeID = new SelectList(db.prompttypes, "typeID", "typeName", prompt.typeID);
            return View(prompt);
        }

        // GET: /Prompt/Delete/5
        public ActionResult Delete(int id = 0)
        {
            prompt prompt = db.prompts.Find(id);
            if (prompt == null)
            {
                return HttpNotFound();
            }

            return View(prompt);//default
            //return PartialView("_Prompt", prompt);
        }

        // POST: /Prompt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        //public PartialViewResult DeleteConfirmed(int id)
        {
            prompt prompt = db.prompts.Find(id);
            var taskID = prompt.taskID;

            db.prompts.Remove(prompt);
            db.SaveChanges();

            //var returnPrompts = db.prompts.Include(p => p.task).Include(p => p.prompttype).Where(p => p.taskID == taskID);
            return Redirect("/Prompt/Index/" + taskID);
            //return RedirectToAction("Index", taskID); //default code
            //return PartialView("_Prompt", returnPrompts.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}