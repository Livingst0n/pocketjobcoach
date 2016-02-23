using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    public class RoutineHelper
    {
        private pjcEntities db = new pjcEntities();

        public List<string> getRoutineNamesCreatedByUser(string username)
        {
            //TODO
            List<string> lstRoutines = new List<string>();
            return lstRoutines;
        }

        public RoutineModel getRoutineByName(string routineName)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            return adminGetRoutineByName(thisUsername, routineName);
        }

        public RoutineModel adminGetRoutineByName(string user, string routineName)
        {
            //TODO
            RoutineModel routine = new RoutineModel();
            return routine;
        }

        public bool routineExists(string routineName)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            return adminRoutineExists(thisUsername, routineName);
        }

        public bool adminRoutineExists(string user, string routineName)
        {
            //TODO
            return true;
        }

        public void updateRoutine(string routineName, RoutineModel model)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            adminUpdateRoutine(thisUsername, routineName, model);
        }

        public void adminUpdateRoutine(string user, string routineName, RoutineModel model)
        {
            //TODO
        }

        public void createRoutine(RoutineModel model)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            adminCreateRoutine(thisUsername, model);
        }

        public void adminCreateRoutine(string user, RoutineModel model)
        {
            //TODO
        }

        public void deleteRoutine(string routineName)
        {
            string thisUsername = System.Web.Security.Membership.GetUser().UserName;
            adminDeleteRoutine(thisUsername, routineName);
        }

        public void adminDeleteRoutine(string user, string routineName)
        {
            //TODO
        }
    }
}