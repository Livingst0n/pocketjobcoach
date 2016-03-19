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
    //TODO add get...AssignedToUser...(...) functions
    public class RoutineHelper
    {
        private TaskHelper taskHelper = new TaskHelper();
        private FeedbackHelper feedbackHelper = new FeedbackHelper();
        private JobHelper jobHelper = new JobHelper();
        private DbHelper helper = new DbHelper();

        #region Current User Methods
        #region Getters
        /* Returns a list of the names of all routines 
         * created by the currently logged in user.
         */
        /* getRoutineNames not needed?
        public List<string> getRoutineNames()
        {
            return getRoutineNames(AccountHelper.getCurrentUsername());
        }
        */
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
        /*TODO*/
        public List<Routine> getRoutinesAssignedTo(string assigneeUsername)
        {
            return getRoutinesAssignedTo(AccountHelper.getCurrentUsername(), assigneeUsername);
        }
        /*TODO*/
        public List<RoutineModel> getRoutineModelsAssignedTo(string assigneeUsername)
        {
            return getRoutineModelsAssignedTo(AccountHelper.getCurrentUsername(), assigneeUsername);
        }
        /*TODO*/
        public List<Routine> getMostRecentRoutines()
        {
            return getMostRecentRoutines(AccountHelper.getCurrentUsername());
        }
        /*TODO*/
        public List<RoutineModel> getMostRecentRoutineModels()
        {
            return getMostRecentRoutineModels(AccountHelper.getCurrentUsername());
        }
        /*TODO*/
        public List<Routine> getMostRecentRoutinesAssignedTo(string assigneeUsername)
        {
            return getMostRecentRoutinesAssignedTo(AccountHelper.getCurrentUsername(), assigneeUsername);
        }
        /*TODO*/
        public List<RoutineModel> getMostRecentRoutineModelsAssignedTo(string assigneeUsername)
        {
            return getMostRecentRoutineModelsAssignedTo(AccountHelper.getCurrentUsername(), assigneeUsername);
        }
        /* Returns a list of all routines created by the 
         * currently logged in user that matches the given 
         * routine name.
         * @param routineName: The routine name to match.
         */
        public List<Routine> getRoutinesAssignedToByName(string routineName, string assigneeName)
        {
            return getRoutinesAssignedToByName(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Returns a list of all routines created by the
         * currently logged in user that matches the given 
         * routine name in model format.
         * @param routineName: The routine name to match.
         */
        public List<RoutineModel> getRoutineModelsAssignedToByName(string routineName, string assigneeName)
        {
            return getRoutineModelsAssignedToByName(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Returns the most recent active routine created
         * by the currently logged in user that matches the
         * given routine name.
         * @param routineName: The routine name to match.
         */
        /*
        public Routine getActiveRoutineByName(string routineName)
        {
            return getActiveRoutineByName(AccountHelper.getCurrentUsername(), routineName);
        }
        */
        /* Returns the most recent active routine created
         * by the currently logged in user that matches the 
         * given routine name in model format.
         * @param routineName: The routine name to match.
         */
        /*
        public RoutineModel getActiveRoutineModelByName(string routineName)
        {
            return getActiveRoutineModelByName(AccountHelper.getCurrentUsername(), routineName);
        }
        */
        /* Returns the most recent routine created by the
         * currently logged in user that matches the given
         * routine name (either active or disabled).
         * @param routineName: The routine name to match.
         */
        public Routine getMostRecentRoutineAssignedToByName(string routineName, string assigneeName)
        {
            return getMostRecentRoutineAssignedToByName(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Returns the most recent routine created by the
         * currently logged in user that matches the given
         * routine name (either active or disabled) in model
         * format.
         * @param routineName: The routine name to match.
         */
        public RoutineModel getMostRecentRoutineModelAssignedToByName(string routineName, string assigneeName)
        {
            return getMostRecentRoutineModelAssignedToByName(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        #endregion
        #region Boolean Checks
        /* Returns whether or not the currently logged in
         * user has created any routines matching the given 
         * name.
         * @param routineName: The routine name to match.
         */
        public bool routineExists(string routineName, string assigneeName)
        {
            return routineExists(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Returns whether or not any routines created by the 
         * currently logged in user that match the given name
         * are active.
         * @param routineName: The routine name to match.
         */
        public bool routineIsActive(string routineName, string assigneeName)
        {
            return routineIsActive(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Returns whether or not the most recent routine 
         * created by the currently logged in user that matches
         * the given name has any jobs associated with it.
         * @param routineName: The routine name to match.
         */
        public bool routineHasJobs(string routineName, string assigneeName)
        {
            return routineHasJobs(AccountHelper.getCurrentUsername(), routineName, assigneeName);
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
        public void deleteRoutine(string routineName, string assigneeName, bool deleteAll = false)
        {
            deleteRoutine(AccountHelper.getCurrentUsername(), routineName, assigneeName, deleteAll);
        }
        /* Deletes all routines created by the currently
         * logged in user that matches the given name.
         * @param routineName: The routine name to match.
         */
        public void deleteAllVersionsOfRoutine(string routineName, string assigneeName)
        {
            deleteAllVersionsOfRoutine(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Deletes the most recent routine created by the 
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void deleteMostRecentRoutine(string routineName, string assigneeName)
        {
            deleteMostRecentRoutine(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        #endregion
        #region Enabling and Disabling
        /* Enables the most recent routine created by the
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void enableRoutine(string routineName, string assigneeName)
        {
            enableRoutine(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Efficiently disables all routines created by the
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void disableRoutine(string routineName, string assigneeName)
        {
            disableRoutine(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Disables the most recent routine created by the 
         * currently logged in user that matches the given
         * name.
         * @param routineName: The routine name to match.
         */
        public void disableMostRecentRoutine(string routineName, string assigneeName)
        {
            disableMostRecentRoutine(AccountHelper.getCurrentUsername(), routineName, assigneeName);
        }
        /* Disables all routines created by the currently
         * logged in user that match the given name.
         * @param routineName: The routine name to match.
         */
        public void disableAllRoutinesByName(string routineName, string assigneeName)
        {
            disableAllRoutinesByName(AccountHelper.getCurrentUsername(), routineName, assigneeName);
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
        /* getRoutineNames not needed?
        public List<string> getRoutineNames(string creatorUsername)
        {
            List<string> lstRoutines = new List<string>();
            foreach (Routine r in helper.findUserName(creatorUsername).Routines)
                lstRoutines.Add(r.routineTitle);

            return lstRoutines;
        }
        */
        /* Returns a list of all routines created by the
         * given user
         * @param creatorUsername: The username of the
         * user who created the routine.
         */
        public List<Routine> getRoutines(string creatorUsername)
        {
            return helper.findUserName(creatorUsername).Routines.ToList();
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
        /*TODO*/
        public List<Routine> getRoutinesAssignedTo(string creatorUsername, string assigneeUsername)
        {
            return getRoutines(creatorUsername).Where(r => r.assigneeUserName != null && r.assigneeUserName.Equals(assigneeUsername)).ToList();
        }
        /*TODO*/
        public List<RoutineModel> getRoutineModelsAssignedTo(string creatorUsername, string assigneeUsername)
        {
            return ModelCopier.copyRoutinesToModels(getRoutinesAssignedTo(creatorUsername, assigneeUsername));
        }
        /*TODO*/
        public List<Routine> getMostRecentRoutines(string creatorUsername)
        {
            var query = getRoutines(creatorUsername).GroupBy(r => new {r.assigneeUserName, r.routineTitle});
            List<Routine> lst = new List<Routine>();

            foreach (var routineGroup in query)
                if (routineGroup.Count() > 0)
                    lst.Add(routineGroup.OrderBy(r => r.updatedDate).First());

            return lst;
        }
        /*TODO*/
        public List<RoutineModel> getMostRecentRoutineModels(string creatorUsername)
        {
            return ModelCopier.copyRoutinesToModels(getMostRecentRoutines(creatorUsername));
            /*List<RoutineModel> lst = new List<RoutineModel>();

            foreach (Routine r in getMostRecentRoutines(creatorUsername))
                lst.Add(ModelCopier.copyRoutineToModel(r));

            return lst;*/
        }
        /*TODO*/
        public List<Routine> getMostRecentRoutinesAssignedTo(string creatorUsername, string assigneeUsername)
        {
            var query = getRoutinesAssignedTo(creatorUsername, assigneeUsername).GroupBy(r => r.routineTitle);
            List<Routine> lst = new List<Routine>();

            foreach (var routineGroup in query)
            {
                if (routineGroup.Count() > 0)
                    lst.Add(routineGroup.OrderBy(r => r.updatedDate).First());
            }

            return lst;
        }
        /*TODO*/
        public List<RoutineModel> getMostRecentRoutineModelsAssignedTo(string creatorUsername, string assigneeUsername)
        {
            return ModelCopier.copyRoutinesToModels(getMostRecentRoutinesAssignedTo(creatorUsername, assigneeUsername));
        }
        /* Returns a list of all routines created by the
         * given user that matches the given routine name.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public List<Routine> getRoutinesAssignedToByName(string creatorUsername, string routineName, string assigneeName)
        {
            return getRoutinesAssignedTo(creatorUsername, assigneeName).Where(r => r.routineTitle.Equals(routineName)).ToList();
        }
        /* Returns a list of all routines created by the 
         * given user that matches the given routine name
         * in model format.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public List<RoutineModel> getRoutineModelsAssignedToByName(string creatorUsername, string routineName, string assigneeName)
        {
            List<RoutineModel> lst = new List<RoutineModel>();

            foreach (Routine r in getRoutinesAssignedToByName(creatorUsername, routineName, assigneeName))
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
        /*
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
        */
        /* Returns the most recent active routine created
         * by the given user that matches the given routine
         * name in model format.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        /*
        public RoutineModel getActiveRoutineModelByName(string creatorUsername, string routineName)
        {
            return ModelCopier.copyRoutineToModel(getActiveRoutineByName(creatorUsername, routineName));
        }
        */
        /* Returns the most recent routine created by the
         * given user that matches the given routine name
         * (either active or disabled).
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public Routine getMostRecentRoutineAssignedToByName(string creatorUsername, string routineName, string assigneeName)
        {
            return getRoutinesAssignedToByName(creatorUsername, routineName, assigneeName).OrderBy(r => r.updatedDate).Last();
        }
        /* Returns the most recent routine created by the
         * given user that matches the given routine name
         * (either active or disabled) in model format.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public RoutineModel getMostRecentRoutineModelAssignedToByName(string creatorUsername, string routineName, string assigneeName)
        {
            return ModelCopier.copyRoutineToModel(getMostRecentRoutineAssignedToByName(creatorUsername, routineName, assigneeName));
        }
        #endregion
        #region Boolean Checks
        /* Returns whether or not the given user has created
         * any routines matching the given name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public bool routineExists(string creatorUsername, string routineName, string assigneeName)
        {
            return getMostRecentRoutineModelsAssignedTo(creatorUsername, assigneeName).Where(r => r.routineTitle.Equals(routineName)).Count() > 0;
        }
        /* Returns whether or not any routines created by the
         * given user that match the given name are active.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public bool routineIsActive(string creatorUsername, string routineName, string assigneeName)
        {
            return !(getMostRecentRoutineAssignedToByName(creatorUsername, routineName, assigneeName).isDisabled);
        }
        /* Returns whether or not the most recent routine
         * created by the given user that matches the given
         * name has any jobs associated with it.
         * @param creatorUsername: The username of the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public bool routineHasJobs(string creatorUsername, string routineName, string assigneeName)
        {
            if (!routineExists(creatorUsername, routineName, assigneeName))
                return false;

            Routine routine = getMostRecentRoutineAssignedToByName(creatorUsername, routineName, assigneeName);

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

            r = helper.createRoutine(r);

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
            if (!routineExists(creatorUsername, routineName, model.assigneeUserName))
                createRoutine(creatorUsername, model);

            if (routineHasJobs(creatorUsername, routineName, model.assigneeUserName))
            { // Cannot edit routines that have existing jobs
                //Except if times are same and steps are still in same order
                //TODO add check for similarity
                disableAllRoutinesByName(creatorUsername, routineName, model.assigneeUserName);
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
        public void deleteRoutine(string creatorUsername, string routineName, string assigneeName, bool deleteAll)
        {
            if (deleteAll)
                deleteAllVersionsOfRoutine(creatorUsername, routineName, assigneeName);
            else
                deleteMostRecentRoutine(creatorUsername, routineName, assigneeName);
        }
        /* Deletes all routines created by the given user
         * that matches the given name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void deleteAllVersionsOfRoutine(string creatorUsername, string routineName, string assigneeName)
        {
            foreach (Routine r in getRoutinesAssignedToByName(creatorUsername, routineName, assigneeName))
                deleteRoutine(r);
        }
        /* Deletes the most recent routine created by the
         * given user that matches the given name.
         * @param creatorUsername: The username of the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void deleteMostRecentRoutine(string creatorUsername, string routineName, string assigneeName)
        {
            deleteRoutine(getMostRecentRoutineAssignedToByName(creatorUsername, routineName, assigneeName));
        }
        #endregion
        #region Enabling and Disabling
        /* Enables the most recent routine created by the
         * given user that matches the given name.
         * @param creatorUsername: The username for the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void enableRoutine(string creatorUsername, string routineName, string assigneeName)
        {
            Routine r = getMostRecentRoutineAssignedToByName(creatorUsername, routineName, assigneeName);

            if (r.isDisabled)
            {
                r.isDisabled = false;
                helper.updateRoutine(r);
            }
        }
        /* Efficiently disables all routines created by the
         * given user that matches the given name.
         * @param creatorUsername: The username for the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void disableRoutine(string creatorUsername, string routineName, string assigneeName)
        {
            disableMostRecentRoutine(creatorUsername, routineName, assigneeName);

            if (routineIsActive(creatorUsername, routineName, assigneeName)) //TODO check if old versions of routines can be enabled
                disableAllRoutinesByName(creatorUsername, routineName, assigneeName);
        }
        /* Disables the most recent routine created by the
         * given user that matches the given name.
         * @param creatorUsername: The username for the
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void disableMostRecentRoutine(string creatorUsername, string routineName, string assigneeName)
        {
            Routine r = getMostRecentRoutineAssignedToByName(creatorUsername, routineName, assigneeName);
            r.isDisabled = true;
            helper.updateRoutine(r);
        }
        /* Disables all routines created by the given user
         * that match the given name.
         * @param creatorUsername: The username for the 
         * user who created the routine.
         * @param routineName: The routine name to match.
         */
        public void disableAllRoutinesByName(string creatorUsername, string routineName, string assigneeName)
        {
            List<Routine> routines = getRoutinesAssignedToByName(creatorUsername, routineName, assigneeName);

            foreach (Routine r in routines)
            {
                if (!r.isDisabled)
                {
                    r.isDisabled = true;
                    helper.updateRoutine(r);
                }
            }
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

            helper.deleteRoutine(routine);

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
            Routine r = getMostRecentRoutineAssignedToByName(creatorUsername, routineName, model.assigneeUserName);

            r.isTimed = model.isTimed;
            r.expectedDuration = model.expectedDuration;
            r.isNotifiable = model.isNotifiable;
            r.isDisabled = model.isDisabled;
            r.updatedDate = DateTime.Now;

            taskHelper.modifyExistingTasks(r.Tasks.ToList(), model.Tasks.ToList(),true);
            feedbackHelper.updateRoutineFeedbacks(r.routineID, model.Feedbacks.ToList());

            helper.updateRoutine(r);
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
                    helper.updateRoutine(routine);
                }
            }
        }
        #endregion

        public void dispose()
        {
            helper.dispose();
            feedbackHelper.dispose();
            jobHelper.dispose();
            taskHelper.dispose();
        }
    }
}