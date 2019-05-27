using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MoslahConsumer.Controllers
{
    public class autoCompleteController : Controller
    {
        #region FromTo
        public JsonResult GetSources(string term)
        {
            List<string> ListOfSources = new List<string>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/SourceNames/{term}").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            ListOfSources = JsonConvert.DeserializeObject<List<string>>(data);
            return Json(ListOfSources, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDestination(string term)
        {
            List<string> ListOfDestinations = new List<string>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/DestinationNames/{term}").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            ListOfDestinations = JsonConvert.DeserializeObject<List<string>>(data);
            return Json(ListOfDestinations, JsonRequestBehavior.AllowGet);
        }
        #endregion
    
    }
}