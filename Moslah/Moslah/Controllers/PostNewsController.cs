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
    [RoutePrefix("Moslah/RoadNews")]
    public class PostNewsController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        [Route("")]
        public IHttpActionResult GetAllNews()
        {
            List<News> Result = DB.News.Where(m => m.Deleted == false).OrderByDescending(o => o.Date).ToList();
            if (Result.Count == 0)
                return NotFound();
            return Ok(Result);
        }
        [Route("ID/{ID}")]
        public IHttpActionResult GetAllNews(int ID)
        {
            News Result = DB.News.FirstOrDefault(m => m.Deleted == false && m.ID == ID);
            if (Result == null)
                return NotFound();
            return Ok(Result);
        }

        [Route("Name/{name}")]
        public IHttpActionResult GetNewsByName(string name)
        {
            List<News> Result = DB.News.Where(m => m.Deleted == false && (m.Source.ToLower().Contains(name.ToLower()) || m.Destination.ToLower().Contains(name.ToLower()))).OrderByDescending(o => o.Date).ToList();
            if (Result == null)
                return NotFound();
            return Ok(Result);
        }

        [Route("{Source}/{Destination}")]

        public IHttpActionResult GetRelatedNews(string Source,string Destination)
        {
            List<News> Result = DB.News.Where(o => o.Source == Source && o.Destination == Destination && o.Deleted == false).OrderByDescending(o => o.Date).ToList();
            if (Result.Count == 0)
                return NotFound();
            return Ok(Result);
        }
        [Route("CreateNews")]

        public void PostNews(News n)
        {
            DB.News.Add(n);
            DB.SaveChanges();
        }
        [Route("UpdateNews/{ID}")]

        public IHttpActionResult PutNews(int ID,News NewNews)
        {
          var Old=DB.News.FirstOrDefault(o=>o.ID==ID);
            if (Old != null)
            {
                Old.Destination = NewNews.Destination;
                Old.Source = NewNews.Source;
                Old.StatusRoad = NewNews.StatusRoad;
                Old.cityID = NewNews.cityID;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        [Route("RemoveNews/{ID}")]
        public IHttpActionResult DeleteNews(int ID)
        {
            var news = DB.News.FirstOrDefault(o => o.ID == ID);
            if (news != null)
            {
                news.Deleted = true;
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
            
        }

    }
}
