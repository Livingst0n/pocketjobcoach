using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* ---------------------------------------------------------
     * The FeedbackHelper class provides common methods relating 
     * to Feedback for the MVC service.
     * ---------------------------------------------------------
     */
    public class FeedbackHelper
    {
        private pjcEntities db = new pjcEntities();
        private EnumHelper helper = new EnumHelper();

        #region Feedback
        /* Returns the matching Feedback record.
         * Returns null if no matching record exists.
         * @param model: The model of the Feedback record
         * desired.
         */
        private Feedback getMatchingFeedback(FeedbackModel model)
        {
            byte mediaTypeID = helper.getMediaType(model.MediaType.mediaTypeName).mediaTypeID;
            byte feedbackTypeID = helper.getFeedbackType(model.FeedbackType.feedbackTypeName).feedbackTypeID;

            List<Feedback> lst = db.Feedbacks.Where(f => f.mediaTypeID == mediaTypeID && f.feedbackID == feedbackTypeID).ToList();
            foreach (Feedback f in lst)
            {
                if (f.feedbackTitle.Equals(model.feedbackTitle) && f.feedbackMessage.Equals(model.feedbackMessage))
                    return f;
            }

            return null;
        }
        /* Creates a new Feedback record matching
         * the given model.
         * @param model: The model of the Feedback record
         * to create.
         */
        private Feedback createFeedback(FeedbackModel model)
        {
            Feedback feedback = new Feedback()
            {
                feedbackTitle = model.feedbackTitle,
                feedbackMessage = model.feedbackMessage
            };
            feedback.FeedbackType = helper.getFeedbackType(model.FeedbackType.feedbackTypeName);
            feedback.MediaType = helper.getMediaType(model.MediaType.mediaTypeName);

            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return feedback;
        }
        #endregion
        #region RoutineFeedback
        /* Creates a new RoutineFeedback connection
         * between the given feedback and the given 
         * routine. Creates a new feedback record if
         * no match exists.
         * Returns the matching feedback that has been
         * connected.
         * @param routineID: The unique ID for the routine.
         * @param model: The feedback model to match.
         */
        public Feedback createRoutineFeedback(int routineID, FeedbackModel model)
        {
            Feedback feedback = getMatchingFeedback(model);
            if (feedback == null)
                feedback = createFeedback(model);

            feedback.Routines.Add(db.Routines.Find(routineID));
            db.Entry<Feedback>(feedback).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return feedback;
        }
        /* Returns whether a RoutineFeedback connection
         * already exists between the given feedback and 
         * the given routine.
         * @param routineID: The unique ID for the routine.
         * @param model: The feedback model to match.
         */
        private bool routineFeedbackExists(int routineID, FeedbackModel model)
                {
                    Feedback feedback = getMatchingFeedback(model);
                    if (feedback == null)
                        return false;

                    foreach (Routine r in feedback.Routines)
                    {
                        if (r.routineID == routineID)
                            return true;
                    }
                    return false;
                }
        /* Updates the feedbacks connected to the given routine.
         * @param routineID: The unique ID for the routine.
         * @param models: The feedback models to match.
         */
        public void updateRoutineFeedbacks(int routineID, List<FeedbackModel> models)
        {
            //TODO check for old Feedback associations
            foreach (FeedbackModel model in models)
            {
                updateRoutineFeedback(routineID, model);
            }
        }
        /* Updates the given feedback connected to the given 
         * routine.
         * @param routineID: The unique ID for the routine.
         * @param model: The feedback model to match.
         */
        public void updateRoutineFeedback(int routineID, FeedbackModel model)
        {
            //TODO need oldModel and newModel?
            if (routineFeedbackExists(routineID, model))
                return; //No changes are needed
            
            Feedback feedback = getMatchingFeedback(model);
            if (feedback == null)
            {
                feedback = createRoutineFeedback(routineID, model);
                return;
            }

            feedback.Routines.Add(db.Routines.Find(routineID));
            db.Entry<Feedback>(feedback).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        /* Creates a RoutineFeedback connection for each 
         * given feedback to the given routine.
         * @param routineID: The unique ID for the routine.
         * @param models: The feedbacks to create connections to.
         */
        public void createRoutineFeedbacks(int routineID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                createRoutineFeedback(routineID, model);
            }
        }
        #endregion
        #region TaskFeedback
        /* Creates a new TaskFeedback connection
         * between the given task and the given 
         * routine. Creates a new feedback record if 
         * no match exists.
         * Returns the matching feedback that has been
         * connected.
         * @param taskID: The unique ID for the task.
         * @param model: The feedback model to match.
         */
        public Feedback createTaskFeedback(int taskID, FeedbackModel model)
        {
            Feedback feedback = getMatchingFeedback(model);
            if (feedback == null)
                feedback = createFeedback(model);

            feedback.Tasks.Add(db.Tasks.Find(taskID));
            db.Entry<Feedback>(feedback).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            return feedback;
        }
        /* Returns whether a TaskFeedback connection
         * already exists between the given feedback and
         * the given task.
         * @param taskID: The unique ID for the task.
         * @param model: The feedback model to match.
         */
        private bool taskFeedbackExists(int taskID, FeedbackModel model)
        {
            Feedback feedback = getMatchingFeedback(model);
            if (feedback == null)
                return false;

            foreach (Task t in feedback.Tasks)
            {
                if (t.taskID == taskID)
                    return true;
            }
            return false;
        }
        /* Updates the feedbacks connected to the given task.
         * @param taskID: The unique ID for the task.
         * @param models: The feedback models to match.
         */
        public void updateTaskFeedbacks(int taskID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                updateTaskFeedback(taskID, model);
            }
        }
        /* Updates the given feedback connected to the given
         * task.
         * @param taskID: The unique ID for the task.
         * @param model: The feedback model to match.
         */
        public void updateTaskFeedback(int taskID, FeedbackModel model)
        {
            //TODO need old and new model?
            if (taskFeedbackExists(taskID, model))
                return; //No changes are needed

            Feedback feedback = getMatchingFeedback(model);
            if (feedback == null)
            {
                feedback = createTaskFeedback(taskID, model);
                return;
            }

            feedback.Tasks.Add(db.Tasks.Find(taskID));
            db.Entry<Feedback>(feedback).State = System.Data.EntityState.Modified;
            db.SaveChanges();
        }
        /* Creates a TaskFeedback connection for each
         * given feedback to the given task.
         * @param taskID: The unique ID for the task.
         * @param models: The feedbacks to create connections to.
         */
        public void createTaskFeedbacks(int taskID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                createTaskFeedback(taskID, model);
            }
        }
        #endregion
        
        //TODO delete processes if .Remove function doesn't remove orphaned children

        public void dispose()
        {
            db.Dispose();
        }
    }
}