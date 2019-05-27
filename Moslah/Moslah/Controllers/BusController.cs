using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/Bus")]
    public class BusController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        Dictionary<int, List<string>> BusStaions = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> SourceLines = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> DestLines = new Dictionary<int, List<string>>();
        List<string> finalLines = new List<string>();
        List<BusLocation> ListOfBuses = new List<BusLocation>();

        [Route("all")]
        public List<BusLocation> Getall()
        {
            ListOfBuses = DB.BusLocations.ToList();
            return ListOfBuses;
        }

        [Route("BusNumbers/{no}")]
        public List<int> GetBusNo(int no)
        {
            return DB.BusLocations.Where(o => o.BusNumber.ToString().StartsWith(no.ToString())).Select(o => o.BusNumber).ToList();
        }
        [Route("allBusNumbers")]
        public List<int> GetAllBusNo()
        {
            return DB.BusLocations.Select(o=>o.BusNumber).Distinct().ToList();
        }


        [Route()]
        [Route("CityBus/{city}")]
        // GET: Bus
        public IHttpActionResult GetBuses([FromUri]string city)
        {
            ListOfBuses = DB.BusLocations.Where(m=>m.Stations.City.CityName==city).ToList();
            if(ListOfBuses.Count==0)
            {
                return NotFound();
            }
            else
            {
                return Ok(ListOfBuses);
            }
        }

        public List<BusLocation> GetBusesByDestination(string destination, string city)
        {
            GetBuses(city);
            List<BusLocation> buses = ListOfBuses.Where(s => s.Destination == destination).ToList();
            return buses;
        }

        public List<BusLocation> GetBusesBySource(string Source,string city)
        {
            GetBuses(city);

            List<BusLocation> buses = ListOfBuses.Where(s => s.Source == Source).ToList();
            return buses;
        }

        [Route("BusRoad/{Source}/{Destination}/{city}")]
        public IHttpActionResult GetBusRoad(string Source, string Destination, string city)
        {
            City ncity = DB.Cities.FirstOrDefault(o => o.CityName == city);
            GetBuses(city);
            ReformateDB();
            SrcDestLines(Source, Destination);
            FindtheRoad();
            for (int i = 0; i < finalLines.Count; i++)
            {
                QuickSearch quickSearch = new QuickSearch();
                quickSearch.Destination = Destination;
                quickSearch.Source = Source;
                quickSearch.RoadDesc = finalLines[i];
                quickSearch.VechialType = "Bus";
                quickSearch.City= ncity;
                DB.Quicksearch.Add(quickSearch);
                DB.SaveChanges();
            }
            if (finalLines.Count == 0)
            {
                return NotFound();
            }

            return Ok(finalLines);

        }
        //This Function is for reformate db and return dictionary with key equal bus number
        //Values equal the stations of the bus
        public  Dictionary<int, List<string>> ReformateDB()
        {
            int busnumOLd = ListOfBuses[0].BusNumber;
            for (int i = 0; i < ListOfBuses.Count; i++)
            {
                int CurrBusNum = ListOfBuses[i].BusNumber;
                //If It has the Same bus number of the previuse so it will be a station
                //of the same bus so add it to the bus
                if (busnumOLd == CurrBusNum)
                {
                    var keyFounded = BusStaions.FirstOrDefault(k => k.Key == CurrBusNum);
                    if (keyFounded.Key != 0)//In BusStation Dictionary
                    {
                        if (!BusStaions[CurrBusNum].Contains(ListOfBuses[i].Source))
                            BusStaions[CurrBusNum].Add(ListOfBuses[i].Source);
                        if (!BusStaions[CurrBusNum].Contains(ListOfBuses[i].Destination))
                            BusStaions[CurrBusNum].Add(ListOfBuses[i].Destination);

                    }
                    else  //Not in BusStation Dictionary
                    {
                        BusStaions.Add(CurrBusNum, new List<string>() { ListOfBuses[i].Source });
                        BusStaions[CurrBusNum].Add(ListOfBuses[i].Destination);
                    }
                }
                else //new bus number
                {
                    busnumOLd = CurrBusNum;
                    var keyFounded = BusStaions.FirstOrDefault(k => k.Key == CurrBusNum);
                    if (keyFounded.Key != 0) //In BusStation Dictionary
                    {
                        if (!BusStaions[CurrBusNum].Contains(ListOfBuses[i].Source))
                            BusStaions[CurrBusNum].Add(ListOfBuses[i].Source);
                        if (!BusStaions[CurrBusNum].Contains(ListOfBuses[i].Destination))
                            BusStaions[CurrBusNum].Add(ListOfBuses[i].Destination);
                    }
                    else
                    {
                        BusStaions.Add(CurrBusNum, new List<string>() { ListOfBuses[i].Source });
                        BusStaions[CurrBusNum].Add(ListOfBuses[i].Destination);
                    }
                }
            }
            return BusStaions;
        }

        public void SrcDestLines(string Source, string Destination)
        {
            List<int> keyList = new List<int>(BusStaions.Keys);
            List<List<string>> valueList = new List<List<string>>(BusStaions.Values);
            for (int i = 0; i < keyList.Count; i++)
            {
                var busNumber = keyList[i];
                if (BusStaions[busNumber].FirstOrDefault(k => k == Source) != null && BusStaions[busNumber].FirstOrDefault(k => k == Destination) != null)
                {
                    int Price = ListOfBuses.FirstOrDefault(o => o.BusNumber == busNumber).PriceBus;
                    string s = $"You can take  bus number {busNumber} from {Source} , it'll arrive you to {Destination},The Estimated Trip Cost Is {Price} pound ";
                    finalLines.Add(s);
                }
                else if (BusStaions[busNumber].FirstOrDefault(k => k == Source) != null)
                {
                    SourceLines.Add(busNumber, BusStaions[busNumber]);
                }

                else if (BusStaions[busNumber].FirstOrDefault(k => k == Destination) != null)
                {
                    DestLines.Add(busNumber, BusStaions[busNumber]);
                }

            }

        }

        public void FindtheRoad()
        {
            List<List<string>> ValueListSrc = new List<List<string>>(SourceLines.Values);
            List<int> KeyListSrc = new List<int>(SourceLines.Keys);
            List<int> KeyListDest = new List<int>(DestLines.Keys);

            for (int i = 0; i < SourceLines.Count; i++)
            {
                for (int j = 0; j < DestLines.Count; j++)
                {
                    int num_source = KeyListSrc[i];
                    int num_dest = KeyListDest[j];
                    string comm = CommonStations(SourceLines[num_source], DestLines[num_dest]);
                    if (comm != null)
                    {
                        int TotalPrice = ListOfBuses.FirstOrDefault(o => o.BusNumber == num_source).PriceBus + ListOfBuses.FirstOrDefault(o => o.BusNumber == num_source).PriceBus;

                        finalLines.Add($"You can take Bus number {num_source} and change in {comm} station then take bus number {num_dest},The Estimated Trip Cost Is {TotalPrice}pound ");
                    }

                }
            }
        }
        public static string CommonStations(List<string> source, List<string> dest)
        {
            for (int i = 0; i < source.Count; i++)
            {
                if (dest.Contains(source[i]))
                {
                    return source[i];
                }
            }
            return null;
        }


        [Route("UpdateBus/{busnum}")]
        public IHttpActionResult PutBus([FromUri]int busnum ,BusLocation bus)//Add new bus
        {
            ListOfBuses = Getall();
            BusLocation busedit= ListOfBuses.FirstOrDefault(m=>m.BusNumber==busnum);
            if (busedit == null)
            {
                return NotFound();
            }
            busedit.BusNumber = bus.BusNumber;
            busedit.Destination = bus.Destination;
            busedit.Source = bus.Source;
            //busedit.Stations = bus.Stations;
            busedit.PriceBus = bus.PriceBus;
            DB.SaveChanges();
            return Ok();
        }

        [Route("CreateBus")]

        public IHttpActionResult PostBus(BusLocation bus)//Add new bus
        {
            ListOfBuses = Getall();
            BusLocation bus1 = ListOfBuses.FirstOrDefault(m => m.BusNumber == bus.BusNumber);
            //if (bus1!=null)
            //{
            //    return NotFound();
            //}
            DB.BusLocations.Add(bus);
            DB.SaveChanges();
            return Ok();
        }
        [Route("RemoveBus/{busNo}")]

        public IHttpActionResult DeleteBus([FromUri]int busNo)// deleteByBusNo.
        {
            ListOfBuses = Getall();
           List< BusLocation> busLocation = ListOfBuses.Where(s => s.BusNumber == busNo).ToList();

            if (busLocation == null)
            {
                return NotFound();
            }
            DB.BusLocations.RemoveRange(busLocation);
            DB.SaveChanges();
            return Ok();
        }



    }
}