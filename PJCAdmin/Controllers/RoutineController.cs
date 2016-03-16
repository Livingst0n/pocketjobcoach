using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using PJCAdmin.Classes.Helpers.MVCModelHelpers;
using PJCAdmin.Models;

namespace PJCAdmin.Controllers
{
    public class RoutineController : Controller
    {
        private RoutineHelper helper = new RoutineHelper();
        private AccountHelper accountHelper = new AccountHelper();
        //Index = List all, Create, Details, Edit, Delete

        // GET: /Routine/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            string thisUsername = AccountHelper.getCurrentUsername();

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    //What should Admin see? A user select to continue?
                    ViewData["JobCoaches"] = accountHelper.getListOfUsersInRole("Job Coach");
                    ViewData["Parents"] = accountHelper.getListOfUsersInRole("Parent");
                    ViewData["mockUser"] = "";
                }
                else
                {
                    ViewData["mockUser"] = mockUser;
                    ViewData["Routines"] = helper.getMostRecentRoutines(mockUser);
                }
            }
            else //user is jobcoach or parent
            {
                ViewData["Routines"] = helper.getMostRecentRoutines();
            }

            return View();
        }

        public ActionResult ListByAssignedUser(string assignedTo, string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            string thisUsername = AccountHelper.getCurrentUsername();

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    ViewData["mockUser"] = mockUser;

                    if (assignedTo == null || !accountHelper.userExists(assignedTo))
                    {
                        if (Roles.IsUserInRole(mockUser, "Job Coach"))
                        {
                            ViewData["Assignees"] = accountHelper.getListOfUsersAssignedToJobCoach(mockUser);
                        }
                        else if (Roles.IsUserInRole(mockUser, "Parent"))
                        {
                            ViewData["Assignees"] = accountHelper.getListOfUsersChildOfParent(mockUser);
                        }
                    }
                    else
                    {
                        ViewData["AssignedTo"] = assignedTo;
                        ViewData["Routines"] = helper.getMostRecentRoutinesAssignedTo(mockUser, assignedTo);
                    }
                }
            }
            else //user is jobcoach or parent
            {
                if (assignedTo == null || !accountHelper.userExists(assignedTo))
                {
                    if (Roles.IsUserInRole("Job Coach"))
                    {
                        ViewData["Assignees"] = accountHelper.getListOfUsersAssignedToJobCoach(thisUsername);
                    }
                    else if (Roles.IsUserInRole("Parent"))
                    {
                        ViewData["Assignees"] = accountHelper.getListOfUsersChildOfParent(thisUsername);
                    }
                }
                else
                {
                    ViewData["AssignedTo"] = assignedTo;
                    ViewData["Routines"] = helper.getMostRecentRoutinesAssignedTo(assignedTo);
                }
            }

            return View();
        }

        public ActionResult Details(string routineName, string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    if (!helper.routineExists(mockUser, routineName))
                        return HttpNotFound();

                    ViewData["RoutineDetails"] = helper.getMostRecentRoutineByName(mockUser, routineName);
                    ViewData["mockUser"] = mockUser;
                    return View(helper.getMostRecentRoutineByName(mockUser, routineName));
                }
            }
            else //User is JobCoach or Parent
            {
                if (!helper.routineExists(routineName))
                    return HttpNotFound();

                ViewData["RoutineDetails"] = helper.getMostRecentRoutineByName(routineName);
                return View(helper.getMostRecentRoutineByName(routineName));
            }
        }

        public ActionResult Create(string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    ViewData["mockUser"] = mockUser;
                    if (Roles.IsUserInRole(mockUser, "Job Coach"))
                    {
                        ViewData["AssignedUsers"] = accountHelper.getListOfUsersAssignedToJobCoach(mockUser);
                    }
                    else if (Roles.IsUserInRole(mockUser, "Parent"))
                    {
                        ViewData["Children"] = accountHelper.getListOfUsersChildOfParent(mockUser);
                    }
                }
            }
            else
            {
                string thisUsername = AccountHelper.getCurrentUsername();

                if (Roles.IsUserInRole("Job Coach"))
                {
                    ViewData["AssignedUsers"] = accountHelper.getListOfUsersAssignedToJobCoach(thisUsername);
                }
                else if (Roles.IsUserInRole("Parent"))
                {
                    ViewData["Children"] = accountHelper.getListOfUsersChildOfParent(thisUsername);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RoutineModel model, string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    if (helper.routineExists(mockUser, model.routineTitle))
                    {
                        ModelState.AddModelError("", "Routine must have a unique name");
                        return View();
                    }

                    helper.createRoutine(mockUser, model);
                    return RedirectToAction("Index", "Routine", mockUser);
                }
            }

            if (helper.routineExists(model.routineTitle))
            {
                ModelState.AddModelError("", "Routine must have a unique name");
                return View();
            }

            helper.createRoutine(model);

            return RedirectToAction("Index", "Routine");
        }

        public ActionResult Edit(string routineName = "", string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    if (!helper.routineExists(mockUser, routineName))
                        return HttpNotFound();

                    ViewData["mockUser"] = mockUser;
                    ViewData["Routine"] = helper.getMostRecentRoutineByName(mockUser, routineName);
                    return View(helper.getMostRecentRoutineModelByName(mockUser, routineName));
                }
            }

            if (!helper.routineExists(routineName))
                return HttpNotFound();

            ViewData["Routine"] = helper.getMostRecentRoutineByName(routineName);
            return View(helper.getMostRecentRoutineModelByName(routineName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RoutineModel model, string routineName, string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    if (!helper.routineExists(mockUser, routineName))
                        return HttpNotFound();

                    helper.updateRoutine(mockUser, routineName, model);
                    return RedirectToAction("Index", "Routine", mockUser);
                }
            }

            if (!helper.routineExists(routineName))
                return HttpNotFound();

            helper.updateRoutine(routineName, model);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string routineName, string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    if (routineName == null || routineName.Equals(""))
                    {
                        Response.Redirect("~/Routine?mockUser=" + mockUser);
                        return View();
                    }

                    ViewData["mockUser"] = mockUser;
                    ViewData["Routine"] = routineName;
                    return View();
                }
            }

            if (routineName == null || routineName.Equals(""))
            {
                Response.Redirect("~/Routine");
                return View();
            }

            ViewData["Routine"] = routineName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string routineName, string mockUser = "", bool deleteAll = false, int nothing = 0)
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    Response.Redirect("~/Routine");
                    return View();
                }
                else
                {
                    helper.deleteRoutine(mockUser, routineName, deleteAll);

                    Response.Redirect("~/Routine?mockUser=" + mockUser);
                    return View();
                }
            }

            helper.deleteRoutine(routineName, deleteAll);

            Response.Redirect("~/Routine");
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            helper.dispose();
            accountHelper.dispose();
            base.Dispose(disposing);
        }
    }
}
