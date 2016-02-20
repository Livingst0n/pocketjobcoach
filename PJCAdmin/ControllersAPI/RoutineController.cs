using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;
using PJCAdmin.Classes;
using PJCAdmin.Classes.APIModelHelpers;

namespace PJCAdmin.ControllersAPI
{
    public class RoutineController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        // GET api/Routine?token=<token>
        public IEnumerable<Routine> Get(string token)
        {
            APIAuth.authorizeToken(token);
            string userName = APIAuth.getUserNameFromToken(token);
            List<Routine> assignedRoutines = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                assignedRoutines.Add(ModelCopier.copyRoutine(r));
            }

            return assignedRoutines;
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
    }
}
