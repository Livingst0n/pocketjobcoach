using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.APIModelHelpers
{
    /* --------------------------------------------------------
     * The RoutineHelper class provides common methods relating 
     * to Routines for the WebAPI service.
     * --------------------------------------------------------
     */
    public class RoutineHelper
    {
        private DbHelper helper = new DbHelper();

        #region Getters
        /*Returns a list of all routines assigned to the given 
         * user by the user listed as their parent. Returned
         * routines have been passed through the modelcopier
         * and are valid for serialization.
         * @param userName: The username for the child who is 
         * assigned the routines by their parent.
         */
        public List<Routine> getRoutinesAssignedByParentForSerialization(string userName)
        {
            string parentUserName = helper.findUserName(userName).guardianUserName;

            List<Routine> routinesByParent = new List<Routine>();

            foreach (Routine r in helper.findUserName(userName).Routines1)
            {
                if (r.creatorUserName.Equals(parentUserName))
                    routinesByParent.Add(ModelCopier.copyRoutine(r));
            }

            return routinesByParent;
        }
        /*Returns a list of all routines assigned to the given 
         * user by the user listed as their job coach. Returned
         * routines have been passed through the modelcopier
         * and are valid for serialization.
         * @param userName: The username for the user who is 
         * assigned the routines by their job coach.
         */
        public List<Routine> getRoutinesAssignedByJobCoachForSerialization(string userName)
        {
            string jobCoachUserName = helper.findUserName(userName).jobCoachUserName;

            List<Routine> routinesByJobCoach = new List<Routine>();

            foreach (Routine r in helper.findUserName(userName).Routines1)
            {
                if (r.creatorUserName.Equals(jobCoachUserName))
                    routinesByJobCoach.Add(ModelCopier.copyRoutine(r));
            }

            return routinesByJobCoach;
        }
        /*Returns a list of all routines assigned to the given 
         * user by both job coach and parent. Returned routines 
         * have been passed through the modelcopier and are 
         * valid for serialization.
         * @param userName: The username for the user who is 
         * assigned the routines by their job coach and parent.
         */
        public List<Routine> getAllRoutinesAssignedToUserForSerialization(string userName)
        {
            List<Routine> assignedRoutines = new List<Routine>();

            foreach (Routine r in helper.findUserName(userName).Routines1)
            {
                assignedRoutines.Add(ModelCopier.copyRoutine(r));
            }

            return assignedRoutines;
        }
        #endregion

        public void dispose()
        {
            helper.dispose();
        }
    }
}