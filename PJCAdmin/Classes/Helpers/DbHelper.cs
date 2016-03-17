using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers
{
    public class DbHelper
    {
        private static pjcEntities db = new pjcEntities();

        #region Accounts
        public void createUserName(object providerUserKey)
        {
            User user = db.Users.Find(providerUserKey);
            db.UserNames.Add(new UserName() { userID = user.UserId, userName1 = user.UserName });
            db.SaveChanges();
        }
        public UserName findUserName(string userName)
        {
            return db.UserNames.Find(userName);
        }
        public void deleteUserName(string userName)
        {
            db.UserNames.Remove(db.UserNames.Find(userName));
            db.SaveChanges();
        }
        #endregion
        #region Job Coach and Parent Assignment
        public void updateJobCoachAssignment(string assignee, string jobCoach)
        {
            UserName user = findUserName(assignee);
            if (jobCoach == null)
                user.UserName3 = null;
            else
                //UserName3 is the job coach
                user.UserName3 = findUserName(jobCoach);

            db.Entry<UserName>(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        public void updateAssignedUsers(string jobCoach, string[] assignees)
        {
            UserName user = db.UserNames.Find(jobCoach);
            //UserName12 is all usernames assigned to the selected username
            user.UserName12.Clear();
            if (assignees != null)
            {
                foreach (string username in assignees)
                {
                    UserName selectedUserName = db.UserNames.Find(username);
                    if (!(selectedUserName == null))
                        user.UserName12.Add(selectedUserName);
                }
            }
            db.Entry<UserName>(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        public void updateParentAssignment(string assignee, string parent)
        {
            UserName user = findUserName(assignee);

            if (parent == null)
                user.UserName2 = null;
            else
                //UserName2 is the guardian
                user.UserName2 = db.UserNames.Find(parent);

            db.Entry<UserName>(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        public void updateAssignedChildren(string parent, string[] assignees)
        {
            //UserName11 is all children of the selected username
            UserName user = findUserName(parent);
            user.UserName11.Clear();
            if (assignees != null)
            {
                foreach (string id in assignees)
                {
                    string selectedUserName = findUserName(id).userName1;
                    user.UserName11.Add(db.UserNames.Find(selectedUserName));
                }
            }
            db.Entry<UserName>(user).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        #endregion
        #region EmailOutboxes
        public IQueryable<EmailOutbox> getAllEmailOutboxes()
        {
            return db.EmailOutboxes.AsQueryable();
        }
        #endregion
        #region Enums
        #region TaskCategories
        public void createTaskCategory(string category)
        {
            TaskCategory tc = new TaskCategory()
            {
                categoryName = category
            };

            db.TaskCategories.Add(tc);
            db.SaveChanges();
        }
        public IQueryable<TaskCategory> getAllTaskCategories()
        {
            return db.TaskCategories.AsQueryable();
        }
        public void updateTaskCategory(TaskCategory oldTC, string newCategory)
        {
            oldTC.categoryName = newCategory;

            db.Entry<TaskCategory>(oldTC).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        public void deleteTaskCategory(TaskCategory taskCategory)
        {
            db.TaskCategories.Remove(taskCategory);
            db.SaveChanges();
        }
        #endregion
        #region MediaTypes
        public IQueryable<MediaType> getAllMediaTypes()
        {
            return db.MediaTypes.AsQueryable();
        }
        #endregion
        #region FeedbackTypes
        public IQueryable<FeedbackType> getAllFeedbackTypes()
        {
            return db.FeedbackTypes.AsQueryable();
        }
        #endregion
        #endregion
        #region Feedbacks
        #region General Feedback
        public IQueryable<Feedback> getAllFeedbacks()
        {
            return db.Feedbacks.AsQueryable();
        }
        public Feedback createFeedback(Feedback feedback)
        {
            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return feedback;
        }
        public Feedback updateFeedback(Feedback feedback)
        {
            db.Entry<Feedback>(feedback).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return feedback;
        }
        #endregion
        #region Routine Feedback
        public IQueryable<Routine> getAllRoutines()
        {
            return db.Routines.AsQueryable();
        }
        public Routine findRoutine(int routineID)
        {
            return db.Routines.Find(routineID);
        }
        #endregion
        #region Task Feedback
        public IQueryable<Task> getAllTasks()
        {
            return db.Tasks.AsQueryable();
        }
        public Task findTask(int taskID)
        {
            return db.Tasks.Find(taskID);
        }
        #endregion
        #endregion
        #region Jobs
        public IQueryable<Job> getAllJobs()
        {
            return db.Jobs.AsQueryable();
        }
        #endregion
        #region Routines
        public Routine createRoutine(Routine r)
        {
            db.Routines.Add(r);
            db.SaveChanges();

            return r;
        }
        public Routine updateRoutine(Routine r)
        {
            db.Entry<Routine>(r).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return r;
        }
        public void deleteRoutine(Routine r)
        {
            db.Routines.Remove(r);
            db.SaveChanges();
        }
        #endregion
        #region Tasks
        public Task createTask(Task t)
        {
            db.Tasks.Add(t);
            db.SaveChanges();

            return t;
        }
        public Task updateTask(Task t)
        {
            db.Entry<Task>(t).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return t;
        }
        public void deleteTask(Task t)
        {
            db.Tasks.Remove(t);
            db.SaveChanges();
        }
        #endregion
        #region AuthTokens
        public IQueryable<AuthToken> getAllAuthTokens()
        {
            return db.AuthTokens.AsQueryable();
        }
        public AuthToken findAuthToken(int id)
        {
            return db.AuthTokens.Find(id);
        }
        public AuthToken reloadToken(AuthToken authToken)
        {
            db.Entry(authToken).Reload();
            return authToken;
        }
        public AuthToken updateToken(AuthToken authToken)
        {
            db.Entry(authToken).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return authToken;
        }
        #endregion

        public void dispose()
        {
           //db.Dispose();
        }
    }
}