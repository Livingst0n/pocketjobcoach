using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PJCAdmin.Models;
using System.Data;
using PJCAdmin.Classes;
using System.Web.Profile;

namespace PJCMobile.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private pjcEntities db = new pjcEntities();
        //
        // GET: /Account/Index

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int id = 0)
        {
            if (Roles.IsUserInRole("Administrator"))
                return View(System.Web.Security.Membership.GetAllUsers());
            else if (Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent"))
            {
                List<MembershipUser> lstUsers = new List<MembershipUser>();
                string thisUsername = System.Web.Security.Membership.GetUser().UserName;
                
                if (Roles.IsUserInRole("Job Coach"))
                {
                    //foreach (PJCAdmin.Models.UserName usr in db.UserNames.Where(u => u.jobCoachUserName.Equals(thisUsername)))
                    //UserName12 is collection where self is jobcoach
                    foreach (PJCAdmin.Models.UserName usr in db.UserNames.Find(thisUsername).UserName12) 
                    {
                        lstUsers.Add(System.Web.Security.Membership.GetUser(usr.userName1));
                    }
                }
                
                if (Roles.IsUserInRole("Parent"))
                { 
                    //foreach (PJCAdmin.Models.UserName usr in db.UserNames.Where(u => u.guardianUserName.Equals(thisUsername)))
                    //UserName11 is collection where self is guardian
                    foreach (PJCAdmin.Models.UserName usr in db.UserNames.Find(thisUsername).UserName11)
                    {
                        lstUsers.Add(System.Web.Security.Membership.GetUser(usr.userName1));
                    }
                } //What if the user is both a parent and job coach of their own child? Duplicate entry here.

                return View(lstUsers);
            }
            else
                Response.Redirect("~/Unauthorized");
             
            return View();
        }

        public ActionResult Details(string user = "")
        {
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent"))
            {
                MembershipUser account = System.Web.Security.Membership.GetUser(user);
                if (account == null)
                {
                    return HttpNotFound();
                }
                List<MembershipUser> lstUsers = new List<MembershipUser>();
                
                //foreach (PJCAdmin.Models.UserName usr in db.UserNames.Where(u => u.jobCoachUserName.Equals(user)))
                foreach (PJCAdmin.Models.UserName usr in db.UserNames.Find(user).UserName12) //user is jobcoach
                {
                    lstUsers.Add(System.Web.Security.Membership.GetUser(usr.userName1));
                }

                ViewData["AssignedUsers"] = lstUsers;
                if (db.UserNames.Find(user).Routines1.Count > 0)
                    ViewData["SelectedRoutine"] = db.UserNames.Find(user).Routines1.ElementAt(0); //routines assigned to user
                else
                    ViewData["SelectedRoutine"] = new PJCAdmin.Models.Routine();
                return View(account);
            }
            else
                Response.Redirect("~/Unauthorized");
             
            return View();
        }

        public ActionResult Edit(string user = "")
        {
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent"))
            {
                string thisUsername = System.Web.Security.Membership.GetUser().UserName;

                MembershipUser account = System.Web.Security.Membership.GetUser(user);

                List<PJCAdmin.Models.User> selectedUsers = new List<PJCAdmin.Models.User>();
                foreach (PJCAdmin.Models.UserName usr in db.UserNames.Find(user).UserName12) //Only users for which this user is JC
                {
                    selectedUsers.Add(db.Users.Find(usr.userID));
                }
                ViewData["SelectedUsers"] = selectedUsers;
                ViewData["Users"] = db.Users.ToList();
                ViewData["Jobs"] = db.UserNames.Find(thisUsername).Routines.ToList(); //Only able to assign routines the current user has created
                if (db.UserNames.Find(user).Routines1.Count > 0)
                    ViewData["SelectedJob"] = db.UserNames.Find(user).Routines1.ElementAt(0);
                else
                    ViewData["SelectedJob"] = new PJCAdmin.Models.Routine();
                if (account == null)
                {
                    return HttpNotFound();
                }
                return View(account);
            }
            else
                Response.Redirect("~/Unauthorized");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string UserName, string Email, string usertype, string[] selectedUsers, string job, string applyJobTemplate, string phonenumber)
        {
            if (Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent"))
            {
                MembershipUser user = System.Web.Security.Membership.GetUser(UserName);

                ProfileBase profile = ProfileBase.Create(UserName,true);
                profile.SetPropertyValue("PhoneNumber", phonenumber);
                profile.Save();
                
                //Assigned Users
                db.UserNames.Find(UserName).UserName12.Clear(); //Only JC
                if (selectedUsers != null)
                {
                    foreach (string id in selectedUsers)
                    {
                        string selectedUserName = db.Users.Find(Guid.Parse(id)).UserName;
                        db.UserNames.Find(UserName).UserName12.Add(db.UserNames.Find(selectedUserName)); //Add user in JC collection
                    }
                }
                //Jobs
                db.UserNames.Find(UserName).Routines1.Clear(); //Currently allows removal of non-self routines
                if (job != "") { }
                db.UserNames.Find(UserName).Routines1.Add(db.Routines.Find(Convert.ToInt32(job)));


                /*if (Convert.ToBoolean(applyJobTemplate))
                {
                    foreach (PJCAdmin.Models.Task t in db.Routines.Find(Convert.ToInt32(job)).Tasks)
                    {
                        db.Users.Find(user.ProviderUserKey).usertasks.Add(new usertask { task = t, User = db.Users.Find(user.ProviderUserKey), daysOfWeek = "" });
                    }
                }*/ //Not using usertask, tasks are associated with the Routine itself.

                db.SaveChanges();
                user.Email = Email;
                System.Web.Security.Membership.UpdateUser(user);
                foreach (string aRole in Roles.GetAllRoles())
                {
                    //Only Let the user be in one role
                    try
                    {
                        Roles.RemoveUserFromRole(user.UserName, aRole);
                    }
                    catch
                    {
                        // Don't Worry About It.... :)
                    }
                }
                Roles.AddUserToRole(user.UserName, usertype);
                return RedirectToAction("List");
            }
            else
                Response.Redirect("~/Unauthorized");
            return View();
        }

        public ActionResult AdminResetPassword(string user)
        {
            if (ModelState.IsValid && Roles.IsUserInRole("Administrator"))
            {

                MembershipUser currentUser = System.Web.Security.Membership.GetUser(user);
                string newpassword = currentUser.ResetPassword();
                //Send email to user with new password
                try
                {
                    EmailOutbox outEmail = db.EmailOutboxes.Where(s => s.purpose == "password reset").FirstOrDefault();

                    string fromAddress = outEmail.emailAddress;;
                    string fromName = outEmail.emailName;
                    string password = outEmail.emailPassword;;
                    string emailBody = "Your password for the Pocket Job Coach has been reset to the temporary password '" + newpassword + "'. Please login and change your password now at http://pjc.gear.host";
                    string server = outEmail.smtpServerName;
                    int port = outEmail.portNumber;
                    int timeout = outEmail.smtpTimeout;
                    Email.send(fromAddress, fromName, currentUser.Email, "Pocket Job Coach Password Reset", emailBody, password, server, port, timeout);
                    Response.Redirect("~/Account/List");
                }
                catch (Exception e)
                {
                    db.Debugs.Add(new Debug() { debugMessage = e.ToString().Substring(0, 199) });
                    db.SaveChanges();
                    
                    Response.Redirect("~/Unauthorized");
                }
                ModelState.AddModelError("", "Password has been reset for " + currentUser.UserName);

            }
            else
                Response.Redirect("~/Unauthorized");
            return View();

        }

        public ActionResult Create()
        {
            if (ModelState.IsValid && Roles.IsUserInRole("Administrator"))
            {
                string thisUsername = System.Web.Security.Membership.GetUser().UserName;
                ViewData["Users"] = db.Users.ToList();
                ViewData["Jobs"] = db.UserNames.Find(thisUsername).Routines.ToList();

                return View();
            }
            else
                Response.Redirect("~/Unauthorized");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterModel model, string usertype, string[] selectedUsers, string job)
        {
            // Attempt to register the user
            MembershipCreateStatus createStatus;
            System.Web.Security.Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

            if (createStatus == MembershipCreateStatus.Success)
            {
                MembershipUser user = System.Web.Security.Membership.GetUser(model.UserName);

                ProfileBase profile = ProfileBase.Create(model.UserName,true);
                profile.SetPropertyValue("PhoneNumber", model.PhoneNumber);
                profile.Save();

                db.UserNames.Add(new UserName() {userID = db.Users.Find(user.ProviderUserKey).UserId, userName1 = user.UserName });

                db.UserNames.Find(model.UserName).UserName12.Clear(); //only JC
                if (selectedUsers != null)
                {
                    foreach (string id in selectedUsers)
                    {
                        string selectedUserName = db.Users.Find(Guid.Parse(id)).UserName;
                        db.UserNames.Find(model.UserName).UserName12.Add(db.UserNames.Find(selectedUserName));
                    }
                }
                //Job Management
                
                db.UserNames.Find(model.UserName).Routines1.Clear();
                if (job != "")
                    db.UserNames.Find(model.UserName).Routines1.Add(db.Routines.Find(Convert.ToInt32(job)));
                db.SaveChanges();

                foreach (string aRole in Roles.GetAllRoles())
                {
                    //Only Let the user be in one role
                    try
                    {
                        Roles.RemoveUserFromRole(model.UserName, aRole);
                    }
                    catch
                    {
                        // Don't Worry About It.... :)
                    }
                }
                Roles.AddUserToRole(model.UserName, usertype);
                return RedirectToAction("List", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Unable to create user!");
            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        public ActionResult Delete(string username)
        {
            if (username != "")
            {
                if (System.Web.Security.Membership.GetUser().UserName.Equals(username))
                {
                    Response.Redirect("~/Unauthorized");
                    return View();
                }

                ViewData["user"] = username;
                return View();
            }
            else
                Response.Redirect("~/Account/List");
            //Will Never Get here
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string username, int nothing = 0)
        {
            //Need to delete all references to this user
            db.UserNames.Remove(db.UserNames.Find(username));
            db.SaveChanges();

            System.Web.Security.Membership.DeleteUser(username);
            Response.Redirect("~/Account/List");
            //Will Never Get Here
            return View();
        }

        [HttpPost]
        public ActionResult Unlock(string username)
        {
            System.Web.Security.Membership.GetUser(username).UnlockUser();
            Response.Redirect("~/Account/List");
            return View();
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }

        //
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                if (System.Web.Security.Membership.ValidateUser(model.UserName, model.Password))
                {
                    if (!Roles.IsUserInRole(model.UserName, "User"))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "This login can only be used with the Pocket Job Coach Mobile App.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = System.Web.Security.Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult AssignedTasks(string user)
        {
            //ViewData["User"] = System.Web.Security.Membership.GetUser(user);
            //ViewData["Tasks"] = db.Users.Find(System.Web.Security.Membership.GetUser(user).ProviderUserKey).usertasks.ToList();

            return View();
        }

        public ActionResult RemoveAssignedTask(string username, int taskid)
        {
            /*if (username != "")
            {
                ViewData["user"] = username;
                ViewData["task"] = db.tasks.Find(taskid);
                return View();
            }
            else
                Response.Redirect("~/Account/AssignedTasks");
            //Will Never Get here
             */
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAssignedTask(string username, int taskid, int nothing = 0)
        {
            /*db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey).usertaskprompts.ToList().RemoveAll(delegate(usertaskprompt p)
            {
                return p.taskID == taskid;
            });
            db.usertasks.Remove(db.usertasks.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey,taskid));
            db.SaveChanges();
            Response.Redirect("~/Account/AssignedTasks?user=" + username);
            //Will Never Get Here
             */
            return View();
        }

        public ActionResult ManagePrompts(string username, int taskid)
        {
            /*ViewBag.Task = db.tasks.Find(taskid).taskName;
            ViewBag.Username = username;
            ViewData["Prompts"] = db.tasks.Find(taskid).prompts.ToList();
            List<prompt> selectedPrompts = new List<prompt>();
            foreach( usertaskprompt p in db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey).usertaskprompts.ToList().FindAll(delegate(usertaskprompt prompt)
            {
                return prompt.taskID == taskid;
            }).ToList()){
                selectedPrompts.Add(p.prompt);
            }
            ViewData["SelectedPrompts"] = selectedPrompts;
             * */
            return View();
        }

        [HttpPost]
        public ActionResult ManagePrompts(string username, int taskid, string[] prompts, string[] promptsh)
        {
            /*List<usertaskprompt> p = db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey).usertaskprompts.ToList().FindAll(delegate(usertaskprompt prompt)
            {
                return prompt.taskID == taskid;
            }).ToList();

            foreach (usertaskprompt utp in p)
            {
                db.usertaskprompts.Remove(db.usertaskprompts.Find(utp.userID,utp.taskID,utp.promptID));
            }

            db.SaveChanges();

            foreach (string id in promptsh)
            {
                //Create a new UserTaskPrompt
                if (id != "0")
                {
                    usertaskprompt utp = new usertaskprompt();
                    utp.prompt = db.prompts.Find(Convert.ToInt32(id));
                    utp.task = db.tasks.Find(Convert.ToInt32(taskid));
                    utp.User = db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey);
                    db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey).usertaskprompts.Add(utp);
                }
            }

            db.SaveChanges();
            Response.Redirect("~/Account/AssignedTasks?user=" + username);
             */
            return View();
        }

        public ActionResult EditAssignedTask(string username, int taskID)
        {
            /*Guid userid = (Guid) System.Web.Security.Membership.GetUser(username).ProviderUserKey;
            int taskid = Convert.ToInt32(taskID);
            usertask ut = db.usertasks.Find(userid,taskid);
            return View("EditAssignedTask", ut);
             */
            return View();
        }

        [HttpPost]
        public ActionResult EditAssignedTask(string userid, int taskID, string[] daysofweek, string starttime, string endtime, string feedback)
        {
            /*Guid user = Guid.Parse(userid);
            int taskid = Convert.ToInt32(taskID);
            usertask ut = db.usertasks.Find(user, taskid);
            string schedule = "";
            foreach (string day in daysofweek)
            {
                if (day != "false")
                    schedule = schedule + day;
            }
            ut.daysOfWeek = schedule;
            if (starttime != "")
            {
                ut.startTime = Convert.ToDateTime(starttime);
            }
            if (endtime != "")
            {
                ut.endTime = Convert.ToDateTime(endtime);
            }
            ut.feedbackMessage = feedback;

            db.SaveChanges();
            Response.Redirect("~/Account/AssignedTasks?user=" + ut.User.UserName);
             * */
            return View();
        }

        public ActionResult AddAssignedTask(string username)
        {
            /*ViewData["Username"] = username;
            List<task> t = new List<task>();
            foreach (usertask ut in db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey).usertasks)
            {
                t.Add(ut.task);
            }
            ViewData["Tasks"] = db.tasks.ToList().Except(t).ToList();
             */
            return View();
        }

        [HttpPost]
        public ActionResult AddAssignedTask(string username, int taskID, string[] daysofweek, string starttime, string endtime, string feedback)
        {
            /*usertask ut = new usertask();
            string schedule = "";
            foreach (string day in daysofweek)
            {
                if (day != "false")
                    schedule = schedule + day;
            }
            ut.daysOfWeek = schedule;
            if (starttime != "")
            {
                ut.startTime = Convert.ToDateTime(starttime);
            }
            if (endtime != "")
            {
                ut.endTime = Convert.ToDateTime(endtime);
            }
            ut.feedbackMessage = feedback;
            ut.User = db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey);
            ut.task = db.tasks.Find(Convert.ToInt32(taskID));

            db.Users.Find(System.Web.Security.Membership.GetUser(username).ProviderUserKey).usertasks.Add(ut);
            db.SaveChanges();
            Response.Redirect("~/Account/AssignedTasks?user=" + username);
             */
            return View();
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
