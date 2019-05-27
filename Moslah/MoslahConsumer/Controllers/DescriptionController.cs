using Moslah.Models;
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

    public class DescriptionController : Controller
    {
        // GET: Description
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllDescription()
        {
            List<Description> ListOfDescription = new List<Description>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var resp = client.GetAsync($"Moslah/RoadDescription").Result;
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var msg = resp.Content.ReadAsStringAsync().Result;
                List<Description> AllNews = JsonConvert.DeserializeObject<List<Description>>(msg);
                ListOfDescription = AllNews;
            }
            return PartialView(ListOfDescription);
        }

        public ActionResult SearchDescription(string name)
        {
            if ((name!="") && (name != null))
            {
                List<Description> ListOfDescription = new List<Description>();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:65046/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resp = client.GetAsync($"Moslah/RoadDescription/Name/{name}").Result;
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var msg = resp.Content.ReadAsStringAsync().Result;
                    ListOfDescription = JsonConvert.DeserializeObject<List<Description>>(msg);
                }
                return PartialView("GetAllDescription", ListOfDescription);

            }
            else
                return RedirectToAction("GetAllDescription");
        }


        public ActionResult DeleteDescription(int ID)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            var resp = client.DeleteAsync($"Moslah/RoadDescription/RemoveDescription/{ID}").Result;
            if (resp.IsSuccessStatusCode)
                return RedirectToAction("GetAllDescription");
            return View();
        }

        public ActionResult NewDescription()
        {
            return PartialView(new Description());
        }

        [HttpPost]
        public ActionResult NewDescription(Description description,string CityName)
        {
           
            City city = new City() { CityName = CityName };

            int cityID = CheckPostCity(city);
            description.cityID = cityID;
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/RoadDescription/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PostAsJsonAsync<Description>("CreateDescription", description).Result;
            return RedirectToAction("GetAllDescription");

        }

        public ActionResult UpdateDescription(int ID)
        {

            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/RoadDescription/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.GetAsync($"ID/{ID}").Result;
            Description DescByID = new Description();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var msg = result.Content.ReadAsStringAsync().Result;
                DescByID = JsonConvert.DeserializeObject<Description>(msg);

            }
            ViewBag.Cityname = GetCityName(DescByID.cityID);

            return PartialView(DescByID);

        }

        [HttpPost]
        public ActionResult UpdateDescription(int ID,string CityName, Description Desc)
        {

            City city = new City() { CityName = CityName };
            int cityID = CheckPostCity(city);
            Desc.cityID = cityID;
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/RoadDescription/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PutAsJsonAsync<Description>($"UpdateDescription/{Desc.ID}", Desc).Result;
            return RedirectToAction("GetAllDescription");

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