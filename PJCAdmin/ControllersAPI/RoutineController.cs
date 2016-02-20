using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;
using PJCAdmin.Classes;

namespace PJCAdmin.ControllersAPI
{
    public class RoutineController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        // GET api/Routine?token=<token>
        public IEnumerable<Routine> Get(string token)
        {
            /*
            APIAuth.authorizeToken(token);
            string userName = APIAuth.getUserNameFromToken(token);
            List<Routine> assignedRoutines = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                assignedRoutines.Add(copyRoutine(r));
            }

            return assignedRoutines;
            */

            List<Routine> routines = new List<Routine>();

            foreach (Routine r in db.Routines)
            {
                //routines.Add(r); //Does not work
                routines.Add(copyRoutine(r)); //Works
            }

            return routines;

            /*List<Task> tasks = new List<Task>();
            foreach (Task t in db.Tasks)
            {
                tasks.Add(copyTask(t));
            }
            return tasks;*/

        }

        // GET api/Routine?token=<token>&assignedBy=<"Parent" or "Job Coach">
        public IEnumerable<Routine> Get(string token, string assignedBy)
        {
            APIAuth.authorizeToken(token);
            string userName = APIAuth.getUserNameFromToken(token);

            if (assignedBy.Equals("Parent"))
                return getRoutinesAssignedByParent(userName);
            if (assignedBy.Equals("Job Coach"))
                return getRoutinesAssignedByJobCoach(userName);

            //assignedBy is not a valid string
            throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
        }

        private List<Routine> getRoutinesAssignedByParent(string userName)
        {
            string parentUserName = db.UserNames.Find(userName).guardianUserName;

            List<Routine> routinesByParent = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                if (r.creatorUserName.Equals(parentUserName))
                    routinesByParent.Add(r);
            }

            return routinesByParent;
        }

        private List<Routine> getRoutinesAssignedByJobCoach(string userName)
        {
            string jobCoachUserName = db.UserNames.Find(userName).jobCoachUserName;

            List<Routine> routinesByJobCoach = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                if (r.creatorUserName.Equals(jobCoachUserName))
                    routinesByJobCoach.Add(r);
            }

            return routinesByJobCoach;
        }

        private Routine copyRoutine(Routine r)
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

        private Task copyTask(Task t)
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

        private MessageType copyMessageType(MessageType mt)
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

        private TaskCategory copyTaskCategory(TaskCategory tc)
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
