using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MoslahConsumer.Controllers
{//"Moslah/Station/CreateStation/{st}"
    public class StationController : Controller
    {
        // GET: Station
        public ActionResult Index()
        {
            return View();
        }
        
        public void PostStation(Stations station)
        {
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/Station/CreateStation");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PostAsJsonAsync<Stations>("", station).Result;
           // return RedirectToAction("GetAllNews");


        }
    }
}