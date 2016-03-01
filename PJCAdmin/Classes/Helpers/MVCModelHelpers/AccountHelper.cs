using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using PJCAdmin.Models;


namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* --------------------------------------------------------
     * The AccountHelper class provides common methods relating 
     * to User Accounts, Parent and JobCoach Assignments,
     * and EmailOutboxes for the MVC service.
     * --------------------------------------------------------
     */
    public class AccountHelper
    {
        private pjcEntities db = new pjcEntities();

        #region User Accounts
        /* Returns a list of all users of the given role.
         * Valid role examples are "Administrator",
         * "Job Coach", "Parent", and "User". Where "User" 
         * defines users only allowed to use the mobile 
         * app or mobile website.
         * @param role: The name of the role desired in 
         * string format.
         */
        public List<MembershipUser> getListOfUsersInRole(string role)
        {
            List<MembershipUser> lstUsers = new List<MembershipUser>();
            foreach (string userName in Roles.GetUsersInRole(role))
            {
                lstUsers.Add(System.Web.Security.Membership.GetUser(userName));
            }
            return lstUsers;
        }
        /* Returns the username for the user
         * currently logged in.
         */
        public static string getCurrentUsername()
        {
            return System.Web.Security.Membership.GetUser().UserName;
        }
        /* Returns whether or not a user exists with the
         * given username.
         * @param userName: The username of the user in 
         * question.
         */
        public bool userExists(string userName)
        {
            MembershipUser targetUser = System.Web.Security.Membership.GetUser(userName);
            if (targetUser == null)
            {
                return false;
            }

            return true;
        }
        /* Changes the role of a given user.
         * @param userName: The username of the user for which
         * to change the role.
         * @param userRole: The name of the new role the user 
         * should be assigned.
         */
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
        /* Updates the email address on file for the
         * given username.
         * @param userName: The username of the user whose
         * email address should be updated.
         * @param emailAddress: The new email address for 
         * the user.
         */
        public void updateEmail(string userName, string emailAddress)
        {
            MembershipUser user = System.Web.Security.Membership.GetUser(userName);
            user.Email = emailAddress;
            System.Web.Security.Membership.UpdateUser(user);
        }
        /* Updates the phone number on file for the 
         * given user.
         * @param userName: The username of the user whose
         * phonenumber should be changed.
         * @param phoneNumber: The new phone number to be
         * associated with the user.
         */
        public void updatePhoneNumber(string userName, string phoneNumber)
        {
            ProfileBase profile = ProfileBase.Create(userName, true);
            profile.SetPropertyValue("PhoneNumber", phoneNumber);
            profile.Save();
        }
        /* Creates a record for the given user in the
         * UserName table. This table connects the 
         * Membership database and the PJC database.
         * This record is required for both MVC and 
         * WebAPI to have access to the PJC database.
         * @param providerUserKey: The unique identifier
         * of the user. This can be obtained by the
         * MembershipUser.ProviderUserKey data member
         * for the user's MembershipUser.
         */
        public void createUserNameRecord(object providerUserKey)
        {
            User user = db.Users.Find(providerUserKey);
            db.UserNames.Add(new UserName() { userID = user.UserId, userName1 = user.UserName });
            db.SaveChanges();
        }
        /* Removes a given user from the PJC system.
         * @param userName: The username of the user
         * to be deleted.
         */
        public void deleteUser(string userName)
        {
            //TODO delete all references to this user
            //deleteAllReferencesToUser(username);

            db.UserNames.Remove(db.UserNames.Find(userName));
            db.SaveChanges();

            System.Web.Security.Membership.DeleteUser(userName);
        }
        #endregion
        #region JobCoach Assignment
        /* Returns a list of all assignees of 
         * a given job coach.
         * @param jobCoachUserName: The job coach's
         * username.
         */
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
        /* Returns a list of all assignees who are not
         * assigned to a job coach.
         */
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
        /* Returns the job coach of the given assignee.
         * @param userName: The username of the assignee
         * whose job coach is desired.
         */
        public MembershipUser getUsersJobCoach(string userName)
        {
            string jobCoachUserName = db.UserNames.Find(userName).jobCoachUserName;
            if (jobCoachUserName == null)
                return null;
            return System.Web.Security.Membership.GetUser(jobCoachUserName);
        }
        /* Returns whether or not the currently logged in
         * user is the given user's job coach.
         * @param userName: The username of the assignee.
         */
        public bool isThisUserUsersJobCoach(string userName)
        {
            string thisUsername = getCurrentUsername();
            MembershipUser jobCoach = getUsersJobCoach(userName);
            if (jobCoach != null && jobCoach.UserName.Equals(thisUsername))
                return true;
            else
                return false;
        }
        /* Updates the job coach for the given user.
         * @param userName: The assignee's username.
         * @param jobCoach: The job coach's username.
         */
        public void updateJobCoach(string userName, string jobCoach)
        {
            if (jobCoach == null)
                db.UserNames.Find(userName).UserName3 = null;
            else
                //UserName3 is the job coach
                db.UserNames.Find(userName).UserName3 = db.UserNames.Find(jobCoach);
            db.SaveChanges();
        }
        /* Updates the list of users assigned to the
         * given job coach.
         * @param userName: The job coach's username.
         * @param assignedUsers: An array of assignees'
         * usernames.
         */
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
        #endregion
        #region Parent Assignment
        /* Returns a list of all children of a
         * given parent.
         * @param parentUserName: The parent's 
         * username.
         */
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
        /* Returns a list of all children who are not
         * assigned to a parent.
         */
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
        /* Returns the parent of a given child.
         * @param userName: The username of the child
         * whose parent is desired.
         */
        public MembershipUser getUsersParent(string userUserName)
        {
            string parentUserName = db.UserNames.Find(userUserName).guardianUserName;
            if (parentUserName == null)
                return null;
            return System.Web.Security.Membership.GetUser(parentUserName);
        }
        /* Returns whether or not the currently logged in
         * user is the given user's parent.
         * @param userName: The username of the child.
         */
        public bool isThisUserUsersParent(string userName)
        {
            string thisUsername = getCurrentUsername();
            MembershipUser parent = getUsersParent(userName);
            if (parent != null && parent.UserName.Equals(thisUsername))
                return true;
            else
                return false;
        }
        /* Updates the parent for the given user.
         * @param userName: The child's username.
         * @param parent: the parent's username.
         */
        public void updateParent(string userName, string parent)
        {
            if (parent == null)
                db.UserNames.Find(userName).UserName2 = null;
            else
                //UserName2 is the guardian
                db.UserNames.Find(userName).UserName2 = db.UserNames.Find(parent);
            db.SaveChanges();
        }
        /* Updates the list of children assigned to the
         * given parent.
         * @param userName: The parent's username.
         * @param assignedChildren: An array of childrens'
         * usernames.
         */
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
        #endregion
        #region EmailOutboxes
        /* Returns the EmailOutbox to be used for a given
         * purpose. A valid purpose example is "password reset".
         * The given purpose must be a purpose listed in the database.
         * @param purpose: The string purpose name associated with an 
         * email outbox.
         */
        public EmailOutbox getEmailOutboxForPurpose(string purpose)
        {
            return db.EmailOutboxes.Where(s => s.purpose == purpose).FirstOrDefault();
        }
        #endregion
        
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

        #region Private Methods
        /* Returns whether or not the given user is
         * assigned to a job coach.
         * @param usr: The assignee's MembershipUser object.
         */
        private bool isUserAssigned(MembershipUser usr)
        {
            string jobCoachUserName = db.UserNames.Find(usr.UserName).jobCoachUserName;
            if (jobCoachUserName == null)
                return false;
            if (!userExists(jobCoachUserName))
                return false;
            return true;
        }
        /* Returns whether or not the given user is
         * assigned to a parent.
         * @param usr: The child's MembershipUser object.
         */
        private bool isChildAssigned(MembershipUser usr)
        {
            string parentUserName = db.UserNames.Find(usr.UserName).guardianUserName;
            if (parentUserName == null)
                return false;
            if (!userExists(parentUserName))
                return false;
            return true;
        }
        #endregion

        public void dispose()
        {
            db.Dispose();
        }
    }
}