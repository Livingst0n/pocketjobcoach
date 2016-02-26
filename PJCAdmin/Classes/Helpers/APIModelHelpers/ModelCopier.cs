using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.APIModelHelpers
{
    /* ---------------------------------------------------------
     * The ModelCopier class provides common methods for copying 
     * database model objects to WebAPI serialization compatable
     * model objects. Most object obtained directly from the db
     * have object overhead that causes serialization errors. 
     * This class remedies that problem.
     * ---------------------------------------------------------
     */
    public class ModelCopier
    {
        #region Non-Enum Models
        public static Routine copyRoutine(Routine r)
        {
            if (r == null)
                return null;

            Routine newRoutine = new Routine()
            {
                updatedDate = r.updatedDate,
                creatorUserName = r.creatorUserName,
                expectedDuration = r.expectedDuration,
                isNotifiable = r.isNotifiable,
                isTimed = r.isTimed,
                Jobs = r.Jobs, //Not copied; not serialized from Routine
                routineID = r.routineID,
                routineTitle = r.routineTitle,
                assigneeUserName = r.assigneeUserName,
                UserName = r.UserName, //Not copied; not serialized from Routine
                UserName1 = r.UserName1, //Not copied; not serialized from Routine
                isDisabled = r.isDisabled,
            };

            foreach (Task t in r.Tasks)
                newRoutine.Tasks.Add(copyTask(t));

            foreach (Feedback f in r.Feedbacks)
                newRoutine.Feedbacks.Add(copyFeedback(f));
            
            return newRoutine;
        }
        public static Task copyTask(Task t)
        {
            if (t == null)
                return null;

            Task newTask = new Task()
            {
                expectedDuration = t.expectedDuration,
                isTimed = t.isTimed,
                Routine = t.Routine, //Not copied; not serialized from Task
                routineID = t.routineID,
                sequenceNo = t.sequenceNo,
                taskCategoryID = t.taskCategoryID,
                taskDescription = t.taskDescription,
                taskName = t.taskName,
                taskID = t.taskID
            };

            newTask.TaskCategory = copyTaskCategory(t.TaskCategory);

            foreach (Feedback f in t.Feedbacks)
                newTask.Feedbacks.Add(copyFeedback(f));

            return newTask;
        }
        public static Feedback copyFeedback(Feedback f)
        {
            if (f == null)
                return null;

            Feedback newFeedback = new Feedback()
            {
                feedbackID = f.feedbackID,
                feedbackMessage = f.feedbackMessage,
                feedbackTitle = f.feedbackTitle,
                feedbackTypeID = f.feedbackTypeID,
                mediaTypeID = f.mediaTypeID,
                Routines = f.Routines, //Not copied; not serialized from Feedback
                Tasks = f.Tasks //Not copied; not serialized from Feedback
            };

            newFeedback.FeedbackType = copyFeedbackType(f.FeedbackType);
            newFeedback.MediaType = copyMediaType(f.MediaType);

            return newFeedback;
        }
        #endregion
        #region Enum Models
        public static TaskCategory copyTaskCategory(TaskCategory tc)
        {
            if (tc == null)
                return null;

            TaskCategory newTC = new TaskCategory()
            {
                categoryName = tc.categoryName,
                taskCategoryID = tc.taskCategoryID,
                Tasks = tc.Tasks //Not copied; not serialized from TaskCategory
            };
            return newTC;
        }
        public static FeedbackType copyFeedbackType(FeedbackType ft)
        {
            if (ft == null)
                return null;

            FeedbackType newFT = new FeedbackType()
            {
                Feedbacks = ft.Feedbacks, //Not copied; not serialized from FeedbackType
                feedbackTypeID = ft.feedbackTypeID,
                feedbackTypeName = ft.feedbackTypeName
            };
            return newFT;
        }
        public static MediaType copyMediaType(MediaType mt)
        {
            if (mt == null)
                return null;

            MediaType newMT = new MediaType()
            {
                Feedbacks = mt.Feedbacks, //Not copied; not serialized from MediaType
                mediaTypeID = mt.mediaTypeID,
                mediaTypeName = mt.mediaTypeName
            };
            return newMT;
        }
        #endregion
    }
}