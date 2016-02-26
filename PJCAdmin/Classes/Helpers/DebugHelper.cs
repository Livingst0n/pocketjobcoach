using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers
{
    /* --------------------------------------------------------
     * The DebugHelper class provides a method to debug through
     * the database bypassing the User Interface for debug 
     * output.
     * --------------------------------------------------------
     */
    public class DebugHelper
    {
        private pjcEntities db = new pjcEntities();

        public void createDebugMessageInDatabase(string message){
            db.Debugs.Add(new Debug() { debugMessage = message.Substring(0, 199) });
            db.SaveChanges();
        }

        public void dispose()
        {
            db.Dispose();
        }
    }
}