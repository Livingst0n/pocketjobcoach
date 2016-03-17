using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* --------------------------------------------------------
     * The TaskHelper class provides common methods relating 
     * to Tasks for the MVC service.
     * --------------------------------------------------------
     */
    public class TaskHelper
    {
        private FeedbackHelper feedbackHelper = new FeedbackHelper();
        private EnumHelper enumHelper = new EnumHelper();
        private DbHelper helper = new DbHelper();

        #region Creating, Updating, and Deleting
        /* Creates a task from the given model for the given 
         * routine.
         * @param routineID: The unique ID of the routine.
         * @param model: The task model to create the task from.
         */
        public void createTask(int routineID, TaskModel model)
        {
            Task t = new Task()
            {
                routineID = routineID,
                sequenceNo =  model.sequenceNo,
                taskName = model.taskName,
                taskDescription = model.taskDescription,
                isTimed = model.isTimed,
                expectedDuration = model.expectedDuration
            };

            t.TaskCategory = enumHelper.getTaskCategory(model.TaskCategory.categoryName);

            t = helper.createTask(t);

            foreach (FeedbackModel f in model.Feedbacks)
                feedbackHelper.createTaskFeedback(t.taskID, f);
        }
        /* Updates the existing tasks from the given tasks
         * to match the task models.
         * @param tasks: The existing tasks to update.
         * @param models: The task models to match existing
         * tasks to.
         * @param jobsExist: Do jobs exist for the routine 
         * existing tasks are from? (if jobs exist, changes
         * are limited).
         */
        public void modifyExistingTasks(List<Task> tasks, List<TaskModel> models, bool jobsExist)
        {
            tasks = tasks.OrderBy(t => t.sequenceNo).ToList();
            models = models.OrderBy(t => t.sequenceNo).ToList();
            int routineID = tasks.First().routineID;

            if (!(isTaskModelListValid(models) && isTaskListValid(tasks)))
                return; //TODO Error

            foreach (Task task in tasks.Where(t => t.sequenceNo > models.Count()))
                deleteTask(task);

            foreach (Task t in tasks)
                modifyExistingTask(t, models.Where(m => m.sequenceNo == t.sequenceNo).First(), jobsExist);

            foreach (TaskModel model in models.Where(m => m.sequenceNo > tasks.Count()))
                createTask(routineID, model);
        }
        /* Updates the existing task from the given task
         * to match the task model.
         * @param task: The existing task to update.
         * @param model: The task model to match existing
         * task to.
         * @param jobsExist: Do jobs exist for the routine
         * existing tasks are from? (if jobs exist, changes
         * are limited).
         */
        public void modifyExistingTask(Task task, TaskModel model, bool jobsExist)
        {
            if (task.sequenceNo != model.sequenceNo)
                return; //sequenceNo's have to match

            task.taskName = model.taskName;
            task.taskDescription = model.taskDescription;
            if (!jobsExist)
            {
                task.isTimed = model.isTimed;
                task.expectedDuration = model.expectedDuration;
            }

            task.TaskCategory = enumHelper.getTaskCategory(model.TaskCategory.categoryName);

            task = helper.updateTask(task);

            feedbackHelper.updateTaskFeedbacks(task.taskID, model.Feedbacks.ToList());
        }
        /* Removes the given task from the database.
         */
        public void deleteTask(Task task)
        {
            //TODO validate orphaned Feedbacks are deleted
            helper.deleteTask(task);
        }
        #endregion
        #region Private Methods
        /* Returns whether the list of task models are 
         * valid for updating.
         * Checks for valid sequenceNo's.
         * @param models: The list of task models to 
         * check for validity.
         */
        private bool isTaskModelListValid(List<TaskModel> models)
        {
            models = models.OrderBy(t => t.sequenceNo).ToList();

            byte seqNo = 1;

            foreach (TaskModel t in models)
            {
                if (t.sequenceNo != seqNo++) //must be 1,2,3,4,etc in order
                    return false;
            }
            return true;
        }
        /* Returns whether the list of tasks are 
         * valid for updating.
         * Checks for valid sequenceNo's and 
         * for identical routineID's.
         * @param tasks: The list of tasks to check
         * for validity.
         */
        private bool isTaskListValid(List<Task> tasks)
        {
            tasks = tasks.OrderBy(t => t.sequenceNo).ToList();

            int routineID = tasks.First().routineID;
            byte seqNo = 1;

            foreach (Task t in tasks)
            {
                if (t.sequenceNo != seqNo++) //must be 1,2,3,4,etc in order
                    return false;
                if (t.routineID != routineID) //must all belong to same routine
                    return false;
            }
            return true;
        }
        #endregion
        
        public void dispose()
        {
            helper.dispose();
            feedbackHelper.dispose();
        }
    }
}