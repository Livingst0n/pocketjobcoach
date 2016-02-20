using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.APIModelHelpers
{
    public class RoutineHelper
    {
        private pjcEntities db = new pjcEntities();

        public List<Routine> getRoutinesAssignedByParentForSerialization(string userName)
        {
            string parentUserName = db.UserNames.Find(userName).guardianUserName;

            List<Routine> routinesByParent = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                if (r.creatorUserName.Equals(parentUserName))
                    routinesByParent.Add(ModelCopier.copyRoutine(r));
            }

            return routinesByParent;
        }

        public List<Routine> getRoutinesAssignedByJobCoachForSerialization(string userName)
        {
            string jobCoachUserName = db.UserNames.Find(userName).jobCoachUserName;

            List<Routine> routinesByJobCoach = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                if (r.creatorUserName.Equals(jobCoachUserName))
                    routinesByJobCoach.Add(ModelCopier.copyRoutine(r));
            }

            return routinesByJobCoach;
        }

        public List<Routine> getAllRoutinesAssignedToUserForSerialization(string userName)
        {
            List<Routine> assignedRoutines = new List<Routine>();

            foreach (Routine r in db.UserNames.Find(userName).Routines1)
            {
                assignedRoutines.Add(ModelCopier.copyRoutine(r));
            }

            return assignedRoutines;
        }
    }
}