using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace Pocket_Job_Coach.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            string format = "h:mm tt MMM d yyyy";
            ViewBag.date = date.ToString(format);

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Admin()
        {
            ViewBag.Message = "Your Admin page.";
            var db = new MySqlConnection("Server=mysql1.gear.host;Port=3306;Database=pjc;Uid=pjc;Pwd=Parcmen!;");
            var selectQueryString = "SELECT * FROM temp ORDER BY firstname";
            db.Open();
            MySqlCommand myCommand = new MySqlCommand(selectQueryString);
            myCommand.Connection = db;
            MySqlDataAdapter myAdapter = new MySqlDataAdapter();
            myAdapter.SelectCommand = myCommand;
            DataTable myData = new DataTable();
            myAdapter.Fill(myData);
            db.Close();

            ViewData.Model = myData.AsEnumerable();

            return View();
        }
    }
}
