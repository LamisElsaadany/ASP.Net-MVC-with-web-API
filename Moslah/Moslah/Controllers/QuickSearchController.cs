using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Moslah.Controllers
{
    public class QuickSearchController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        [Route("Moslah/Quicksearch/{Source}/{Destination}/{Name}")]
        public IHttpActionResult GetByQuickSearch(string Source, string Destination,string Name)
        {
            List<QuickSearch> list= db.Quicksearch.Where(m =>m.City.CityName== Name && m.Source == Source && m.Destination == Destination).ToList();
            if (list.Count != 0)
                return Ok(list);
            return NotFound();
        }
        [Route("Moslah/DestinationNames/{letter}")]
        public List<string> GetAllDestinations(string letter)
        {
            List<string> res = db.BusLocations.Where(o => o.Destination.StartsWith(letter)).Select(o => o.Destination).ToList();
            res.AddRange(db.MetroLocations.Where(o => o.Destination.StartsWith(letter)).Select(o => o.Destination).ToList());
            res.AddRange(db.Microbus.Where(o => o.Destination.StartsWith(letter)).Select(o => o.Destination).ToList());

            return res.Distinct().ToList();
        }
        [Route("Moslah/SourceNames/{letter}")]
        public List<string> GetAllSources(string letter)
        {
            List<string> res = db.BusLocations.Where(o => o.Source.StartsWith(letter)).Select(o => o.Source).ToList();
            res.AddRange(db.MetroLocations.Where(o => o.Source.StartsWith(letter)).Select(o => o.Source).ToList());
            res.AddRange(db.Microbus.Where(o => o.Source.StartsWith(letter)).Select(o => o.Source).ToList());

            return res.Distinct().ToList();
        }
    }
}