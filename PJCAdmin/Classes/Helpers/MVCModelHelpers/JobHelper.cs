using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.MVCModelHelpers
{
    /* --------------------------------------------------------
     * The JobHelper class provides common methods relating 
     * to Jobs for the MVC service.
     * --------------------------------------------------------
     */
    public class JobHelper
    {
        private pjcEntities db = new pjcEntities();

        /* Returns whether or not jobs have been created
         * for the given routine.
         * @param routineID: The unique ID for the routine.
         */
        public bool jobsExistForRoutine(int routineID)
        {
            return db.Routines.Find(routineID).Jobs.Count() > 0;
        }

        public void dispose()
        {
            db.Dispose();
        }
    }
}