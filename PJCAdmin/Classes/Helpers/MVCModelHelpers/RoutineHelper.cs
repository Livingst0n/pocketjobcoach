using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    public class RoutineHelper
    {
        private pjcEntities db = new pjcEntities();
        private TaskHelper taskHelper = new TaskHelper();
        private FeedbackHelper feedbackHelper = new FeedbackHelper();
        private JobHelper jobHelper = new JobHelper();

        public List<string> getRoutineNames()
        {
            return getRoutineNames(AccountHelper.getCurrentUsername());
        }

        public List<string> getRoutineNames(string creatorUsername)
        {
            List<string> lstRoutines = new List<string>();
            foreach (Routine r in db.UserNames.Find(creatorUsername).Routines)
                lstRoutines.Add(r.routineTitle);

            return lstRoutines;
        }

        public List<Routine> getRoutines()
        {
            return getRoutines(AccountHelper.getCurrentUsername());
        }

        public List<Routine> getRoutines(string creatorUsername)
        {
            return db.UserNames.Find(creatorUsername).Routines.ToList();
        }

        public List<RoutineModel> getRoutineModels()
        {
            return getRoutineModels(AccountHelper.getCurrentUsername());
        }

        public List<RoutineModel> getRoutineModels(string creatorUsername)
        {
            List<RoutineModel> lst = new List<RoutineModel>();

            foreach (Routine r in getRoutines(creatorUsername))
                lst.Add(ModelCopier.copyRoutineToModel(r));

            return lst;
        }

        public List<Routine> getRoutinesByName(string routineName)
        {
            return getRoutinesByName(AccountHelper.getCurrentUsername(), routineName);
        }

        public List<Routine> getRoutinesByName(string creatorUsername, string routineName)
        {
            return getRoutines(creatorUsername).AsQueryable().Where(r => r.routineTitle.Equals(routineName)).ToList();
        }

        public List<RoutineModel> getRoutineModelsByName(string routineName)
        {
            return getRoutineModelsByName(AccountHelper.getCurrentUsername(), routineName);
        }

        public List<RoutineModel> getRoutineModelsByName(string creatorUsername, string routineName)
        {
            List<RoutineModel> lst = new List<RoutineModel>();

            foreach (Routine r in getRoutinesByName(creatorUsername, routineName))
                lst.Add(ModelCopier.copyRoutineToModel(r));

            return lst;
        }

        public Routine getActiveRoutineByName(string routineName)
        {
            return getActiveRoutineByName(AccountHelper.getCurrentUsername(), routineName);
        }

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

        public RoutineModel getActiveRoutineModelByName(string routineName)
        {
            return getActiveRoutineModelByName(AccountHelper.getCurrentUsername(), routineName);
        }

        public RoutineModel getActiveRoutineModelByName(string creatorUsername, string routineName)
        {
            return ModelCopier.copyRoutineToModel(getActiveRoutineByName(creatorUsername, routineName));
        }

        public Routine getMostRecentRoutineByName(string routineName)
        {
            return getMostRecentRoutineByName(AccountHelper.getCurrentUsername(), routineName);
        }

        public Routine getMostRecentRoutineByName(string creatorUsername, string routineName)
        {
            return getRoutinesByName(creatorUsername, routineName).OrderBy(r => r.updatedDate).Last();
        }

        public RoutineModel getMostRecentRoutineModelByName(string routineName)
        {
            return getMostRecentRoutineModelByName(AccountHelper.getCurrentUsername(), routineName);
        }

        public RoutineModel getMostRecentRoutineModelByName(string creatorUsername, string routineName)
        {
            return ModelCopier.copyRoutineToModel(getMostRecentRoutineByName(creatorUsername, routineName));
        }

        public bool routineExists(string routineName)
        {
            return routineExists(AccountHelper.getCurrentUsername(), routineName);
        }

        public bool routineExists(string creatorUsername, string routineName)
        {
            return getRoutinesByName(creatorUsername, routineName).Count() > 0;
        }

        public bool routineIsActive(string routineName)
        {
            return routineIsActive(AccountHelper.getCurrentUsername(), routineName);
        }

        public bool routineIsActive(string creatorUsername, string routineName)
        {
            return getActiveRoutineByName(creatorUsername, routineName) != null;
        }

        public bool routineHasJobs(string routineName)
        {
            return routineHasJobs(AccountHelper.getCurrentUsername(), routineName);
        }

        public bool routineHasJobs(string creatorUsername, string routineName)
        {
            if (!routineExists(creatorUsername, routineName))
                return false;

            Routine routine = getMostRecentRoutineByName(creatorUsername, routineName);

            if (jobHelper.jobsExistForRoutine(routine.routineID))
                return true;
            return false;
        }

        public void updateRoutine(string routineName, RoutineModel model)
        {
            updateRoutine(AccountHelper.getCurrentUsername(), routineName, model);
        }

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

        public void createRoutine(RoutineModel model)
        {
            createRoutine(AccountHelper.getCurrentUsername(), model);
        }

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

        public void disableRoutine(string routineName)
        {
            disableRoutine(AccountHelper.getCurrentUsername(), routineName);
        }

        public void disableRoutine(string creatorUsername, string routineName)
        {
            disableMostRecentRoutine(creatorUsername, routineName);

            if (routineIsActive(creatorUsername, routineName))
                disableAllRoutinesByName(creatorUsername, routineName);
        }

        public void disableMostRecentRoutine(string routineName)
        {
            disableMostRecentRoutine(AccountHelper.getCurrentUsername(), routineName);
        }

        public void disableMostRecentRoutine(string creatorUsername, string routineName)
        {
            Routine r = getMostRecentRoutineByName(creatorUsername, routineName);
            r.isDisabled = true;
            db.Entry<Routine>(r).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }

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

        public void disableAllRoutinesByName(string routineName)
        {
            disableAllRoutinesByName(AccountHelper.getCurrentUsername(), routineName);
        }

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

        //Will always be the most recent
        public void enableRoutine(string routineName)
        {
            enableRoutine(AccountHelper.getCurrentUsername(), routineName);
        }

        //Will always be the most recent
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

        public void deleteRoutine(string routineName, bool deleteAll)
        {
            deleteRoutine(AccountHelper.getCurrentUsername(), routineName, deleteAll);
        }

        public void deleteRoutine(string creatorUsername, string routineName, bool deleteAll)
        {
            if (deleteAll)
                deleteAllVersionsOfRoutine(creatorUsername, routineName);
            else
                deleteMostRecentRoutine(creatorUsername, routineName);
        }

        public void deleteRoutine(Routine routine)
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

        public void deleteAllVersionsOfRoutine(string routineName)
        {
            deleteAllVersionsOfRoutine(AccountHelper.getCurrentUsername(), routineName);
        }

        public void deleteAllVersionsOfRoutine(string creatorUsername, string routineName)
        {
            foreach (Routine r in getRoutinesByName(creatorUsername, routineName))
                deleteRoutine(r);
        }

        public void deleteMostRecentRoutine(string routineName)
        {
            deleteMostRecentRoutine(AccountHelper.getCurrentUsername(), routineName);
        }

        public void deleteMostRecentRoutine(string creatorUsername, string routineName)
        {
            deleteRoutine(getMostRecentRoutineByName(creatorUsername, routineName));
        }

        public void dispose()
        {
            db.Dispose();
            feedbackHelper.dispose();
            jobHelper.dispose();
            taskHelper.dispose();
        }
    }
}