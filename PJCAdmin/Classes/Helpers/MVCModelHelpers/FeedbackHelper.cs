using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    public class FeedbackHelper
    {
        private pjcEntities db = new pjcEntities();

        public void updateRoutineFeedbacks(int routineID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                updateRoutineFeedback(routineID, model);
            }
        }

        public void updateTaskFeedbacks(int taskID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                updateTaskFeedback(taskID, model);
            }
        }

        public void updateRoutineFeedback(int routineID, FeedbackModel model)
        {
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

        public void updateTaskFeedback(int taskID, FeedbackModel model)
        {
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

        public void createRoutineFeedbacks(int routineID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                createRoutineFeedback(routineID, model);
            }
        }

        public void createTaskFeedbacks(int taskID, List<FeedbackModel> models)
        {
            foreach (FeedbackModel model in models)
            {
                createTaskFeedback(taskID, model);
            }
        }

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

        private Feedback createFeedback(FeedbackModel model)
        {
            Feedback feedback = new Feedback()
            {
                feedbackTitle = model.feedbackTitle,
                feedbackMessage = model.feedbackMessage
            };
            feedback.FeedbackType = getMatchingFeedbackType(model.FeedbackType.feedbackTypeName);
            feedback.MediaType = getMatchingMediaType(model.MediaType.mediaTypeName);

            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return feedback;
        }

        private Feedback getMatchingFeedback(FeedbackModel model)
        {
            byte mediaTypeID = getMatchingMediaType(model.MediaType.mediaTypeName).mediaTypeID;
            byte feedbackTypeID = getMatchingFeedbackType(model.FeedbackType.feedbackTypeName).feedbackTypeID;

            List<Feedback> lst = db.Feedbacks.Where(f => f.mediaTypeID == mediaTypeID && f.feedbackID == feedbackTypeID).ToList();
            foreach (Feedback f in lst)
            {
                if (f.feedbackTitle.Equals(model.feedbackTitle) && f.feedbackMessage.Equals(model.feedbackMessage))
                    return f;
            }

            return null;
        }

        private MediaType getMatchingMediaType(string typeName)
        {
            return db.MediaTypes.Where(t => t.mediaTypeName.Equals(typeName)).First();
        }

        private FeedbackType getMatchingFeedbackType(string typeName)
        {
            return db.FeedbackTypes.Where(t => t.feedbackTypeName.Equals(typeName)).First();
        }

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

        //TODO delete processes if .Remove function doesn't remove orphaned children
    }
}