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
        public IEnumerable<AuthTest> Get(string token)
        {
            APIAuth.authorizeToken(token);

            string userName = APIAuth.getUserNameFromToken(token);

            return db.AuthTests.Where(a => a.UserName.Equals(userName));
        }

        // POST api/AuthTest?token=<token>
        //public HttpResponseMessage Post(Token<AuthTest> packet)
        public HttpResponseMessage Post(string token, AuthTest test)
        {
            //string token = packet.token;
            //AuthTest test = packet.obj;

            APIAuth.authorizeToken(token);

            test.UserName = APIAuth.getUserNameFromToken(token);

            db.AuthTests.Add(test);
            db.SaveChanges();

            var response = Request.CreateResponse<AuthTest>(HttpStatusCode.Created,test);
            //var response = Request.CreateResponse(HttpStatusCode.Created);
            return response;
        }
    }
}
