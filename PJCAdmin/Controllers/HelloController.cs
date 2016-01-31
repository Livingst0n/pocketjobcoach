using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;

namespace PJCAdmin.Controllers
{
    public class HelloController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        // GET api/<controller>
        public IEnumerable<Hello> Get()
        {
            return db.Helloes;
        }

        // GET api/<controller>/5
        public Hello Get(int id)
        {
            return db.Helloes.Find(id);
        }

        // GET api/<controller>?lang=<lang>
        public Hello Get(string lang)
        {
            return db.Helloes.Where(h => h.helloLanguage.Equals(lang)).FirstOrDefault();
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Hello hello)
        {
            
            db.Helloes.Add(hello);
            db.SaveChanges();

            var response = Request.CreateResponse<Hello>(HttpStatusCode.Created, hello);
            
            string uri = Url.Link("DefaultApi", new { id = hello.helloID });
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}