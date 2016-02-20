using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.APIModelHelpers
{
    public class ModelCopier
    {
        public static Routine copyRoutine(Routine r)
        {
            if (r == null)
                return null;

            Routine newRoutine = new Routine()
            {
                assignedOrUpdatedDate = r.assignedOrUpdatedDate,
                creatorUserName = r.creatorUserName,
                expectedDuration = r.expectedDuration,
                isNotifiable = r.isNotifiable,
                isTimed = r.isTimed,
                Jobs = r.Jobs, //Not copied; not serialized from Routine
                MessageType = copyMessageType(r.MessageType),
                MessageType1 = copyMessageType(r.MessageType1),
                negativeFeedbackMessage = r.negativeFeedbackMessage,
                negativeFeedbackTitle = r.negativeFeedbackTitle,
                negativeFeedbackTypeID = r.negativeFeedbackTypeID,
                positiveFeedbackMessage = r.positiveFeedbackMessage,
                positiveFeedbackTitle = r.positiveFeedbackTitle,
                positiveFeedbackTypeID = r.positiveFeedbackTypeID,
                routineID = r.routineID,
                routineTitle = r.routineTitle,
                userName = r.userName,
                UserName1 = r.UserName1, //Not copied; not serialized from Routine
                UserName2 = r.UserName2 //Not copied; not serialized from Routine
            };

            foreach (Task t in r.Tasks)
            {
                newRoutine.Tasks.Add(copyTask(t));
            }

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
                MessageType = copyMessageType(t.MessageType),
                MessageType1 = copyMessageType(t.MessageType1),
                MessageType2 = copyMessageType(t.MessageType2),
                negativeFeedbackMessage = t.negativeFeedbackMessage,
                negativeFeedbackTitle = t.negativeFeedbackTitle,
                negativeFeedbackTypeID = t.negativeFeedbackTypeID,
                positiveFeedbackMessage = t.positiveFeedbackMessage,
                positiveFeedbackTitle = t.positiveFeedbackTitle,
                positiveFeedbackTypeID = t.positiveFeedbackTypeID,
                promptMessage = t.promptMessage,
                promptTitle = t.promptTitle,
                promptTypeID = t.promptTypeID,
                Routine = t.Routine, //Not copied; not serialized from Task
                routineID = t.routineID,
                sequenceNo = t.sequenceNo,
                taskCategoryID = t.taskCategoryID,
                taskDescription = t.taskDescription,
                taskName = t.taskName
            };

            newTask.TaskCategory = copyTaskCategory(t.TaskCategory);

            return newTask;
        }

        public static MessageType copyMessageType(MessageType mt)
        {
            if (mt == null)
                return null;

            MessageType newMT = new MessageType()
            {
                messageTypeID = mt.messageTypeID,
                messageTypeName = mt.messageTypeName,
                Routines = mt.Routines, //Not copied; not serialized from MessageType
                Routines1 = mt.Routines1, //Not copied; not serialized from MessageType
                Tasks = mt.Tasks, //Not copied; not serialized from MessageType
                Tasks1 = mt.Tasks1, //Not copied; not serialized from MessageType
                Tasks2 = mt.Tasks2 //Not copied; not serialized from MessageType
            };

            return newMT;
        }

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
    }
}