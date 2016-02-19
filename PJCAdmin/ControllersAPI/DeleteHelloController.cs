using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PJCAdmin.Models;

namespace PJCAdmin.ControllersAPI
{
    public class DeleteHelloController : ApiController
    {
        private pjcEntities db = new pjcEntities();

        // GET api/DeleteHello
        public IEnumerable<Hello> GetHelloes()
        {
            return db.Helloes; //.AsEnumerable();
        }

        // GET api/DeleteHello/5
        // To Delete
        public HttpResponseMessage GetHello(int id)
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

        // PUT api/DeleteHello/5
        public HttpResponseMessage PutHello(int id, Hello hello)
        {
            if (ModelState.IsValid && id == hello.helloID)
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

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/DeleteHello
        // To Update
        public HttpResponseMessage PostHello(Hello hello)
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

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/DeleteHello/5
        public HttpResponseMessage DeleteHello(int id)
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