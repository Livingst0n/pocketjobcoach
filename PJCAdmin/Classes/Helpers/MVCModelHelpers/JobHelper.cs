using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    public class JobHelper
    {
        private pjcEntities db = new pjcEntities();

        public bool jobsExistForRoutine(int routineID)
        {
            return db.Routines.Find(routineID).Jobs.Count() > 0;
        }
    }
}