using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;

namespace PJCAdmin.ControllersAPI
{
    public class TestController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        public string Get()
        {
            return "";
        }
    }
}
