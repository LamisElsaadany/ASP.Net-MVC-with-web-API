using Moslah.Models;
using Moslah.Models.OwnModels;
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
    //[Authorize(Roles = "admin")]

    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        #region Bus
        [HttpGet]
        public ActionResult AddBus()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddBus(int _Busnumber, string busLines, string CityName, string zone,string Name, int _price)
        {
            City city = new City() { CityName = CityName };

            int cityID = CheckPostCity(city);

            Stations stations = new Stations() { Type = "Bus", Name = Name, zone = zone, cityID = cityID };
            

            int stationId=CheckPostStation(stations);

            string[] _Lines = busLines.Split(',');
            for (int i = 0; i < _Lines.Length - 1; i++)
            {
                BusLocation bus = new BusLocation();
                bus.BusNumber = _Busnumber;
                bus.PriceBus = _price;
                bus.Source = _Lines[i];
                bus.Destination = _Lines[i + 1];

                bus.stationId = stationId;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:65046/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respond = client.PostAsJsonAsync<BusLocation>($"Moslah/Bus/CreateBus", bus).Result;
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult EditBus()
        {
            List<int> BusIDs = new List<int>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/Bus/allBusNumbers").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            BusIDs = JsonConvert.DeserializeObject<List<int>>(data);
            ViewBag.Nos = BusIDs;

            return PartialView();
        }
        [HttpPost]
        public ActionResult EditBus(int BusNumber, string busLines,string CityName, int _price, string zone, string Name)
        {
            City city = new City() { CityName = CityName };

            int cityID = CheckPostCity(city);

            Stations stations = new Stations() { Type = "Bus", Name = Name, zone = zone, cityID = cityID };


            int stationId = CheckPostStation(stations);
            int busNo = BusNumber;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = client.DeleteAsync($"Moslah/Bus/RemoveBus/{busNo}").Result;

            string[] _Lines = busLines.Split(',');
            for (int i = 0; i < _Lines.Length - 1; i++)
            {
                BusLocation bus = new BusLocation();
                bus.BusNumber = BusNumber;
                bus.PriceBus = _price;
                bus.Source = _Lines[i];
                bus.Destination = _Lines[i + 1];
                bus.stationId = stationId;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respond1 = client.PostAsJsonAsync<BusLocation>($"Moslah/Bus/CreateBus", bus).Result;
            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult DeleteBus()
        {

            List<int> BusIDs = new List<int>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/Bus/allBusNumbers").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            BusIDs = JsonConvert.DeserializeObject<List<int>>(data);
            ViewBag.Nos = BusIDs;
            return PartialView();
        }
        [HttpPost]
        public ActionResult DeleteBus(int BusNumber)
        {
            int busNo = BusNumber;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = client.DeleteAsync($"Moslah/Bus/RemoveBus/{busNo}").Result;
            return View("Index");
        }
        #endregion

        #region Microbus
        [HttpGet]
        public ActionResult AddMicrobus()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddMicrobus(int _MicroBusnumber ,string MicrobusLines, string zone, string Name,string CityName ,int _Microprice)
        {


            City city = new City() { CityName = CityName };

            int cityID = CheckPostCity(city);
            Stations stations = new Stations() { Type = "MicroBus", Name = Name, zone = zone,cityID=cityID };

            int stationId = CheckPostStation(stations);
            string[] _Lines = MicrobusLines.Split(',');
            for (int i = 0; i < _Lines.Length - 1; i++)
            {
                MicroBus Microbus = new MicroBus();
                Microbus.MicroID = _MicroBusnumber;
                Microbus.PriceMicro = _Microprice;
                Microbus.Source = _Lines[i];
                Microbus.Destination = _Lines[i + 1];
                Microbus.stationId = stationId;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:65046/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respond = client.PostAsJsonAsync<MicroBus>($"Moslah/MicroBus/CreateMicroBus", Microbus).Result;
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult EditMicrobus()
        {
            List<int> microIDs = new List<int>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/MicroBus/allMicroNumbers").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            microIDs = JsonConvert.DeserializeObject<List<int>>(data);
            ViewBag.Nos = microIDs;

            return PartialView();
        }
        [HttpPost]
        public ActionResult EditMicrobus(int MicroID, string MicrobusLines,string CityName, int _Microprice,string Name,string zone)
        {

            City city = new City() { CityName = CityName };

            int cityID = CheckPostCity(city);
            Stations stations = new Stations() { Type = "MicroBus", Name = Name, zone = zone, cityID = cityID };


            int stationId = CheckPostStation(stations);
            int MicroNo = MicroID;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = client.DeleteAsync($"Moslah/MicroBus/RemoveMicroBus/{MicroNo}").Result;

            string[] _Lines = MicrobusLines.Split(',');
            for (int i = 0; i < _Lines.Length - 1; i++)
            {
                MicroBus Microbus = new MicroBus();
                Microbus.MicroID = MicroID;
                Microbus.PriceMicro = _Microprice;
                Microbus.Source = _Lines[i];
                Microbus.Destination = _Lines[i + 1];
                Microbus.stationId = stationId;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respond1 = client.PostAsJsonAsync<MicroBus>($"Moslah/MicroBus/CreateMicroBus", Microbus).Result;

            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult DeleteMicrobus()
        {
            List<int> microIDs = new List<int>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/MicroBus/allMicroNumbers").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            microIDs = JsonConvert.DeserializeObject<List<int>>(data);
            ViewBag.Nos = microIDs;

            return PartialView();
        }
        [HttpPost]
        public ActionResult DeleteMicrobus(int MicroID)
        {
            int MicroNo = MicroID;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = client.DeleteAsync($"Moslah/MicroBus/RemoveMicroBus/{MicroNo}").Result;
            return View("Index");
        }
        #endregion

        #region Metro
        [HttpGet]
        public ActionResult AddMetro()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult AddMetro(int _Busnumber, string busLines, string CityName, string zone, string Name)
        {
            City city = new City() { CityName = CityName };
            int cityID = CheckPostCity(city);
            Stations stations = new Stations() { Type = "Metro", Name = Name, zone = zone, cityID = cityID };
            int stationId = CheckPostStation(stations);
            string[] _Lines = busLines.Split(',');
            for (int i = 0; i < _Lines.Length - 1; i++)
            {
                MetroLocation metro = new MetroLocation();
                metro.MetroNumber = _Busnumber;
                metro.Source = _Lines[i];
                metro.Destination = _Lines[i + 1];
                metro.stationId = stationId;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:65046/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respond = client.PostAsJsonAsync<MetroLocation>($"Moslah/Metro/CreateMetro", metro).Result;
            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult EditMetro()
        {
            List<int> metroIDs = new List<int>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/Metro/allMetroNumbers").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            metroIDs = JsonConvert.DeserializeObject<List<int>>(data);
            ViewBag.Nos = metroIDs;

            return PartialView();
        }
        [HttpPost]
        public ActionResult EditMetro(int MetroNumber, string busLines,string CityName, string Name,string zone)
        {
            City city = new City() { CityName = CityName };
            int cityID = CheckPostCity(city);
            Stations stations = new Stations() { Type = "Metro", Name = Name, zone = zone, cityID = cityID };


            int stationId = CheckPostStation(stations);
            int metroNo = MetroNumber;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = client.DeleteAsync($"Moslah/Metro/RemoveMetro/{metroNo}").Result;

            string[] _Lines = busLines.Split(',');
            for (int i = 0; i < _Lines.Length - 1; i++)
            {
                MetroLocation metro = new MetroLocation();
                metro.MetroNumber = MetroNumber;
                metro.Source = _Lines[i];
                metro.Destination = _Lines[i + 1];
                metro.stationId = stationId;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var respond2 = client.PostAsJsonAsync<MetroLocation>($"Moslah/Metro/CreateMetro", metro).Result;

            }
            return View("Index");
        }
        [HttpGet]
        public ActionResult DeleteMetro()
        {
            List<int> metroIDs = new List<int>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var resp = client.GetAsync($"Moslah/Metro/allMetroNumbers").Result;
            var data = resp.Content.ReadAsStringAsync().Result;
            metroIDs = JsonConvert.DeserializeObject<List<int>>(data);
            ViewBag.Nos = metroIDs;

            return PartialView();
        }
        [HttpPost]
        public ActionResult DeleteMetro(int MetroNumber)
        {
            int metroNo = MetroNumber;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65046/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = client.DeleteAsync($"Moslah/Metro/RemoveMetro/{metroNo}").Result;
            return View("Index");
        }
        #endregion
        #region City&Station
        public int CheckPostStation(Stations station)
        {
         
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result1 = Client.GetAsync("Station/all").Result;
            if (result1.IsSuccessStatusCode)
            {
                var data = result1.Content.ReadAsStringAsync().Result;
                var lst = JsonConvert.DeserializeObject<List<Stations>>(data);
                    var obj = lst.FirstOrDefault(m => m.Name == station.Name && m.zone == station.zone && m.Type == station.Type);
                    if (obj != null)
                        return obj.ID;
                

            }

             var result = Client.PostAsJsonAsync<Stations>("Station/CreateStation", station).Result;
             var result3 = Client.GetAsync("Station/all").Result;
             if (result3.IsSuccessStatusCode)
                {
                    var data2 = result3.Content.ReadAsStringAsync().Result;
                    var lst2 = JsonConvert.DeserializeObject<List<Stations>>(data2);
                    var obj2 = lst2.FirstOrDefault(m => m.Name == station.Name && m.zone == station.zone && m.Type == station.Type);
                    return obj2.ID;
                }
            return -1;
        

        }


        public int CheckPostCity(City City)
        {
            City.CityCode = 4;
             HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.PostAsJsonAsync<City>("City/CreateCity", City).Result;
            if (result.StatusCode==System.Net.HttpStatusCode.OK)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var CityCode = JsonConvert.DeserializeObject<int>(data);
                return CityCode;
            }
            return -1;
        }



        #endregion
    }


}