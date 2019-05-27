using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Headers;
using Moslah.Models.OwnModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;

namespace MoslahConsumer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<City> DDl = Cities();
            return View(DDl);
        }
        List<QuickSearch> returnResult = new List<QuickSearch>();
        int ResultCounter = 0;
        public ActionResult GetPath(string Source,string Destination,string city)
        {
            returnResult.Clear();
         
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var respond = Client.GetAsync($"Moslah/Quicksearch/{Source}/{Destination}/{city}").Result;
            if (respond.StatusCode==HttpStatusCode.OK)
            {
                var data = respond.Content.ReadAsStringAsync().Result;
                returnResult = JsonConvert.DeserializeObject<List<QuickSearch>>(data);
            }
            else
            {
                var BusResponse   = Client.GetAsync($"Moslah/Bus/BusRoad/{Source}/{Destination}/{city}").Result;
                var MetroResponse = Client.GetAsync($"Moslah/Metro/MetroRoad/{Source}/{Destination}/{city}").Result;
                var MicroResponse = Client.GetAsync($"Moslah/MicroBus/MicroBusRoad/{Source}/{Destination}/{city}").Result;
                if(BusResponse.StatusCode==HttpStatusCode.OK)
                {
                    var Busdata = BusResponse.Content.ReadAsStringAsync().Result;
                    List<string> finallines = JsonConvert.DeserializeObject<List<string>>(Busdata);
                    AddFinallinestoQS(finallines,Source,Destination, "Bus");
                 }
                if (MetroResponse.StatusCode == HttpStatusCode.OK)
                {
                    var Metrodata = BusResponse.Content.ReadAsStringAsync().Result;
                    List<string> finallines = JsonConvert.DeserializeObject<List<string>>(Metrodata);
                    AddFinallinestoQS(finallines, Source, Destination, "Metro");
                }
                if (MicroResponse.StatusCode == HttpStatusCode.OK)
                {
                    var Microdata = BusResponse.Content.ReadAsStringAsync().Result;
                    List<string> finallines = JsonConvert.DeserializeObject<List<string>>(Microdata);
                    AddFinallinestoQS(finallines, Source, Destination, "MicroBus");
                }
            }
            
            return PartialView(returnResult);
        }
        public void AddFinallinestoQS(List<string> Finallines,string Source,string Destination, string vechialtype)
        {
            for (int i = 0; i < Finallines.Count; i++)
            {
                returnResult[ResultCounter].Source = Source;
                returnResult[ResultCounter].Destination = Destination;
                returnResult[ResultCounter].RoadDesc = Finallines[i];
                returnResult[ResultCounter].VechialType = vechialtype;
            }

        }
        public List<City> Cities()
        {
            HttpClient Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:65046/Moslah/");
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = Client.GetAsync($"City/all").Result;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var Cities = JsonConvert.DeserializeObject<List<City>>(data);
                return Cities;
            }
            return null;
        }
        public ActionResult HomeDesign()
        {
            return PartialView();
        }

        public ActionResult About()
        {
            return PartialView();
        }
        public ActionResult Service()
        {
            return PartialView();
        }

        public ActionResult Contact()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult SendEmail(string Username, string Userphone, string UserMail, string Message)
        {
            MailMessage mailMessage = new MailMessage(UserMail, "tempmailmail6@gmail.com");
            mailMessage.Subject = "Moslah";
            mailMessage.Body = Message;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.Credentials = new NetworkCredential()
            {
                UserName = "tempmailmail6@gmail.com",
                Password = "Temp2014"
            };

            //client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;

            client.EnableSsl = true;

            client.Send(mailMessage);

            return RedirectToAction("Index");
        }
       


        public ActionResult Weather()
        {
            return PartialView();
        }

       
    }
}