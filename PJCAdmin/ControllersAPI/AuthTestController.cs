using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;
using PJCAdmin.Classes;

namespace PJCAdmin.ControllersAPI
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

        // POST api/AuthTest/put?token=<token>&id=<id>
        [HttpPost]
        public HttpResponseMessage Put(bool put, string token, int id, AuthTest test)
        {
            return new HttpResponseMessage();
        }

        // GET api/AuthTest/delete?token=<token>&id=<id>
        [HttpGet]
        public HttpResponseMessage Delete(bool get, string token, int id)
        {
            return new HttpResponseMessage();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
