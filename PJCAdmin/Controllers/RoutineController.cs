﻿using System;
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

        public ActionResult Index(string mockUser = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            string thisUsername = System.Web.Security.Membership.GetUser().UserName;

            if (Roles.IsUserInRole("Administrator"))
            {
                if (mockUser == null || mockUser.Equals("") || !accountHelper.userExists(mockUser))
                {
                    //What should Admin see? A user select to continue?
                    ViewData["JobCoaches"] = accountHelper.getListOfUsersInRole("Job Coach");
                    ViewData["Parents"] = accountHelper.getListOfUsersInRole("Parent");
                }
                else
                {
                    ViewData["RoutineNames"] = helper.getRoutineNamesCreatedByUser(mockUser);
                }
            }
            else //user is jobcoach or parent
            {
                ViewData["RoutineNames"] = helper.getRoutineNamesCreatedByUser(thisUsername);
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
                    if (!helper.adminRoutineExists(mockUser, routineName))
                        return HttpNotFound();

                    ViewData["RoutineDetails"] = helper.adminGetRoutineByName(mockUser, routineName);
                }
            }
            else //User is JobCoach or Parent
            {
                if (!helper.routineExists(routineName))
                    return HttpNotFound();

                ViewData["RoutineDetails"] = helper.getRoutineByName(routineName);
            }
            return View();
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
                string thisUsername = System.Web.Security.Membership.GetUser().UserName;

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
                    if (helper.adminRoutineExists(mockUser, model.routineTitle))
                    {
                        ModelState.AddModelError("", "Routine must have a unique name");
                        return View();
                    }

                    helper.adminCreateRoutine(mockUser, model);
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
                    if (!helper.adminRoutineExists(mockUser, routineName))
                        return HttpNotFound();

                    ViewData["Routine"] = helper.adminGetRoutineByName(mockUser, routineName);
                    return View();
                }
            }

            if (!helper.routineExists(routineName))
                return HttpNotFound();

            ViewData["Routine"] = helper.getRoutineByName(routineName);
            return View();
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
                    if (!helper.adminRoutineExists(mockUser, routineName))
                        return HttpNotFound();

                    helper.adminUpdateRoutine(mockUser, routineName, model);
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
        public ActionResult Delete(string routineName, string mockUser = "", int nothing = 0)
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
                    helper.adminDeleteRoutine(mockUser, routineName);

                    Response.Redirect("~/Routine?mockUser=" + mockUser);
                    return View();
                }
            }

            helper.deleteRoutine(routineName);

            Response.Redirect("~/Routine");
            return View();
        }

    }
}