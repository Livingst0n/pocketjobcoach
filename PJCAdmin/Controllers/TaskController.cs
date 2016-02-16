using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJCAdmin.Models;
using System.IO;
using PagedList;//NuGet package used to add paging 

namespace PJCAdmin.Controllers
{
    public class TaskController : Controller
    {
        private pjcEntities db = new pjcEntities();

        // GET: /Task/
        public ActionResult Index(string searchString, int? page)
        {
            /*//Retrieve tasks and related taskcategory, a reference of the list of associated prompts is already in each task
            var tasks = db.tasks.Include(t => t.taskcategory);//before pagination

            if(!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.taskName.Contains(searchString));
                page = 1;
            }

            tasks = tasks.OrderBy(t => t.taskName);
            int pageSize = 25;
            int pageNumber = (page ?? 1); //if page is null, pageNumber= 1

            ViewBag.searchString = searchString;
            //return View(tasks.ToList());  //before adding pagination
            return View(tasks.ToPagedList(pageNumber, pageSize));
             */
            return View();
        }

        //public ActionResult Index(int?)

        public ActionResult _Prompt()
        {        
            return PartialView("_Prompt");
        }

        // GET: /Task/Details/5
        public ActionResult Details(int id = 0)
        {
            /*var task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
             */
            return View();
        }


        // GET: /Task/Create
        public ActionResult Create()
        {
            /*ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName");
            //ViewBag.prompt = new SelectList(db.prompts, "prompt", "prompt");
            ViewBag.promptTypeID = new SelectList(db.prompttypes, "typeID", "typeName");
             */
            return View();
        }

        // POST: /Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(task task, List<prompt> prompts)
        public ActionResult Create(string task, List<string> prompts)
        {
            /*//validate
            if (ModelState.IsValid)
            {
                db.tasks.Add(task);
                db.SaveChanges();

                //return RedirectToAction("Index");//default code
                return Redirect("/Prompt/Index/" + task.taskID);
            }

            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            return View(task);
             */
            return View();
        }

        // GET: /Task/Edit/5
        public ActionResult Edit(int id = 0)
        {
            /*task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            
            return View(task);
             */
            return View();
        }

        // POST: /Task/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(task task)
        public ActionResult Edit(string task)
        {
            /*if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                //db.Entry(task.prompts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.taskCategoryID = new SelectList(db.taskcategories, "categoryID", "categoryName", task.taskCategoryID);
            return View(task);
             */
            return View();
        }

        // GET: /Task/Delete/5
        public ActionResult Delete(int id = 0)
        {
            /*task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
             */
            return View();
        }

        // POST: /Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            /*var prompts = db.prompts.Where(p => p.taskID == id);
            
            task task = db.tasks.Find(id);
            db.tasks.Remove(task);
            foreach (prompt p in prompts)
            {
                db.prompts.Remove(p);
            }
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