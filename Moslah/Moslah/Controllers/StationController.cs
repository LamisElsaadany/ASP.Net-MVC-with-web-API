using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Web.Http;

namespace Moslah.Controllers
{//"Moslah/Station/CreateStation/{st}"
    [RoutePrefix("Moslah/Station")]
    public class StationController : ApiController
    {

        ApplicationDbContext db = new ApplicationDbContext();
        [Route("all")]

        public IHttpActionResult get()
        {
            if (db.Stations.ToList().Count == 0)
                return NotFound();
            return Ok(db.Stations.ToList());
        }
        [Route("CreateStation")]

        public IHttpActionResult Post(Stations st)
        {
            //if (db.Stations.FirstOrDefault(m => m.ID == st.ID) == null)
            //    return NotFound();
          
            db.Stations.Add(st);
            db.SaveChanges();
            return Ok();
        }
        [Route("UpdateStation/{id}/{st}")]

        public IHttpActionResult Put(int id, Stations st)
        {
            var s = db.Stations.FirstOrDefault(a => a.ID == id);
            if (s != null)
            {
                s.ID = st.ID;
                s.Name = st.Name;
                //s.BusNumber = st.BusNumber;
                s.City = st.City;
                //s.MetroNumber = st.MetroNumber;
                //s.MicroID = st.MicroID;
                s.Type = st.Type;
                s.zone = st.zone;
                db.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        [Route("RemoveStation/{id}")]

        public IHttpActionResult delete(int id)
        {
            var st = db.Stations.FirstOrDefault(a => a.ID == id);
            if (st == null)
                return NotFound();
            db.Stations.Remove(st);
            db.SaveChanges();
            return Ok();
        }
    }
}