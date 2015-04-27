using PJCMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Profile;

namespace PJCMobile.Controllers
{
    public class ContactController : Controller
    {

        private pjcEntities db = new pjcEntities();

        //
        // GET: /Contact/

        public ActionResult Index()
        {
            User user = new User();
            if (db.Users.Find(System.Web.Security.Membership.GetUser().ProviderUserKey).Users1.Count > 0)
            {
                user = db.Users.Find(System.Web.Security.Membership.GetUser().ProviderUserKey).Users1.ToList().FindAll(delegate(User u)
                {
                    return Roles.GetRolesForUser(u.UserName).ToList().Contains("Job Coach");
                }).First();
            }


            return View("Index", user);
        }

        //
        // GET: /Contact/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Contact/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Contact/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contact/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Contact/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Contact/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Contact/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
