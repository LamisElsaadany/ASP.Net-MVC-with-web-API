using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/RoadDescription")]
    public class PostDescriptionController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        [Route("")]
        public IHttpActionResult GetAllDescription()
        {
            List<Description> Result = DB.Description.Where(o => o.Deleted == false).OrderByDescending(o => o.Date).ToList();
            if (Result.Count == 0)
                return NotFound();
            return Ok(Result);
        }

        [Route("ID/{ID}")]
        public IHttpActionResult GetAllDescription(int ID)
        {
            Description Result = DB.Description.FirstOrDefault(m => m.Deleted == false && m.ID == ID);
            if (Result == null)
                return NotFound();
            return Ok(Result);
        }

        [Route("Name/{name}")]
        public IHttpActionResult GetDescriptionByName(string name)
        {
           List<Description> Result = DB.Description.Where(m => m.Deleted == false && (m.Source.ToLower().Contains(name.ToLower())||m.Destination.ToLower().Contains(name.ToLower()))).OrderByDescending(o=>o.Date).ToList();
            if (Result == null)
                return NotFound();
            return Ok(Result);
        }

        [Route("{Source}/{Destination}")]

        public IHttpActionResult GetRelatedDescription(string Source, string Destination)
        {
           List<Description> Result = DB.Description.Where(o => o.Source == Source && o.Destination == Destination&& o.Deleted == false).OrderByDescending(o => o.Date).ToList();

            if (Result.Count == 0)
                return NotFound();
            return Ok(Result);
        }
        [Route("CreateDescription")]

        public void PostDescription(Description NewDescrip)
        {
            DB.Description.Add(NewDescrip);
            DB.SaveChanges();
        }

        [Route("UpdateDescription/{ID}")]
        public IHttpActionResult PutDescription(int ID, Description NewDescription)
        {
            var Old = DB.Description.FirstOrDefault(o => o.ID == ID);
            if (Old != null) {
                Old.Destination = NewDescription.Destination;
                Old.Source = NewDescription.Source;
                Old.StatusRoad = NewDescription.StatusRoad;
                Old.cityID = NewDescription.cityID;

                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [Route("RemoveDescription/{ID}")]
        public IHttpActionResult DeleteDescription(int ID)
        {
            var Description = DB.Description.FirstOrDefault(o => o.ID == ID);
            if (Description != null)
            {
                Description.Deleted = true;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

    }
}

