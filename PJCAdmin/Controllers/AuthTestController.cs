using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;
using PJCAdmin.Classes;

namespace PJCAdmin.Controllers
{
    public class AuthTestController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        // GET api/AuthTest
        public string Get(string token)
        {
            APIAuth.authorizeToken(token);

            Guid userID = APIAuth.getUserIDFromToken(token);

            IEnumerable<AuthTest> results = db.AuthTests.Where(a => a.UserID.Equals(userID)).AsEnumerable();

            string result = "";

            if (results.Count() > 0 )
                 result = results.First().TestMessage;

            //UserID as a Guid is causing an internal 500 error when serializing for http response.
            return result;
        }

        // POST api/AuthTest
        public HttpResponseMessage Post(string token, AuthTest test)
        {
            APIAuth.authorizeToken(token);

            test.UserID = APIAuth.getUserIDFromToken(token);

            db.AuthTests.Add(test);
            db.SaveChanges();

            var response = Request.CreateResponse<AuthTest>(HttpStatusCode.Created,test);
            //var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
