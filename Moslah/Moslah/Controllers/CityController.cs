using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Web.Http;

namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/City")]
    public class CityController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Route("all")]
        public IHttpActionResult get()
        {
            List<City> Cities = db.Cities.ToList();
            if (Cities.Count == 0)
                return NotFound();
            return Ok(Cities);
        }

        [Route("CityName/{C}")]
        public IHttpActionResult get(int C)
        {
            City City = db.Cities.FirstOrDefault(o=>o.CityCode==C);
            if (City==null)
                return NotFound();
            return Ok(City.CityName);
        }
        [Route("Cityid/{N}")]
        public IHttpActionResult get(string N)
        {
            City City = db.Cities.FirstOrDefault(o => o.CityName == N);
            if (City == null)
                return NotFound();
            return Ok(City.CityCode);
        }

        [Route("CreateCity")]
        public IHttpActionResult Post(City cy)
        {
            var res = db.Cities.FirstOrDefault(m => m.CityName == cy.CityName);
            if (res == null)
            {
                db.Cities.Add(cy);
                db.SaveChanges();
                return Ok(db.Cities.FirstOrDefault(m => m.CityName == cy.CityName).CityCode);
            }
            
            return Ok(res.CityCode);
            

         

        }
        [Route("UpdateCity/{CityCode}")]
        public IHttpActionResult Put(int CityCode, City cy)
        {
            var s = db.Cities.FirstOrDefault(a => a.CityCode == CityCode);

            if (s != null)
            {
                s.CityName = cy.CityName;
                s.Stations = cy.Stations;
                db.SaveChanges();
                return Ok();
            }
            else
                return NotFound();
        }
        [Route("RemoveCity/{CityCode}")]

        public IHttpActionResult delete(int CityCode)
        {
            var st = db.Cities.FirstOrDefault(a => a.CityCode == CityCode);
            if (st == null)
                return NotFound();
            db.Cities.Remove(st);
            db.SaveChanges();
            return Ok();
        }
    }
}