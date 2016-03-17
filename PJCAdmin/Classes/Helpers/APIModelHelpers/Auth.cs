using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using PJCAdmin.Models;

namespace PJCAdmin.Classes.Helpers.APIModelHelpers
{
    /* --------------------------------------------------------
     * The APIAuth class provides common methods relating 
     * to Authentication for the WebAPI service.
     * --------------------------------------------------------
     */
    public class Auth
    {
        private static DbHelper helper = new DbHelper();

        /* Returns the username for the user associated
         * with the given token.
         * @param token: The token retrieved from the 
         * server that was returned to the server with
         * an HTTP request.
         * @throws TokenIsInvalidException: if isTokenValid
         * returns false for the token.
         */
        public static string getUserNameFromToken(string token)
        {
            if (! isTokenValid(token))
            {
                throw new TokenIsInvalidException();
            }

            int id = Int32.Parse(token.Substring(37));
            AuthToken authToken = helper.findAuthToken(id);

            return authToken.userName;
        }
        /* Returns whether or not the given token is valid.
         * A token is considered valid if it exists in an
         * AuthToken record within the database and if
         * isExpired returns false for the AuthToken's 
         * expiration date.
         * @param token: The token retrieved from the
         * server that was returned to the server with
         * an HTTP request.
         */
        public static bool isTokenValid(string token)
        {
            if (token.Length < 38)
                return false;

            string guid = token.Substring(0, 36);
            int id = Int32.Parse(token.Substring(37));

            AuthToken authToken = helper.findAuthToken(id);

            if (authToken == null)
                return false;

            if (!authToken.token.Equals(token))
            {
                authToken = helper.reloadToken(authToken);

                if (!authToken.token.Equals(token))
                    return false;
            }

            if (isExpired(authToken.expirationDate))
                return false;

            return true;
        }
        /* Authorizes the given token and updates the token's
         * expiration date.
         * @param token: The token retrieved from the 
         * server that was returned to the server with
         * an HTTP request.
         * @throws HTTPStatusCode.Unauthorized: if 
         * isTokenValid returns false. This causes 
         * an HTTP Response to be sent to the client.
         */
        public static void authorizeToken(string token)
        {
            if (!isTokenValid(token))
            {
                throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized));
            }

            int id = Int32.Parse(token.Substring(37));
            AuthToken authToken = helper.findAuthToken(id);

            if (isExpired(authToken.expirationDate.AddMinutes(-10)))
            {
                //update expiration, 10 minutes of inactivity
                authToken.expirationDate = DateTime.Now.AddMinutes(10);
            }
            else
            {
                authToken.expirationDate = DateTime.Now.AddYears(50);
            }

            helper.updateToken(authToken);

        }
        /* Returns whether or not the given expiration date
         * has passed.
         * @param expirationDate: The datetime to be checked.
         */
        private static bool isExpired(DateTime expirationDate)
        {
            if (expirationDate.CompareTo(DateTime.Now) < 0)
                return true;
            else
                return false;
        }
        
        public void dispose()
        {
            helper.dispose();
        }
    }
}