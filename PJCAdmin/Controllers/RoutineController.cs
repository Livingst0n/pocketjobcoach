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

        public ActionResult Details(string routineName, string assigneeName, string mockUser = "")
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
                    if (!helper.routineExists(mockUser, routineName, assigneeName))
                        return HttpNotFound();

                    ViewData["RoutineDetails"] = helper.getMostRecentRoutineAssignedToByName(mockUser, routineName, assigneeName);
                    ViewData["mockUser"] = mockUser;
                    return View(helper.getMostRecentRoutineAssignedToByName(mockUser, routineName, assigneeName));
                }
            }
            else //User is JobCoach or Parent
            {
                if (!helper.routineExists(routineName, assigneeName))
                    return HttpNotFound();

                ViewData["RoutineDetails"] = helper.getMostRecentRoutineAssignedToByName(routineName, assigneeName);
                return View(helper.getMostRecentRoutineAssignedToByName(routineName, assigneeName));
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
                        ViewData["Assignees"] = accountHelper.getListOfUsersAssignedToJobCoach(mockUser);
                    }
                    else if (Roles.IsUserInRole(mockUser, "Parent"))
                    {
                        ViewData["Assignees"] = accountHelper.getListOfUsersChildOfParent(mockUser);
                    }
                }
            }
            else
            {
                string thisUsername = AccountHelper.getCurrentUsername();

                if (Roles.IsUserInRole("Job Coach"))
                {
                    ViewData["Assignees"] = accountHelper.getListOfUsersAssignedToJobCoach(thisUsername);
                }
                else if (Roles.IsUserInRole("Parent"))
                {
                    ViewData["Assignees"] = accountHelper.getListOfUsersChildOfParent(thisUsername);
                }
            }

            EnumHelper enumhelp = new EnumHelper();
            ViewData["TaskCategories"] = enumhelp.getAllTaskCategoryNames().ToList();
            ViewData["FeedbackTypes"] = enumhelp.getAllFeedbackTypeNames().ToList();
            ViewData["MediaTypes"] = enumhelp.getAllMediaTypeNames().ToList();

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
            
            EnumHelper enumhelp = new EnumHelper();
            ViewData["TaskCategories"] = enumhelp.getAllTaskCategoryNames().ToList();
            ViewData["FeedbackTypes"] = enumhelp.getAllFeedbackTypeNames().ToList();
            ViewData["MediaTypes"] = enumhelp.getAllMediaTypeNames().ToList();

            if (Roles.IsUserInRole("Administrator"))
            {
                ViewData["mockUser"] = mockUser;
                if (Roles.IsUserInRole(mockUser, "Job Coach"))
                {
                    ViewData["Assignees"] = accountHelper.getListOfUsersAssignedToJobCoach(mockUser);
                }
                else if (Roles.IsUserInRole(mockUser, "Parent"))
                {
                    ViewData["Assignees"] = accountHelper.getListOfUsersChildOfParent(mockUser);
                }
            }
            else if (Roles.IsUserInRole("Job Coach"))
            {
                string thisUsername = AccountHelper.getCurrentUsername();
                ViewData["Assignees"] = accountHelper.getListOfUsersAssignedToJobCoach(thisUsername);
            }
            else if (Roles.IsUserInRole("Parent"))
            {
                string thisUsername = AccountHelper.getCurrentUsername();
                ViewData["Assignees"] = accountHelper.getListOfUsersChildOfParent(thisUsername);
            }

            if (!(ModelState.IsValid))
            {
                return View(model);
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
                    if (helper.routineExists(mockUser, model.routineTitle, model.assigneeUserName))
                    {
                        ModelState.AddModelError("", "Routine must have a unique name");
                        return View(model);
                    }

                    helper.createRoutine(mockUser, model);
                    return RedirectToAction("List", "Routine", new { mockUser = mockUser });
                }
            }

            if (helper.routineExists(model.routineTitle, model.assigneeUserName))
            {
                ModelState.AddModelError("", "Routine must have a unique name");
                return View(model);
            }

            helper.createRoutine(model);

            return RedirectToAction("List", "Routine");
        }

        public ActionResult Edit(string routineName, string assigneeName, string mockUser = "")
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
                    if (!helper.routineExists(mockUser, routineName, assigneeName))
                        return HttpNotFound();

                    ViewData["mockUser"] = mockUser;
                    ViewData["Routine"] = helper.getMostRecentRoutineModelAssignedToByName(mockUser, routineName, assigneeName);
                }
            }
            else
            {

                if (!helper.routineExists(routineName, assigneeName))
                    return HttpNotFound();

                ViewData["Routine"] = helper.getMostRecentRoutineModelAssignedToByName(routineName, assigneeName);
            }

            EnumHelper enumhelp = new EnumHelper();
            ViewData["TaskCategories"] = enumhelp.getAllTaskCategoryNames().ToList();
            ViewData["FeedbackTypes"] = enumhelp.getAllFeedbackTypeNames().ToList();
            ViewData["MediaTypes"] = enumhelp.getAllMediaTypeNames().ToList();

            return View(ViewData["Routine"]);
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

            if (!ModelState.IsValid)
            {
                EnumHelper enumhelp = new EnumHelper();
                ViewData["TaskCategories"] = enumhelp.getAllTaskCategoryNames().ToList();
                ViewData["FeedbackTypes"] = enumhelp.getAllFeedbackTypeNames().ToList();
                ViewData["MediaTypes"] = enumhelp.getAllMediaTypeNames().ToList();

                ViewData["mockUser"] = mockUser;

                return View(model);
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
                    if (!helper.routineExists(mockUser, routineName, model.assigneeUserName))
                        return HttpNotFound();

                    helper.updateRoutine(mockUser, routineName, model);
                    return RedirectToAction("List", "Routine", new { mockUser = mockUser });
                }
            }

            if (!helper.routineExists(routineName, model.assigneeUserName))
                return HttpNotFound();

            helper.updateRoutine(routineName, model);

            return RedirectToAction("List");
        }

        public ActionResult Delete(string routineName, string assigneeName, string mockUser = "")
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
                    ViewData["Assignee"] = assigneeName;
                    return View();
                }
            }

            if (routineName == null || routineName.Equals(""))
            {
                Response.Redirect("~/Routine");
                return View();
            }

            ViewData["Routine"] = routineName;
            ViewData["Assignee"] = assigneeName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string routineName, string assigneeName, bool deleteAll = false, string mockUser = "", int nothing = 0)
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
                    helper.deleteRoutine(mockUser, routineName, assigneeName, deleteAll);

                    Response.Redirect("~/Routine/List?mockUser=" + mockUser);
                    return View();
                }
            }

            helper.deleteRoutine(routineName, assigneeName, deleteAll);

            Response.Redirect("~/Routine/List");
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
