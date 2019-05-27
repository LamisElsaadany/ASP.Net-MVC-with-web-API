using Moslah.Models;
using Moslah.Models.OwnModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Moslah.Controllers
{
    [RoutePrefix("Moslah/Metro")]
    public class MetroController : ApiController
    {

        ApplicationDbContext DB = new ApplicationDbContext();
        Dictionary<int, List<string>> MetroStaions = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> SourceLines = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> DestLines = new Dictionary<int, List<string>>();
        List<string> finalLines = new List<string>();
        List<MetroLocation> ListOfMetro = new List<MetroLocation>();
        [Route("all")]
        public List<MetroLocation> Getall()
        {
            ListOfMetro = DB.MetroLocations.ToList();
            return ListOfMetro;
        }

        [Route("MetroNumbers/{no}")]
        public List<int> GetMetroNo(int no)
        {
            return DB.MetroLocations.Where(o => o.MetroNumber.ToString().StartsWith(no.ToString())).Select(o => o.MetroNumber).ToList();
        }

        [Route("allMetroNumbers")]
        public List<int> GetAllMetroNo()
        {
            return DB.MetroLocations.Select(o => o.MetroNumber).Distinct().ToList();
        }
        // GET: Bus
        [Route("CityMetro/{city}")]
        public IHttpActionResult GetMetro(string city)
        {
            ListOfMetro = DB.MetroLocations.Where(m=>m.Stations.City.CityName==city).ToList();
            if (ListOfMetro.Count == 0)
                return NotFound();

            return Ok(ListOfMetro);
        }
        public List<MetroLocation> GetMetroByDestination(string destination,string city)
        {
            GetMetro(city);
            List<MetroLocation> metro = ListOfMetro.Where(s => s.Destination == destination).ToList();
            return metro;
        }
        public List<MetroLocation> GetMetroBySource(string Source,string city)
        {
            GetMetro(city);
            List<MetroLocation> metro = ListOfMetro.Where(s => s.Source == Source).ToList();
            return metro;
        }
        [Route("MetroRoad/{Source}/{Destination}/{city}")]
        public IHttpActionResult GetMetroRoad(string Source, string Destination, string city)
        {
            City ncity = DB.Cities.FirstOrDefault(o => o.CityName == city);
            GetMetro(city);
            ReformateDB();
            SrcDestLines(Source, Destination);
            FindtheRoad(Source, Destination);
            for (int i = 0; i < finalLines.Count; i++)
            {
                QuickSearch quickSearch = new QuickSearch();
                quickSearch.Destination = Destination;
                quickSearch.Source = Source;
                quickSearch.RoadDesc = finalLines[i];
                quickSearch.VechialType = "Metro";
                quickSearch.City = ncity;
                DB.Quicksearch.Add(quickSearch);
                DB.SaveChanges();
            }
            if (finalLines.Count == 0)
                return NotFound();

            return Ok(finalLines);

        }
        //This Function is for reformate db and return dictionary with key equal bus number
        //Values equal the stations of the bus
        public  Dictionary<int, List<string>> ReformateDB()
        {
            int busnumOLd = ListOfMetro[0].MetroNumber;
            for (int i = 0; i < ListOfMetro.Count; i++)
            {
                int CurrBusNum = ListOfMetro[i].MetroNumber;
                //If It has the Same bus number of the previuse so it will be a station
                //of the same bus so add it to the bus
                if (busnumOLd == CurrBusNum)
                {
                    var keyFounded = MetroStaions.FirstOrDefault(k => k.Key == CurrBusNum);
                    if (keyFounded.Key != 0)//In BusStation Dictionary
                    {
                        if (!MetroStaions[CurrBusNum].Contains(ListOfMetro[i].Source))
                            MetroStaions[CurrBusNum].Add(ListOfMetro[i].Source);
                        if (!MetroStaions[CurrBusNum].Contains(ListOfMetro[i].Destination))
                            MetroStaions[CurrBusNum].Add(ListOfMetro[i].Destination);

                    }
                    else  //Not in BusStation Dictionary
                    {
                        MetroStaions.Add(CurrBusNum, new List<string>() { ListOfMetro[i].Source });
                        MetroStaions[CurrBusNum].Add(ListOfMetro[i].Destination);
                    }
                }
                else //new bus number
                {
                    busnumOLd = CurrBusNum;
                    var keyFounded = MetroStaions.FirstOrDefault(k => k.Key == CurrBusNum);
                    if (keyFounded.Key != 0) //In BusStation Dictionary
                    {
                        if (!MetroStaions[CurrBusNum].Contains(ListOfMetro[i].Source))
                            MetroStaions[CurrBusNum].Add(ListOfMetro[i].Source);
                        if (!MetroStaions[CurrBusNum].Contains(ListOfMetro[i].Destination))
                            MetroStaions[CurrBusNum].Add(ListOfMetro[i].Destination);
                    }
                    else
                    {
                        MetroStaions.Add(CurrBusNum, new List<string>() { ListOfMetro[i].Source });
                        MetroStaions[CurrBusNum].Add(ListOfMetro[i].Destination);
                    }
                }
            }
            return MetroStaions;

        }

        public void SrcDestLines(string Source, string Destination)
        {
            List<int> keyList = new List<int>(MetroStaions.Keys);
            List<List<string>> valueList = new List<List<string>>(MetroStaions.Values);
            for (int i = 0; i < keyList.Count; i++)
            {
                var MetroNumber = keyList[i];
                if (MetroStaions[MetroNumber].FirstOrDefault(k => k == Source) != null && MetroStaions[MetroNumber].FirstOrDefault(k => k == Destination) != null)
                {
                    int stationNo = Math.Abs(MetroStaions[MetroNumber].IndexOf(Destination) - MetroStaions[MetroNumber].IndexOf(Source));
                    string s = $"You can take the mertroline {MetroNumber} from {Source} , it'll arrive you to {Destination},The Estimated Trip Cost Is  {calculatePrice(stationNo)} pound ";
                    finalLines.Add(s);
                }
                else if (MetroStaions[MetroNumber].FirstOrDefault(k => k == Source) != null)
                {
                    SourceLines.Add(MetroNumber, MetroStaions[MetroNumber]);
                }

                else if (MetroStaions[MetroNumber].FirstOrDefault(k => k == Destination) != null)
                {
                    DestLines.Add(MetroNumber, MetroStaions[MetroNumber]);
                }

            }

        }
        public int calculatePrice(int StationsCount)
        {
            if (StationsCount <= 9)
            {
                return 3;

            }
            else if (StationsCount <= 16)
            {
                return 5;
            }
            else
            {
                return 7;
            }
        }

        public void FindtheRoad(string Source, string Destination)
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
                        int stationNo1 = Math.Abs(MetroStaions[num_source].IndexOf(Source) - MetroStaions[num_source].IndexOf(comm));
                        int stationNo2 = Math.Abs(MetroStaions[num_source].IndexOf(Destination) - MetroStaions[num_source].IndexOf(comm));
                        int stationNo = stationNo1 + stationNo2;

                        string s = $"You can take Metro line {num_source} from {Source} station and change in {comm} then take  {Destination} from Metro Line{num_dest},the trip Cost {calculatePrice(stationNo)} pound ";

                        finalLines.Add(s);
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

        [Route("UpdateMetro/{metronum}")]
        public IHttpActionResult PutMetro(int metronum, MetroLocation metro)//Add new bus
        {
            ListOfMetro = Getall();
            MetroLocation metroedit= ListOfMetro.FirstOrDefault(m => m.MetroNumber == metronum);
            if (metroedit == null)
                return NotFound();
            metroedit.MetroNumber = metro.MetroNumber;
            metroedit.Destination = metro.Destination;
            metroedit.Source = metro.Source;
            //metroedit.Stations = metro.Stations;
            DB.SaveChanges();
            return Ok();
        
        }

        [Route("CreateMetro")]
        public IHttpActionResult Post(MetroLocation metro)//Add new bus
        {
            ListOfMetro = Getall();
           // if (ListOfMetro.FirstOrDefault(m => m.MetroNumber == metro.MetroNumber) == null)
            //{
                DB.MetroLocations.Add(metro);
                DB.SaveChanges();
                return Ok();
           // }
           // return NotFound();
        }

        [Route("RemoveMetro/{metroNo}")]
        public IHttpActionResult DeleteMetro(int metroNo)// deleteByBusNo.
        {
            ListOfMetro = Getall();
            List<MetroLocation> MetroLocation = ListOfMetro.Where(s => s.MetroNumber == metroNo).ToList();
            if(MetroLocation==null)
            {
                return NotFound();
            }
            DB.MetroLocations.RemoveRange(MetroLocation);
            DB.SaveChanges();
            return Ok();
        }



    }
}