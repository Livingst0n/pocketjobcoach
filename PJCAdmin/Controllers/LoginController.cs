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

        public HttpResponseMessage Post(LoginModel model)
        {

            if (System.Web.Security.Membership.ValidateUser(model.UserName, model.Password))
            {
                Guid userID = db.Users.Where<User>(a => a.UserName.Equals(model.UserName)).FirstOrDefault().UserId;
                AuthToken token;
                try
                {
                    token = db.AuthTokens.Where<AuthToken>(t => t.UserID.Equals(userID)).First();
                    //User already has a token -> update token
                    token.Token = Guid.NewGuid().ToString() + ":" + token.AuthTokenID;
                    token.ExpirationDate = DateTime.Now.AddMinutes(10); //Expires in 10 minutes

                    db.Entry(token).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }
                catch (InvalidOperationException ioe)
                {
                    //Token does not already exist for the user -> create token
                    token = new AuthToken();
                    token.UserID = userID;

                    token.Token = Guid.NewGuid().ToString();
                    token.ExpirationDate = DateTime.Now.AddMinutes(10); //Expires in 10 minutes

                    token = db.AuthTokens.Add(token);
                    db.SaveChanges();

                    token.Token = token.Token + ":" + token.AuthTokenID;

                    db.Entry(token).State = System.Data.EntityState.Modified;
                    db.SaveChanges();
                }

                var response = Request.CreateResponse<string>(HttpStatusCode.OK, token.Token);

                return response;
            }
            else
                return Request.CreateResponse(HttpStatusCode.Forbidden);
        }
    }
}
