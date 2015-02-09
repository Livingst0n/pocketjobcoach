using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PJCAdmin.Models;

namespace PJCAdmin.Filters
{
    public class UserTypeController : Controller
    {
        private PJCEntities db = new PJCEntities();

        //
        // GET: /UserType/

        public ActionResult Index()
        {
            var usertypes = db.usertypes.Include(u => u.user);
            return View(usertypes.ToList());
        }

        //
        // GET: /UserType/Details/5

        public ActionResult Details(int id = 0)
        {
            usertype usertype = db.usertypes.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);
        }

        //
        // GET: /UserType/Create

        public ActionResult Create()
        {
            ViewBag.userTypeID = new SelectList(db.users, "userID", "firstName");
            return View();
        }

        //
        // POST: /UserType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(usertype usertype)
        {
            if (ModelState.IsValid)
            {
                db.usertypes.Add(usertype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.userTypeID = new SelectList(db.users, "userID", "firstName", usertype.userTypeID);
            return View(usertype);
        }

        //
        // GET: /UserType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            usertype usertype = db.usertypes.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            ViewBag.userTypeID = new SelectList(db.users, "userID", "firstName", usertype.userTypeID);
            return View(usertype);
        }

        //
        // POST: /UserType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(usertype usertype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usertype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.userTypeID = new SelectList(db.users, "userID", "firstName", usertype.userTypeID);
            return View(usertype);
        }

        //
        // GET: /UserType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            usertype usertype = db.usertypes.Find(id);
            if (usertype == null)
            {
                return HttpNotFound();
            }
            return View(usertype);
        }

        //
        // POST: /UserType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            usertype usertype = db.usertypes.Find(id);
            db.usertypes.Remove(usertype);
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