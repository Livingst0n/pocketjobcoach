using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* ---------------------------------------------------------
     * The ModelCopier class provides common methods for copying 
     * database model objects to MVC user display compatable
     * model objects. Objects obtained directly from the db
     * do not have display parameters for User Interface. 
     * This class remedies that problem.
     * ---------------------------------------------------------
     */
    public class ModelCopier
    {
        #region Non-Enum Models
        public static List<RoutineModel> copyRoutinesToModels(List<Routine> rs)
        {
            if (rs == null || rs.Count() == 0)
                return null;

            List<RoutineModel> lst = new List<RoutineModel>();
            foreach (Routine r in rs)
                lst.Add(copyRoutineToModel(r));

            return lst;
        }
        public static RoutineModel copyRoutineToModel(Routine r)
        {
            if (r == null)
                return null;

            RoutineModel newRoutine = new RoutineModel()
            {
                expectedDuration = r.expectedDuration,
                isNotifiable = r.isNotifiable,
                isTimed = r.isTimed,
                routineTitle = r.routineTitle,
                assigneeUserName = r.assigneeUserName,
                isDisabled = r.isDisabled,
            };

            foreach (Task t in r.Tasks)
                newRoutine.Tasks.Add(copyTaskToModel(t));

            foreach (Feedback f in r.Feedbacks)
                newRoutine.Feedbacks.Add(copyFeedbackToModel(f));

            return newRoutine;
        }
        public static List<TaskModel> copyTasksToModel(List<Task> ts)
        {
            if (ts == null || ts.Count() == 0)
                return null;

            List<TaskModel> lst = new List<TaskModel>();
            foreach (Task t in ts)
                lst.Add(copyTaskToModel(t));

            return lst;
        }
        public static TaskModel copyTaskToModel(Task t)
        {
            if (t == null)
                return null;

            TaskModel newTask = new TaskModel()
            {
                expectedDuration = t.expectedDuration,
                isTimed = t.isTimed,
                sequenceNo = t.sequenceNo,
                taskDescription = t.taskDescription,
                taskName = t.taskName
            };

            newTask.TaskCategory = copyTaskCategoryToModel(t.TaskCategory);

            foreach (Feedback f in t.Feedbacks)
                newTask.Feedbacks.Add(copyFeedbackToModel(f));

            return newTask;
        }
        public static List<FeedbackModel> copyFeedbacksToModel(List<Feedback> fs)
        {
            if (fs == null || fs.Count() == 0)
                return null;

            List<FeedbackModel> lst = new List<FeedbackModel>();
            foreach (Feedback f in fs)
                lst.Add(copyFeedbackToModel(f));

            return lst;
        }
        public static FeedbackModel copyFeedbackToModel(Feedback f)
        {
            if (f == null)
                return null;

            FeedbackModel newFeedback = new FeedbackModel()
            {
                feedbackMessage = f.feedbackMessage,
                feedbackTitle = f.feedbackTitle
            };

            newFeedback.FeedbackType = copyFeedbackTypeToModel(f.FeedbackType);
            newFeedback.MediaType = copyMediaTypeToModel(f.MediaType);

            return newFeedback;
        }
        #endregion
        #region Enum Models
        public static TaskCategoryModel copyTaskCategoryToModel(TaskCategory tc)
        {
            if (tc == null)
                return null;

            TaskCategoryModel newTC = new TaskCategoryModel()
            {
                categoryName = tc.categoryName
            };
            return newTC;
        }
        public static FeedbackTypeModel copyFeedbackTypeToModel(FeedbackType ft)
        {
            if (ft == null)
                return null;

            FeedbackTypeModel newFT = new FeedbackTypeModel()
            {
                feedbackTypeName = ft.feedbackTypeName
            };
            return newFT;
        }
        public static MediaTypeModel copyMediaTypeToModel(MediaType mt)
        {
            if (mt == null)
                return null;

            MediaTypeModel newMT = new MediaTypeModel()
            {
                mediaTypeName = mt.mediaTypeName
            };
            return newMT;
        }
        #endregion
    }
}