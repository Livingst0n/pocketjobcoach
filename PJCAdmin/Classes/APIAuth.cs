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

        public static Guid getUserIDFromToken(string token)
        {
            if (! isTokenValid(token))
            {
                throw new TokenIsInvalidException();
            }

            int id = Int32.Parse(token.Substring(37));
            AuthToken authToken = db.AuthTokens.Find(id);

            return authToken.UserID;
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

            if (!authToken.Token.Equals(token))
                return false;

            if (isExpired(authToken.ExpirationDate))
                return false;

            return true;
        }

        public static void authorizeToken(string token)
        {
            if (!isTokenValid(token))
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
            }
        }
    }
}