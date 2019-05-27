using Moslah.Models.OwnModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MoslahConsumer.Controllers
{
    //[Authorize]

    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllNews()
        {
            List<News> ListOfNews = new List<News>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resp = client.GetAsync($"Moslah/RoadNews").Result;
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var msg = resp.Content.ReadAsStringAsync().Result;
                List<News> AllNews = JsonConvert.DeserializeObject<List<News>>(msg);
                ListOfNews = AllNews;
            }
            return PartialView(ListOfNews);
        }


        public ActionResult SearchNews(string name)
        {
            if ((name != "") && (name != null))
            {
                List<News> ListOfDescription = new List<News>();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:65046/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resp = client.GetAsync($"Moslah/RoadNews/Name/{name}").Result;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var msg = resp.Content.ReadAsStringAsync().Result;
                    ListOfDescription = JsonConvert.DeserializeObject<List<News>>(msg);
                }
                return PartialView("GetAllNews", ListOfDescription);

            }
            else
                return RedirectToAction("GetAllNews");
        }


        public ActionResult DeleteNew(int ID)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            var resp = client.DeleteAsync($"Moslah/RoadNews/RemoveNews/{ID}").Result;
            if (resp.IsSuccessStatusCode)
                return RedirectToAction("GetAllNews");
            return View();
        }

        public ActionResult NewNews()
        {
            return PartialView(new News());
        }

        [HttpPost]
        public ActionResult NewNews(News news,string CityName)
        {
            City city = new City() { CityName = CityName };

            int cityID = CheckPostCity(city);
            news.cityID = cityID;
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/RoadNews/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PostAsJsonAsync<News>("CreateNews", news).Result;
            return RedirectToAction("GetAllNews");


        }

        public ActionResult UpdateNews(int ID)
        {
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/RoadNews/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.GetAsync($"ID/{ID}").Result;
            News NewsByID = new News();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var msg = result.Content.ReadAsStringAsync().Result;
                NewsByID = JsonConvert.DeserializeObject<News>(msg);

            }
           ViewBag.Cityname=GetCityName(NewsByID.cityID);

            return PartialView(NewsByID);

        }

        [HttpPost]
        public ActionResult UpdateNews(int ID,string CityName, News news)
        {
            City city = new City() { CityName = CityName };
            int cityID = CheckPostCity(city);
            news.cityID = cityID;
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/RoadNews/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PutAsJsonAsync<News>($"UpdateNews/{news.ID}", news).Result;
            return RedirectToAction("GetAllNews");

        }

        public int CheckPostCity(City City)
        {
            City.CityCode = 4;
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PostAsJsonAsync<City>("City/CreateCity", City).Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var CityCode = JsonConvert.DeserializeObject<int>(data);
                return CityCode;
            }
            return -1;
        }

        public string GetCityName(int id)
        {
          
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.GetAsync($"City/CityName/{id}").Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var CityName = JsonConvert.DeserializeObject<string>(data);
                return CityName;
            }
            return "";
        }
    }
}