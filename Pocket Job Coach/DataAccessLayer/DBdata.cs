using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Pocket_Job_Coach.Models;
using MySql.Data.MySqlClient;

namespace Pocket_Job_Coach.DataAccessLayer
{
    class DBdata
    {
        public string InsertData(AdminData AD)
        {
            MySqlConnection db = null;
            string result = "";
            try
            {
                db = new MySqlConnection("Server=mysql1.gear.host;Port=3306;Database=pjc;Uid=pjc;Pwd=Parcmen!;");
                //var db = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                var selectQueryString = "INSERT INTO temp (firstName, lastName) value (@FirstName, @LastName) set firstName=@FirstName, lastName=@LastName";

                MySqlCommand cmd = new MySqlCommand(selectQueryString, db);
                cmd.CommandType = CommandType.Text;

                // i will pass zero to MobileID beacause its Primary .
                cmd.Parameters.AddWithValue("@FirstName", AD.firstName);
                cmd.Parameters.AddWithValue("@LastName", AD.lastName);
                db.Open();
                result = cmd.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                db.Close();
            }
        }

        public string UpdateData(AdminData AD)
        {
            MySqlConnection db = null;
            string result = "";
            try
            {
                db = new MySqlConnection("Server=mysql1.gear.host;Port=3306;Database=pjc;Uid=pjc;Pwd=Parcmen!;");
                //var db = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                var selectQueryString = "INSERT INTO temp (firstName, lastName) value (@FirstName, @LastName) set firstName=@FirstName, lastName=@LastName";

                MySqlCommand cmd = new MySqlCommand(selectQueryString, db);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@FirstName", AD.firstName);
                cmd.Parameters.AddWithValue("@LastName", AD.lastName);
                db.Open();
                result = cmd.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                db.Close();
            }
        }

        public string DeleteData(AdminData AD)
        {
            MySqlConnection db = null;
            string result = "";
            try
            {
                db = new MySqlConnection("Server=mysql1.gear.host;Port=3306;Database=pjc;Uid=pjc;Pwd=Parcmen!;");
                //var db = new MySqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                var selectQueryString = "INSERT INTO temp (firstName, lastName) value (@FirstName, @LastName) set firstName=@FirstName, lastName=@LastName";

                
                MySqlCommand cmd = new MySqlCommand(selectQueryString, db);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FirstName", AD.firstName);
                cmd.Parameters.AddWithValue("@LastName", AD.lastName);

                db.Open();

                result = cmd.ExecuteScalar().ToString();
                return result;
            }
            catch
            {
                return result = "";
            }
            finally
            {
                db.Close();
            }
        }

    }
}
