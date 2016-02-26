using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;
using PJCAdmin.Classes;
using PJCAdmin.Classes.Helpers.APIModelHelpers;

namespace PJCAdmin.ControllersAPI
{
    public class RoutineController : ApiController
    {
        private RoutineHelper helper = new RoutineHelper();

        // GET api/Routine?token=<token>
        public IEnumerable<Routine> Get(string token)
        {
            APIAuth.authorizeToken(token);
            string userName = APIAuth.getUserNameFromToken(token);
            
            return helper.getAllRoutinesAssignedToUserForSerialization(userName);
        }

        // GET api/Routine?token=<token>&assignedBy=<"Parent" or "Job Coach">
        public IEnumerable<Routine> Get(string token, string assignedBy)
        {
            APIAuth.authorizeToken(token);
            string userName = APIAuth.getUserNameFromToken(token);

            if (assignedBy.Equals("Parent"))
                return helper.getRoutinesAssignedByParentForSerialization(userName);
            if (assignedBy.Equals("Job Coach"))
                return helper.getRoutinesAssignedByJobCoachForSerialization(userName);

            //assignedBy is not a valid string
            throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));
        }

        protected override void Dispose(bool disposing)
        {
            helper.dispose();
            base.Dispose(disposing);
        }
    }
}
