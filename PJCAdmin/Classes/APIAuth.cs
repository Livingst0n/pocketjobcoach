using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using PJCAdmin.Models;

namespace PJCAdmin.Classes
{
    public class APIAuth
    {
        private static pjcEntities db = new pjcEntities();

        public static string getUserNameFromToken(string token)
        {
            if (! isTokenValid(token))
            {
                throw new TokenIsInvalidException();
            }

            int id = Int32.Parse(token.Substring(37));
            AuthToken authToken = db.AuthTokens.Find(id);

            return authToken.userName;
        }

        private static bool isExpired(DateTime expirationDate)
        {
            if (expirationDate.CompareTo(DateTime.Now) < 0)
                return true;
            else
                return false;
        }

        public static bool isTokenValid(string token)
        {
            if (token.Length < 38)
                return false;

            string guid = token.Substring(0, 36);
            int id = Int32.Parse(token.Substring(37));

            AuthToken authToken = db.AuthTokens.Find(id);

            if (authToken == null)
                return false;

            if (!authToken.token.Equals(token))
            {
                db.Entry(authToken).Reload();

                if (!authToken.token.Equals(token))
                    return false;
            }

            if (isExpired(authToken.expirationDate))
                return false;

            return true;
        }

        public static void authorizeToken(string token)
        {
            if (!isTokenValid(token))
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
            }

            int id = Int32.Parse(token.Substring(37));
            AuthToken authToken = db.AuthTokens.Find(id);

            //update expiration, 10 minutes of inactivity
            authToken.expirationDate = DateTime.Now.AddMinutes(10);

            db.Entry(authToken).State = System.Data.EntityState.Modified;
            db.SaveChanges();

        }

        public void dispose()
        {
            db.Dispose();
        }
    }
}