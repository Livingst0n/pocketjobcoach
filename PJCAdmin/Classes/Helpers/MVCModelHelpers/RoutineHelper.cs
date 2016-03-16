using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* --------------------------------------------------------
     * The RoutineHelper class provides common methods relating 
     * to Routines for the MVC service. It contains methods for
     * both the current user (not requiring the username) and
     * for Administrators to provide the Routine creator's 
     * username.
     * --------------------------------------------------------
     */
    public class RoutineHelper
    {
        private pjcEntities db = new pjcEntities();
        private TaskHelper taskHelper = new TaskHelper();
        private FeedbackHelper feedbackHelper = new FeedbackHelper();
        private JobHelper jobHelper = new JobHelper();

        #region Current User Methods
        #region Getters
        /* Returns a list of the names of all routines 
         * created by the currently logged in user.
         */
        public List<string> getRoutineNames()
        {
            return getRoutineNames(AccountHelper.getCurrentUsername());
        }
        /* Returns a list of all routines created by the 
         * currently logged in user.
         */
        public List<Routine> getRoutines()
        {
            return getRoutines(AccountHelper.getCurrentUsername());
        }
        /* Returns a list of all routines created by the 
         * currently logged in user in model format.
         */
        public List<RoutineModel> getRoutineModels()
        {
            return getRoutineModels(AccountHelper.getCurrentUsername());
        }
        /* Returns a list of all routines created by the 
         * currently logged in user that matches the given 
         * routine name.
         * @param routineName: The routine name to match.
         */
        public List<Routine> getRoutinesByName(string routineName)
        {
            return getRoutinesByName(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns a list of all routines created by the
         * currently logged in user that matches the given 
         * routine name in model format.
         * @param routineName: The routine name to match.
         */
        public List<RoutineModel> getRoutineModelsByName(string routineName)
        {
            return getRoutineModelsByName(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns the most recent active routine created
         * by the currently logged in user that matches the
         * given routine name.
         * @param routineName: The routine name to match.
         */
        public Routine getActiveRoutineByName(string routineName)
        {
            return getActiveRoutineByName(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns the most recent active routine created
         * by the currently logged in user that matches the 
         * given routine name in model format.
         * @param routineName: The routine name to match.
         */
        public RoutineModel getActiveRoutineModelByName(string routineName)
        {
            return getActiveRoutineModelByName(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns the most recent routine created by the
         * currently logged in user that matches the given
         * routine name (either active or disabled).
         * @param routineName: The routine name to match.
         */
        public Routine getMostRecentRoutineByName(string routineName)
        {
            return getMostRecentRoutineByName(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns the most recent routine created by the
         * currently logged in user that matches the given
         * routine name (either active or disabled) in model
         * format.
         * @param routineName: The routine name to match.
         */
        public RoutineModel getMostRecentRoutineModelByName(string routineName)
        {
            return getMostRecentRoutineModelByName(AccountHelper.getCurrentUsername(), routineName);
        }
        #endregion
        #region Boolean Checks
        /* Returns whether or not the currently logged in
         * user has created any routines matching the given 
         * name.
         * @param routineName: The routine name to match.
         */
        public bool routineExists(string routineName)
        {
            return routineExists(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns whether or not any routines created by the 
         * currently logged in user that match the given name
         * are active.
         * @param routineName: The routine name to match.
         */
        public bool routineIsActive(string routineName)
        {
            return routineIsActive(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Returns whether or not the most recent routine 
         * created by the currently logged in user that matches
         * the given name has any jobs associated with it.
         * @param routineName: The routine name to match.
         */
        public bool routineHasJobs(string routineName)
        {
            return routineHasJobs(AccountHelper.getCurrentUsername(), routineName);
        }
        #endregion
        #region Creating, Updating, and Deleting
        /* Creates a new routine for the currently logged in
         * user from the given model.
         * @param model: The model of the routine to create
         * from.
         */
        public void createRoutine(RoutineModel model)
        {
            createRoutine(AccountHelper.getCurrentUsername(), model);
        }
        /* Updates the most recent routine created by the 
         * currently logged in user that matches the given
         * name. Creates a new routine if no routine matches
         * the given name or if the matched routine already
         * has jobs associated with it.
         * @param routineName: The routine name to match.
         * @param model: The model to update or create
         * a routine from.
         */
        public void updateRoutine(string routineName, RoutineModel model)
        {
            updateRoutine(AccountHelper.getCurrentUsername(), routineName, model);
        }
        /* Deletes the most recent routine created by the 
         * currently logged in user that matches the given
         * name. Deletes all routines created by the current
         * user that matches the given name if deleteAll is 
         * true.
         * @param routineName: The routine name to match.
         * @param deleteAll: Determines whether all matching
         * routines should be deleted or only the most recent.
         */
        public void deleteRoutine(string routineName, bool deleteAll = false)
        {
            deleteRoutine(AccountHelper.getCurrentUsername(), routineName, deleteAll);
        }
        /* Deletes all routines created by the currently
         * logged in user that matches the given name.
         * @param routineName: The routine name to match.
         */
        public void deleteAllVersionsOfRoutine(string routineName)
        {
            deleteAllVersionsOfRoutine(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Deletes the most recent routine created by the 
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void deleteMostRecentRoutine(string routineName)
        {
            deleteMostRecentRoutine(AccountHelper.getCurrentUsername(), routineName);
        }
        #endregion
        #region Enabling and Disabling
        /* Enables the most recent routine created by the
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void enableRoutine(string routineName)
        {
            enableRoutine(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Efficiently disables all routines created by the
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void disableRoutine(string routineName)
        {
            disableRoutine(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Disables the most recent routine created by the 
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void disableMostRecentRoutine(string routineName)
        {
            disableMostRecentRoutine(AccountHelper.getCurrentUsername(), routineName);
        }
        /* Disables all routines created by the currently
         * logged in user that match the given name.
         * @param routineName: The routine name to match.
         */
        public void disableAllRoutinesByName(string routineName)
        {
            disableAllRoutinesByName(AccountHelper.getCurrentUsername(), routineName);
        }
        #endregion
        #endregion
        #region Specified Creator Methods
        #region Getters
        /* Returns a list of the names of all routines
         * created by the given user.
         * @param creatorUsername: The username of the
         * user who created the routine.
         */
        public List<string> getRoutineNames(string creatorUsername)
        {
            List<string> lstRoutines = new List<string>();
            foreach (Routine r in db.UserNames.Find(creatorUsername).Routines)
                lstRoutines.Add(r.routineTitle);

            return lstRoutines;
        }
        /* Returns a list of all routines created by the
         * given user
         * @param creatorUsername: The username of the
         * user who created the routine.
         */
        public List<Routine> getRoutines(string creatorUsername)
        {
            return db.UserNames.Find(creatorUsername).Routines.ToList();
        }
        /* Returns a list of all routines created by the
         * given user in model format.
         * @param creatorUsername: The username of the
         * user who created the routine.
         */
        public List<RoutineModel> getRoutineModels(string creatorUsername)
        {
            return ModelCopier.copyRoutinesToModels(getRoutines(creatorUsername));
        }
        /* Returns a list of all routines created by the
         * given user that matches the given routine name.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public List<Routine> getRoutinesByName(string creatorUsername, string routineName)
        {
            return getRoutines(creatorUsername).AsQueryable().Where(r => r.routineTitle.Equals(routineName)).ToList();
        }
        /* Returns a list of all routines created by the 
         * given user that matches the given routine name
         * in model format.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public List<RoutineModel> getRoutineModelsByName(string creatorUsername, string routineName)
        {
            List<RoutineModel> lst = new List<RoutineModel>();

            foreach (Routine r in getRoutinesByName(creatorUsername, routineName))
                lst.Add(ModelCopier.copyRoutineToModel(r));

            return lst;
        }
        /* Returns the most recent active routine created
         * by the given user that matches the given routine
         * name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public Routine getActiveRoutineByName(string creatorUsername, string routineName)
        {
            IQueryable<Routine> activeRoutines = getRoutinesByName(creatorUsername, routineName).AsQueryable().Where(r => r.isDisabled == false).OrderBy(r => r.updatedDate);
            //Last should be current (newest) routine

            if (activeRoutines.Count() == 0)
                return null; //No active routines found by this name

            if (activeRoutines.Count() > 1)
                disableOldRoutines(activeRoutines.ToList());

            return activeRoutines.Last();
        }
        /* Returns the most recent active routine created
         * by the given user that matches the given routine
         * name in model format.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public RoutineModel getActiveRoutineModelByName(string creatorUsername, string routineName)
        {
            return ModelCopier.copyRoutineToModel(getActiveRoutineByName(creatorUsername, routineName));
        }
        /* Returns the most recent routine created by the
         * given user that matches the given routine name
         * (either active or disabled).
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public Routine getMostRecentRoutineByName(string creatorUsername, string routineName)
        {
            return getRoutinesByName(creatorUsername, routineName).OrderBy(r => r.updatedDate).Last();
        }
        /* Returns the most recent routine created by the
         * given user that matches the given routine name
         * (either active or disabled) in model format.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public RoutineModel getMostRecentRoutineModelByName(string creatorUsername, string routineName)
        {
            return ModelCopier.copyRoutineToModel(getMostRecentRoutineByName(creatorUsername, routineName));
        }
        #endregion
        #region Boolean Checks
        /* Returns whether or not the given user has created
         * any routines matching the given name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public bool routineExists(string creatorUsername, string routineName)
        {
            return getRoutinesByName(creatorUsername, routineName).Count() > 0;
        }
        /* Returns whether or not any routines created by the
         * given user that match the given name are active.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public bool routineIsActive(string creatorUsername, string routineName)
        {
            return getActiveRoutineByName(creatorUsername, routineName) != null;
        }
        /* Returns whether or not the most recent routine
         * created by the given user that matches the given
         * name has any jobs associated with it.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public bool routineHasJobs(string creatorUsername, string routineName)
        {
            if (!routineExists(creatorUsername, routineName))
                return false;

            Routine routine = getMostRecentRoutineByName(creatorUsername, routineName);

            if (jobHelper.jobsExistForRoutine(routine.routineID))
                return true;
            return false;
        }
        #endregion
        #region Creating, Updating, and Deleting
        /* Creates a new routine for the given user from 
         * the given model.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param model: The model of the routine to create
         * from.
         */
        public void createRoutine(string creatorUsername, RoutineModel model)
        {
            Routine r = new Routine()
            {
                assigneeUserName = model.assigneeUserName,
                creatorUserName = creatorUsername,
                expectedDuration = model.expectedDuration,
                isDisabled = model.isDisabled,
                isNotifiable = model.isNotifiable,
                isTimed = model.isTimed,
                routineTitle = model.routineTitle,
                updatedDate = DateTime.Now
            };

            db.Routines.Add(r);
            db.SaveChanges();

            foreach (TaskModel t in model.Tasks)
                taskHelper.createTask(r.routineID, t);

            foreach (FeedbackModel f in model.Feedbacks)
                feedbackHelper.createRoutineFeedback(r.routineID, f);
        }
        /* Updates the most recent routine created by the
         * given user that matches the given name. Creates
         * a new routine if no routine matches the given
         * name or if the matched routine already has jobs
         * associated with it.
         * @param creatorUsername: The username of the
         * user who is to be the creator of the routine.
         * @param routineName: The routine name to match.
         * @param model: The model to update or create
         * a routine from.
         */
        public void updateRoutine(string creatorUsername, string routineName, RoutineModel model)
        {
            if (!routineExists(creatorUsername, routineName))
                createRoutine(creatorUsername, model);

            if (routineHasJobs(creatorUsername, routineName))
            { // Cannot edit routines that have existing jobs
                //Except if times are same and steps are still in same order
                //TODO add check for similarity
                disableAllRoutinesByName(creatorUsername, routineName);
                createRoutine(creatorUsername, model);
            }
            else
            {
                modifyExistingRoutine(creatorUsername, routineName, model);
            }
        }
        /* Deletes the most recent routine created by the
         * given user that matches the given name. Deletes
         * all routines created by the given user that 
         * matches the given name if deleteAll is true.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         * @param deleteAll: Determines whether all matching
         * routines should be deleted or only the most recent.
         */
        public void deleteRoutine(string creatorUsername, string routineName, bool deleteAll)
        {
            if (deleteAll)
                deleteAllVersionsOfRoutine(creatorUsername, routineName);
            else
                deleteMostRecentRoutine(creatorUsername, routineName);
        }
        /* Deletes all routines created by the given user
         * that matches the given name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void deleteAllVersionsOfRoutine(string creatorUsername, string routineName)
        {
            foreach (Routine r in getRoutinesByName(creatorUsername, routineName))
                deleteRoutine(r);
        }
        /* Deletes the most recent routine created by the
         * given user that matches the given name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void deleteMostRecentRoutine(string creatorUsername, string routineName)
        {
            deleteRoutine(getMostRecentRoutineByName(creatorUsername, routineName));
        }
        #endregion
        #region Enabling and Disabling
        /* Enables the most recent routine created by the
         * given user that matches the given name.
         * @param creatorUsername: The username for the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void enableRoutine(string creatorUsername, string routineName)
        {
            Routine r = getMostRecentRoutineByName(creatorUsername, routineName);

            if (r.isDisabled)
            {
                r.isDisabled = false;
                db.Entry<Routine>(r).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
        }
        /* Efficiently disables all routines created by the
         * given user that matches the given name.
         * @param creatorUsername: The username for the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void disableRoutine(string creatorUsername, string routineName)
        {
            disableMostRecentRoutine(creatorUsername, routineName);

            if (routineIsActive(creatorUsername, routineName))
                disableAllRoutinesByName(creatorUsername, routineName);
        }
        /* Disables the most recent routine created by the
         * given user that matches the given name.
         * @param creatorUsername: The username for the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void disableMostRecentRoutine(string creatorUsername, string routineName)
        {
            Routine r = getMostRecentRoutineByName(creatorUsername, routineName);
            r.isDisabled = true;
            db.Entry<Routine>(r).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        /* Disables all routines created by the given user
         * that match the given name.
         * @param creatorUsername: The username for the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void disableAllRoutinesByName(string creatorUsername, string routineName)
        {
            List<Routine> routines = getRoutinesByName(creatorUsername, routineName);

            foreach (Routine r in routines)
            {
                if (!r.isDisabled)
                {
                    r.isDisabled = true;
                    db.Entry<Routine>(r).State = System.Data.EntityState.Modified;
                }
            }

            db.SaveChanges();
        }
        #endregion
        #endregion
        #region Private Methods
        /* Deletes the given routine from the database.
         * @param routine: The routine record to delete.
         */
        private void deleteRoutine(Routine routine)
        {
            /*jobHelper.deleteAllJobsByRoutine(routine.routineID);
            feedbackHelper.deleteFeedbacksByRoutine(routine.routineID);
            taskHelper.deleteTasksByRoutine(routine.routineID);

            db.Entry<Routine>(routine).State = System.Data.EntityState.Deleted;
            db.SaveChanges();*/

            db.Routines.Remove(routine);
            db.SaveChanges();

            //TODO check Feedback, Task, Job, Note etc for deletion. (Orphans should be, non-orphans should not be)
        }
        /* Updates the most recent routine created by the
         * given user that matches the given name, using
         * the given model.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         * @param model: The model to use for updating
         * the existing routine.
         */
        private void modifyExistingRoutine(string creatorUsername, string routineName, RoutineModel model)
        {
            Routine r = getMostRecentRoutineByName(creatorUsername, routineName);

            r.isTimed = model.isTimed;
            r.expectedDuration = model.expectedDuration;
            r.isNotifiable = model.isNotifiable;
            r.isDisabled = model.isDisabled;
            r.updatedDate = DateTime.Now;

            taskHelper.modifyExistingTasks(r.Tasks.ToList(), model.Tasks.ToList(),true);
            feedbackHelper.updateRoutineFeedbacks(r.routineID, model.Feedbacks.ToList());

            db.Entry<Routine>(r).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        /* Disables all old routines in the given list of
         * routines. The most recent routine in the list is
         * left as is.
         * @param routines: A list of routines for which only
         * the most recent should be allowed to be active. 
         * All entries should have the same routine name.
         */
        private void disableOldRoutines(List<Routine> routines)
        {
            routines.Remove(routines.OrderBy(r => r.updatedDate).Last());

            foreach (Routine routine in routines)
            {
                if (routine.isDisabled == false)
                {
                    routine.isDisabled = true;
                    db.Entry<Routine>(routine).State = System.Data.EntityState.Modified;
                }
            }

            db.SaveChanges();
        }
        #endregion

        public void dispose()
        {
            db.Dispose();
            feedbackHelper.dispose();
            jobHelper.dispose();
            taskHelper.dispose();
        }
    }
}