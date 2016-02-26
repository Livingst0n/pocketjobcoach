using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;
using PJCAdmin.Classes.Helpers.APIModelHelpers;

namespace PJCAdmin.ControllersAPI
{
    public class TestController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        public IEnumerable<Routine> Get()
        {
            List<Routine> routines = new List<Routine>();

            foreach (Routine r in db.Routines)
            {
                //routines.Add(r); //Does not work
                routines.Add(ModelCopier.copyRoutine(r)); //Works
            }

            return routines;

            /*List<Task> tasks = new List<Task>();
            foreach (Task t in db.Tasks)
            {
                tasks.Add(copyTask(t));
            }
            return tasks;*/
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
