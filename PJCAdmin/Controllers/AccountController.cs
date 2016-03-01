using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PJCAdmin.Models;
using PJCAdmin.Classes;
using PJCAdmin.Classes.Helpers;
using PJCAdmin.Classes.Helpers.MVCModelHelpers;

namespace PJCMobile.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        private pjcEntities db = new pjcEntities();
        private AccountHelper helper = new AccountHelper();
        private DebugHelper debug = new DebugHelper();
        //
        // GET: /Account/Index

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int id = 0)
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (Roles.IsUserInRole("Administrator"))
            {
                ViewData["Admins"] = helper.getListOfUsersInRole("Administrator");
                ViewData["Job Coaches"] = helper.getListOfUsersInRole("Job Coach");
                ViewData["Parents"] = helper.getListOfUsersInRole("Parent");
                ViewData["Users"] = helper.getListOfUsersInRole("User");
                
                return View(System.Web.Security.Membership.GetAllUsers());
            }

            List<MembershipUser> lstUsers = new List<MembershipUser>();
            string thisUsername = AccountHelper.getCurrentUsername();
            
            if (Roles.IsUserInRole("Job Coach"))
            {
                lstUsers = helper.getListOfUsersAssignedToJobCoach(thisUsername);
                ViewData["AssignedUsers"] = lstUsers;
            }

            if (Roles.IsUserInRole("Parent"))
            {
                lstUsers = helper.getListOfUsersChildOfParent(thisUsername);
                ViewData["Children"] = lstUsers;
            }
            
            return View(lstUsers);
        }

        public ActionResult Details(string user = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (!(Roles.IsUserInRole("Administrator") || helper.isThisUserUsersParent(user) || helper.isThisUserUsersJobCoach(user)))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (!helper.userExists(user))
                return HttpNotFound();

            if (Roles.IsUserInRole(user,"Administrator")){
                ViewData["Role"] = "Administrator";
            }
            else if (Roles.IsUserInRole(user,"Job Coach")){
                ViewData["Role"] = "Job Coach";
                ViewData["AssignedUsersNew"] = helper.getListOfUsersAssignedToJobCoach(user);
                //ViewData["CreatedRoutines"] = getListOfCreatedRoutines(user);
            }
            else if (Roles.IsUserInRole(user,"Parent")){
                ViewData["Role"] = "Parent";
                ViewData["Children"] = helper.getListOfUsersChildOfParent(user);
                //ViewData["CreatedRoutines"] = getListOfCreatedRoutines(user);
            }
            else if (Roles.IsUserInRole(user,"User")){
                ViewData["Role"] = "User";
                ViewData["Job Coach"] = helper.getUsersJobCoach(user);
                ViewData["Parent"] = helper.getUsersParent(user);
                //ViewData["AssignedRoutines"] = getListOfAssignedRoutines(user);
            }

            //Below Here
            ViewData["AssignedUsers"] = helper.getListOfUsersAssignedToJobCoach(user);

            if (db.UserNames.Find(user).Routines1.Count > 0)
                ViewData["SelectedRoutine"] = db.UserNames.Find(user).Routines1.ElementAt(0); //routines assigned to user
            else
                ViewData["SelectedRoutine"] = new PJCAdmin.Models.Routine();
            return View(System.Web.Security.Membership.GetUser(user));
        }

        public ActionResult Edit(string user = "")
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (!(Roles.IsUserInRole("Administrator") || helper.isThisUserUsersParent(user) || helper.isThisUserUsersJobCoach(user)))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            string thisUsername = AccountHelper.getCurrentUsername();

            if (!helper.userExists(user))
                return HttpNotFound();

            if (Roles.IsUserInRole("Administrator")){
                ViewData["AvailableJobCoaches"] = helper.getListOfUsersInRole("Job Coach");
                ViewData["AvailableParents"] = helper.getListOfUsersInRole("Parent");
                ViewData["AvailableUsers"] = helper.getListOfUnassignedUsers();
                ViewData["AvailableChildren"] = helper.getListOfUnassignedChildren();
            }
            else if (Roles.IsUserInRole("Job Coach")){
                //ViewData["AvailableRoutines"] = getListOfCreatedRoutines(thisUsername);
            }
            else if (Roles.IsUserInRole("Parent")){
                //ViewData["AvailableRoutines"] = getListOfCreatedRoutines(thisUsername);
            }

            if (Roles.IsUserInRole(user, "Administrator")){
                ViewData["Role"] = "Administrator";
            }
            else if (Roles.IsUserInRole(user, "Job Coach"))
            {
                ViewData["Role"] = "Job Coach";
                ViewData["AssignedUsers"] = helper.getListOfUsersAssignedToJobCoach(user);
            }
            else if (Roles.IsUserInRole(user, "Parent"))
            {
                ViewData["Role"] = "Parent";
                ViewData["Children"] = helper.getListOfUsersChildOfParent(user);
            }
            else if (Roles.IsUserInRole(user, "User"))
            {
                ViewData["Role"] = "User";
                ViewData["JobCoach"] = helper.getUsersJobCoach(user);
                ViewData["Parent"] = helper.getUsersParent(user);
            }

            //Below Here
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

            return View(System.Web.Security.Membership.GetUser(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit(string userName, string email, string phoneNumber, string userRole, string[] assignedUsers, string[] assignedChildren, string jobCoach, string parent) 
        public ActionResult Edit(string userName, string email, string userType, string[] selectedUsers, string job, string applyJobTemplate, string phoneNumber)
        {
            if (!(Roles.IsUserInRole("Administrator") || Roles.IsUserInRole("Job Coach") || Roles.IsUserInRole("Parent")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (!(Roles.IsUserInRole("Administrator") || helper.isThisUserUsersParent(userName) || helper.isThisUserUsersJobCoach(userName)))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            helper.updatePhoneNumber(userName, phoneNumber);
            helper.updateEmail(userName, email);
            
            if (Roles.IsUserInRole("Administrator"))
            {
                string userRole = userType;
                helper.updateUserRole(userName, userRole);

                helper.updateAssignedUsers(userName, null);
                //updateAssignedChildren(userName, null);
                //updateJobCoach(userName, null);
                //updateParent(userName, null);

                if (Roles.IsUserInRole(userName, "Administrator")){
                    //placeholder
                }
                else if (Roles.IsUserInRole(userName, "Job Coach")){
                    string[] assignedUsers = selectedUsers;
                    helper.updateAssignedUsers(userName, assignedUsers);
                }
                else if (Roles.IsUserInRole(userName, "Parent")){
                    //updateAssignedChildren(userName, assignedChildren);
                }
                else if (Roles.IsUserInRole(userName, "User")){
                    //updateJobCoach(userName, jobCoach);
                    //updateParent(userName, parent);
                }
            }
            
            //Below Here
            //Jobs
            db.UserNames.Find(userName).Routines1.Clear(); //Currently allows removal of non-self routines
            if (job != "") { }
            db.UserNames.Find(userName).Routines1.Add(db.Routines.Find(Convert.ToInt32(job)));

            /*if (Convert.ToBoolean(applyJobTemplate))
            {
                foreach (PJCAdmin.Models.Task t in db.Routines.Find(Convert.ToInt32(job)).Tasks)
                {
                    db.Users.Find(user.ProviderUserKey).usertasks.Add(new usertask { task = t, User = db.Users.Find(user.ProviderUserKey), daysOfWeek = "" });
                }
            }*/ //Not using usertask, tasks are associated with the Routine itself.

            db.SaveChanges();
            
            return RedirectToAction("List");
        }

        public ActionResult AdminResetPassword(string user)
        {
            if (!(ModelState.IsValid && Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            MembershipUser currentUser = System.Web.Security.Membership.GetUser(user);
            
            if (currentUser.IsLockedOut)
                currentUser.UnlockUser();

            string newpassword = currentUser.ResetPassword();
            //Send email to user with new password
            try
            {
                EmailOutbox outEmail = helper.getEmailOutboxForPurpose("password reset");

                string emailBody = "Your password for the Pocket Job Coach has been reset to the temporary password '" + newpassword + "'. Please login and change your password now at http://pjc.gear.host";
                Email.send(outEmail, currentUser.Email, "Pocket Job Coach Password Reset", emailBody);

                Response.Redirect("~/Account/List");
            }
            catch (Exception e)
            {
                debug.createDebugMessageInDatabase(e.ToString());
                    
                Response.Redirect("~/Unauthorized");
            }
            ModelState.AddModelError("", "Password has been reset for " + currentUser.UserName);

            return View();
        }

        public ActionResult Create()
        {
            if (!(ModelState.IsValid && Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            ViewData["AvailableJobCoaches"] = helper.getListOfUsersInRole("Job Coach");
            ViewData["AvailableParents"] = helper.getListOfUsersInRole("Parent");
            ViewData["AvailableUsers"] = helper.getListOfUnassignedUsers();
            ViewData["AvailableChildren"] = helper.getListOfUnassignedChildren();

            //Below Here
            string thisUsername = AccountHelper.getCurrentUsername();
            ViewData["Users"] = db.Users.ToList();
            ViewData["Jobs"] = db.UserNames.Find(thisUsername).Routines.ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(RegisterModel model, string userRole, string[] assignedUsers, string[] assignedChildren, string jobCoach, string parent)
        public ActionResult Create(RegisterModel model, string usertype, string[] selectedUsers, string job)
        {
            if (!(Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            // Attempt to register the user
            MembershipCreateStatus createStatus;
            System.Web.Security.Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                ModelState.AddModelError("", "Unable to create user!");

                // If we got this far, something failed, redisplay form
                return View();
            }
                
            MembershipUser user = System.Web.Security.Membership.GetUser(model.UserName);

            helper.createUserNameRecord(user.ProviderUserKey);
            helper.updatePhoneNumber(model.UserName, model.PhoneNumber);
            string userRole = usertype;
            helper.updateUserRole(model.UserName, userRole);
                
            if (Roles.IsUserInRole(model.UserName, "Administrator"))
            {
                //placeholder
            }
            else if (Roles.IsUserInRole(model.UserName, "Job Coach"))
            {
                string[] assignedUsers = selectedUsers;
                helper.updateAssignedUsers(model.UserName, assignedUsers);
            }
            else if (Roles.IsUserInRole(model.UserName, "Parent"))
            {
                //updateAssignedChildren(model.UserName, assignedChildren);
            }
            else if (Roles.IsUserInRole(model.UserName, "User"))
            {
                //updateJobCoach(model.UserName, jobCoach);
                //updateParent(model.UserName, parent);
            }

            //Below Here
            //Job Management
                
            db.UserNames.Find(model.UserName).Routines1.Clear();
            if (job != "")
                db.UserNames.Find(model.UserName).Routines1.Add(db.Routines.Find(Convert.ToInt32(job)));
            db.SaveChanges();

            return RedirectToAction("List", "Account");
        }

        public ActionResult Delete(string username)
        {
            if (!(Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (username == null || username.Equals(""))
            {
                Response.Redirect("~/Account/List");
                return View();
            }

            if (System.Web.Security.Membership.GetUser().UserName.Equals(username))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            ViewData["user"] = username;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string username, int nothing = 0)
        {
            if (!(Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            helper.deleteUser(username);

            Response.Redirect("~/Account/List");
            //Will Never Get Here
            return View();
        }

        public ActionResult Unlock(string username)
        {
            if (!(Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

            if (username == null || username.Equals(""))
            {
                Response.Redirect("~/Account/List");
                return View();
            }

            ViewData["user"] = username;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Unlock(string username, int nothing = 0)
        {
            if (!(Roles.IsUserInRole("Administrator")))
            {
                Response.Redirect("~/Unauthorized");
                return View();
            }

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
            if (!(ModelState.IsValid))
                return View(model);

            if (!(System.Web.Security.Membership.ValidateUser(model.UserName, model.Password)))
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
                return View(model);
            }

            if (Roles.IsUserInRole(model.UserName, "User"))
            {
                ModelState.AddModelError("", "This login can only be used with the Pocket Job Coach Mobile App.");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
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
            if (!(ModelState.IsValid))
                return View(model);

            // ChangePassword will throw an exception rather
            // than return false in certain failure scenarios.
            bool changePasswordSucceeded;
            try
            {
                MembershipUser currentUser = System.Web.Security.Membership.GetUser(User.Identity.Name, userIsOnline: true);
                changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
            }
            catch (Exception){
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
                return RedirectToAction("ChangePasswordSuccess");
            else
            {
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                return View(model);
            }
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //Below Here
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            helper.dispose();
            debug.dispose();
            base.Dispose(disposing);
        }
    }
}
