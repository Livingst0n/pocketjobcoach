using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PJCAdmin.Models;

namespace PJCAdmin.ControllersAPI
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

        // PUT
        // POST api/Hello?put=true
        [HttpPost]
        public HttpResponseMessage Put(string put, Hello hello)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hello).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse<Hello>(HttpStatusCode.OK,hello);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE
        // GET api/Hello/<id>?delete=true
        [HttpGet]
        public HttpResponseMessage Delete(string delete, int id)
        {
            Hello hello = db.Helloes.Find(id);
            if (hello == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Helloes.Remove(hello);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, hello);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}