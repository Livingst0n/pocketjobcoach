using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using PJCAdmin.Models;


namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    public class AccountHelper
    {
        private pjcEntities db = new pjcEntities();

        public List<MembershipUser> getListOfUsersInRole(string role)
        {
            List<MembershipUser> lstUsers = new List<MembershipUser>();
            foreach (string userName in Roles.GetUsersInRole(role))
            {
                lstUsers.Add(System.Web.Security.Membership.GetUser(userName));
            }
            return lstUsers;
        }

        public List<MembershipUser> getListOfUsersAssignedToJobCoach(string jobCoachUserName)
        {
            List<MembershipUser> lstUsers = new List<MembershipUser>();

            //UserName12 is collection where self is jobcoach
            foreach (PJCAdmin.Models.UserName usr in db.UserNames.Find(jobCoachUserName).UserName12)
            {
                lstUsers.Add(System.Web.Security.Membership.GetUser(usr.userName1));
            }
            return lstUsers;
        }

        public List<MembershipUser> getListOfUsersChildOfParent(string parentUserName)
        {
            List<MembershipUser> lstUsers = new List<MembershipUser>();

            //UserName11 is collection where self is guardian
            foreach (PJCAdmin.Models.UserName usr in db.UserNames.Find(parentUserName).UserName11)
            {
                lstUsers.Add(System.Web.Security.Membership.GetUser(usr.userName1));
            }
            return lstUsers;
        }

        public List<MembershipUser> getListOfUnassignedUsers()
        {
            List<MembershipUser> users = getListOfUsersInRole("User");
            List<MembershipUser> unassignedUsers = new List<MembershipUser>();

            foreach (MembershipUser usr in users)
            {
                if (!isUserAssigned(usr))
                    unassignedUsers.Add(usr);
            }
            return unassignedUsers;
        }

        public List<MembershipUser> getListOfUnassignedChildren()
        {
            List<MembershipUser> users = getListOfUsersInRole("User");
            List<MembershipUser> unassignedChildren = new List<MembershipUser>();

            foreach (MembershipUser usr in users)
            {
                if (!isChildAssigned(usr))
                    unassignedChildren.Add(usr);
            }
            return unassignedChildren;
        }

        public MembershipUser getUsersJobCoach(string userUserName)
        {
            string jobCoachUserName = db.UserNames.Find(userUserName).jobCoachUserName;
            if (jobCoachUserName == null)
                return null;
            return System.Web.Security.Membership.GetUser(jobCoachUserName);
        }

        public MembershipUser getUsersParent(string userUserName)
        {
            string parentUserName = db.UserNames.Find(userUserName).guardianUserName;
            if (parentUserName == null)
                return null;
            return System.Web.Security.Membership.GetUser(parentUserName);
        }

        //TODO move to Routine controller
        public List<Routine> getListOfCreatedRoutines(string creatorUserName)
        {//TODO check if duplicated routines show up. They probably will and will need to be restricted to unique routine names
            List<Routine> createdRoutines = new List<Routine>();
            //Routines is routines username has created
            foreach (Routine r in db.UserNames.Find(creatorUserName).Routines)
            {
                createdRoutines.Add(r);
            }
            return createdRoutines;
        }

        //TODO move to Routine controller
        public List<Routine> getListOfAssignedRoutines(string assigneeUserName)
        {//TODO check if previous versions of routines show up. They probably will and will need to be restricted to unique routine names
            List<Routine> assignedRoutines = new List<Routine>();
            //Routines1 is routines username has been assigned
            foreach (Routine r in db.UserNames.Find(assigneeUserName).Routines1)
            {
                assignedRoutines.Add(r);
            }
            return assignedRoutines;
        }

        public EmailOutbox getEmailOutboxForPurpose(string purpose)
        {
            return db.EmailOutboxes.Where(s => s.purpose == purpose).FirstOrDefault();
        }

        public bool userExists(string userName)
        {
            MembershipUser targetUser = System.Web.Security.Membership.GetUser(userName);
            if (targetUser == null)
            {
                return false;
            }

            return true;
        }

        public bool isUserAssigned(MembershipUser usr)
        {
            string jobCoachUserName = db.UserNames.Find(usr.UserName).jobCoachUserName;
            if (jobCoachUserName == null)
                return false;
            if (!userExists(jobCoachUserName))
                return false;
            return true;
        }

        public bool isChildAssigned(MembershipUser usr)
        {
            string parentUserName = db.UserNames.Find(usr.UserName).guardianUserName;
            if (parentUserName == null)
                return false;
            if (!userExists(parentUserName))
                return false;
            return true;
        }

        public bool isThisUserUsersParent(string user)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            MembershipUser parent = getUsersParent(user);
            if (parent != null && parent.UserName.Equals(thisUsername))
                return true;
            else
                return false;
        }

        public bool isThisUserUsersJobCoach(string user)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            MembershipUser jobCoach = getUsersJobCoach(user);
            if (jobCoach != null && jobCoach.UserName.Equals(thisUsername))
                return true;
            else
                return false;
        }

        public void updateParent(string userName, string parent)
        {
            if (parent == null)
                db.UserNames.Find(userName).UserName2 = null;
            else
                //UserName2 is the guardian
                db.UserNames.Find(userName).UserName2 = db.UserNames.Find(parent);
            db.SaveChanges();
        }

        public void updateJobCoach(string userName, string jobCoach)
        {
            if (jobCoach == null)
                db.UserNames.Find(userName).UserName3 = null;
            else
                //UserName3 is the job coach
                db.UserNames.Find(userName).UserName3 = db.UserNames.Find(jobCoach);
            db.SaveChanges();
        }

        public void updateAssignedChildren(string userName, string[] assignedChildren)
        {
            //UserName11 is all children of the selected username
            db.UserNames.Find(userName).UserName11.Clear();
            if (assignedChildren != null)
            {
                foreach (string id in assignedChildren)
                {
                    string selectedUserName = db.Users.Find(Guid.Parse(id)).UserName;
                    db.UserNames.Find(userName).UserName11.Add(db.UserNames.Find(selectedUserName));
                }
            }
            db.SaveChanges();
        }

        public void updateAssignedUsers(string userName, string[] assignedUsers)
        {
            //UserName12 is all usernames assigned to the selected username
            db.UserNames.Find(userName).UserName12.Clear();
            if (assignedUsers != null)
            {
                foreach (string id in assignedUsers)
                {
                    string selectedUserName = db.Users.Find(Guid.Parse(id)).UserName;
                    db.UserNames.Find(userName).UserName12.Add(db.UserNames.Find(selectedUserName));
                }
            }
            db.SaveChanges();
        }

        public void updateUserRole(string userName, string userRole)
        {
            MembershipUser user = System.Web.Security.Membership.GetUser(userName);
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
            Roles.AddUserToRole(user.UserName, userRole);
            System.Web.Security.Membership.UpdateUser(user);
        }

        public void updateEmail(string userName, string emailAddress)
        {
            MembershipUser user = System.Web.Security.Membership.GetUser(userName);
            user.Email = emailAddress;
            System.Web.Security.Membership.UpdateUser(user);
        }

        public void updatePhoneNumber(string userName, string phoneNumber)
        {
            ProfileBase profile = ProfileBase.Create(userName, true);
            profile.SetPropertyValue("PhoneNumber", phoneNumber);
            profile.Save();
        }

        public void createUserNameRecord(object providerUserKey)
        {
            User user = db.Users.Find(providerUserKey);
            db.UserNames.Add(new UserName() { userID = user.UserId, userName1 = user.UserName });
            db.SaveChanges();
        }

        public void deleteUser(string userName)
        {
            //TODO delete all references to this user
            //deleteAllReferencesToUser(username);

            db.UserNames.Remove(db.UserNames.Find(userName));
            db.SaveChanges();

            System.Web.Security.Membership.DeleteUser(userName);
        }

        public static string getCurrentUsername()
        {
            return System.Web.Security.Membership.GetUser().UserName;
        }
    }
}