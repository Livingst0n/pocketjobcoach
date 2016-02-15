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
                IQueryable<UserName> matchingUserNames = db.UserNames.Where<UserName>(a => a.userName1.Equals(model.UserName));

                if (matchingUserNames.Count() == 0)
                {
                    //This is user's first Web API login; create API record
                    UserName userNameRecord = new UserName() { userName1 = model.UserName };
                    userNameRecord.userID = (Guid)System.Web.Security.Membership.FindUsersByName(model.UserName).Cast<System.Web.Security.MembershipUser>().FirstOrDefault().ProviderUserKey;

                    db.UserNames.Add(userNameRecord);
                    db.SaveChanges();
                }

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
