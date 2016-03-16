using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PJCAdmin.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(List<string> users)
        {
            if (!(users == null))
            {
                ViewData["Users"] = users;
                return View(users);
            }

            List<string> availableUsers = new List<string>();
            availableUsers.Add("Chris");
            availableUsers.Add("Bob");
            availableUsers.Add("Joe");
            availableUsers.Add("Steve");
            availableUsers.Add("Jeff");
            ViewData["Users"] = availableUsers;
            return View(availableUsers);
        }

        public ActionResult Edit()
        {
            List<string> availableUsers = new List<string>();
            availableUsers.Add("Chris");
            availableUsers.Add("Bob");
            availableUsers.Add("Joe");
            availableUsers.Add("Steve");
            availableUsers.Add("Jeff");
            ViewData["AvailableUsers"] = availableUsers;

            return View();
        }

        [HttpPost]
        public ActionResult Edit(string[] selectedUsers)
        {
            List<string> Users = new List<string>();
            foreach (string user in selectedUsers)
            {
                Users.Add(user);
            }

            return View("List",Users);
        }

    }
}
