using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJCAdmin.Models;
using PJCAdmin.Classes.Helpers.MVCModelHelpers;

namespace PJCAdmin.Controllers
{
    [Authorize]
    public class TaskCategoryController : Controller
    {
        private EnumHelper enumHelper = new EnumHelper();

        //
        // GET: /TaskCategory/

        public ActionResult Index()
        {
            return View(enumHelper.getAllTaskCategories());
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
        public ActionResult Create(string taskCategory)
        {
            enumHelper.createTaskCategory(taskCategory);
            return RedirectToAction("Index");
        }

        //
        // GET: /TaskCategory/Edit/5

        public ActionResult Edit(string taskCategory)
        {
            if (!enumHelper.taskCategoryExists(taskCategory))
                return HttpNotFound();

            TaskCategory tc = enumHelper.getTaskCategory(taskCategory);
            return View(tc);
        }

        //
        // POST: /TaskCategory/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string oldCategory, string newCategory)
        {
            enumHelper.updateTaskCategory(oldCategory, newCategory);
            return RedirectToAction("Index");
        }

        //
        // GET: /TaskCategory/Delete/5

        public ActionResult Delete(string taskCategory)
        {
            if (!enumHelper.taskCategoryExists(taskCategory))
                return HttpNotFound();

            TaskCategory tc = enumHelper.getTaskCategory(taskCategory);
            return View(tc);
        }

        //
        // POST: /TaskCategory/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string taskCategory)
        {
            enumHelper.deleteTaskCategory(taskCategory);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            enumHelper.dispose();
            base.Dispose(disposing);
        }
    }
}