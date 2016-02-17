using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;

namespace PJCAdmin.Controllers
{
    public class LoginController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        //POST .../api/Login
        public HttpResponseMessage Post(LoginModel model)
        {
            if (System.Web.Security.Membership.ValidateUser(model.UserName, model.Password))
            {
                string userName = db.UserNames.Where<UserName>(a => a.userName1.Equals(model.UserName)).FirstOrDefault().userName1;
               
                AuthToken token;
                try
                {
                    token = db.AuthTokens.Where<AuthToken>(t => t.userName.Equals(userName)).First();
                    //User already has a token -> update token
                    token.token = Guid.NewGuid().ToString() + ":" + token.authTokenID;
                    token.expirationDate = DateTime.Now.AddMinutes(10); //Expires in 10 minutes

                    db.Entry(token).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (InvalidOperationException ioe)
                {
                    //Token does not already exist for the user -> create token
                    token = new AuthToken();
                    token.userName = userName;

                    token.token = Guid.NewGuid().ToString();
                    token.expirationDate = DateTime.Now.AddMinutes(10); //Expires in 10 minutes

                    token = db.AuthTokens.Add(token);
                    db.SaveChanges();

                    token.token = token.token + ":" + token.authTokenID;

                    db.Entry(token).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }

                var response = Request.CreateResponse<string>(HttpStatusCode.OK, token.token);

                return response;
            }
            else
                return Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}
