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
    [RoutePrefix("Moslah/MicroBus")]
    public class MicroBusController : ApiController
    {
        ApplicationDbContext DB = new ApplicationDbContext();
        Dictionary<int, List<string>> MicrobusStations = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> SourceLines = new Dictionary<int, List<string>>();
        Dictionary<int, List<string>> DestLines = new Dictionary<int, List<string>>();
        List<string> finalLines = new List<string>();
        List<MicroBus> ListOfMicro = new List<MicroBus>();
        [Route("all")]
        public List<MicroBus> Getall()
        {
            ListOfMicro = DB.Microbus.ToList();
            return ListOfMicro;
        }
        [Route("MicroNumbers/{no}")]
        public List<int> GetMicroNo(int no)
        {
            return DB.Microbus.Where(o => o.MicroID.ToString().StartsWith(no.ToString())).Select(o=>o.MicroID).ToList();
        }

        [Route("allMicroNumbers")]
        public List<int> GetAllMicroNo()
        {
            return DB.Microbus.Select(o => o.MicroID).Distinct().ToList();
        }
        //Lazam fe awl 3 get na3ml check 3ala el city 3ashan magabsh el db kolha
        // GET: Bus
        [Route("CityMicroBus/{city}")]
        public IHttpActionResult GetMicroBuses(string city)
        {
            ListOfMicro = DB.Microbus.Where(m=>m.Stations.City.CityName==city).ToList();
            if (ListOfMicro.Count == 0)
                return NotFound();
            return Ok(ListOfMicro);
        }
        public List<MicroBus> GetMicrbusByDestination(string destination, string city)
        {
            GetMicroBuses(city);
            List<MicroBus> buses = ListOfMicro.Where(s => s.Destination == destination).ToList();
            return buses;
        }
        public List<MicroBus> GetMicrobusBySource(string Source, string city)
        {
            GetMicroBuses(city);
            List<MicroBus> buses = ListOfMicro.Where(s => s.Source == Source).ToList();
            return buses;
        }
        [Route("MicroBusRoad/{Source}/{Destination}/{city}")]
        public IHttpActionResult GetMicroBusRoad(string Source, string Destination, string city)
        {
            City ncity = DB.Cities.FirstOrDefault(o => o.CityName == city);
            GetMicroBuses(city);
            ReformateDB();
            SrcDestLines(Source, Destination);
            FindtheRoad();
            for (int i = 0; i < finalLines.Count; i++)
            {
                QuickSearch quickSearch = new QuickSearch();
                quickSearch.Destination = Destination;
                quickSearch.Source = Source;
                quickSearch.RoadDesc = finalLines[i];
                quickSearch.VechialType = "MicroBus";
                quickSearch.City = ncity;
                DB.Quicksearch.Add(quickSearch);
                DB.SaveChanges();
            }
            if (finalLines.Count == 0)
                return NotFound();

            return Ok(finalLines);

        }
        //This Function is for reformate db and return dictionary with key equal Microbus number
        //Values equal the stations of the Microbus
        public Dictionary<int, List<string>> ReformateDB()
        {
            int busnumOLd = ListOfMicro[0].MicroID;
            for (int i = 0; i < ListOfMicro.Count; i++)
            {
                int CurrBusNum = ListOfMicro[i].MicroID;
                //If It has the Same bus number of the previuse so it will be a station
                //of the same bus so add it to the bus
                if (busnumOLd == CurrBusNum)
                {
                    var keyFounded = MicrobusStations.FirstOrDefault(k => k.Key == CurrBusNum);
                    if (keyFounded.Key != 0)//In BusStation Dictionary
                    {
                        if (!MicrobusStations[CurrBusNum].Contains(ListOfMicro[i].Source))
                            MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Source);
                        if (!MicrobusStations[CurrBusNum].Contains(ListOfMicro[i].Destination))
                            MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Destination);

                    }
                    else  //Not in BusStation Dictionary
                    {
                        MicrobusStations.Add(CurrBusNum, new List<string>() { ListOfMicro[i].Source });
                        MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Destination);
                    }
                }
                else //new bus number
                {
                    busnumOLd = CurrBusNum;
                    var keyFounded = MicrobusStations.FirstOrDefault(k => k.Key == CurrBusNum);
                    if (keyFounded.Key != 0) //In BusStation Dictionary
                    {
                        if (!MicrobusStations[CurrBusNum].Contains(ListOfMicro[i].Source))
                            MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Source);
                        if (!MicrobusStations[CurrBusNum].Contains(ListOfMicro[i].Destination))
                            MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Destination);
                    }
                    else
                    {
                        MicrobusStations.Add(CurrBusNum, new List<string>() { ListOfMicro[i].Source });
                        MicrobusStations[CurrBusNum].Add(ListOfMicro[i].Destination);
                    }
                }
            }
            return MicrobusStations;

        }
        public void SrcDestLines(string Source, string Destination)
        {
            List<int> keyList = new List<int>(MicrobusStations.Keys);
            List<List<string>> valueList = new List<List<string>>(MicrobusStations.Values);
            for (int i = 0; i < keyList.Count; i++)
            {
                var MicrobusNum = keyList[i];
                if (MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Source) != null && MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Destination) != null)
                {
                    int Price = DB.Microbus.FirstOrDefault(o => o.Destination == Destination && o.Source == Source).PriceMicro;
                    string s = $"You can take Microbus from {Source} , it'll arrive you to {Destination},The Trip Cost{Price}";

                    finalLines.Add(s);
                }
                else if (MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Source) != null)
                {
                    SourceLines.Add(MicrobusNum, MicrobusStations[MicrobusNum]);
                }

                else if (MicrobusStations[MicrobusNum].FirstOrDefault(k => k == Destination) != null)
                {
                    DestLines.Add(MicrobusNum, MicrobusStations[MicrobusNum]);
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
                        int TotalPrice = ListOfMicro.FirstOrDefault(o => o.MicroID == num_source).PriceMicro + ListOfMicro.FirstOrDefault(o => o.MicroID == num_source).PriceMicro;

                        finalLines.Add($"You can take Microbus from {SourceLines[num_source]} " /*+
                            $"in station {ListOfMicro.FirstOrDefault(m => m.MicroID == num_source).Stations}" */+
                            $" and change in {comm} then take Microbus to {DestLines[num_dest]}, the estimated trip cost {TotalPrice}");
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

        [Route("UpdateMicroBus/{Micronum}")]
        public IHttpActionResult PutMicrobus([FromUri]int Micronum, MicroBus Microbus)//Add new bus
        {
            ListOfMicro = Getall();
            MicroBus microedit = ListOfMicro.FirstOrDefault(m => m.MicroID == Micronum);
            if (microedit != null)
            {
                microedit.MicroID = Microbus.MicroID;
                microedit.Destination = Microbus.Destination;
                microedit.Source = Microbus.Source;
                //microedit.Stations = Microbus.Stations;
                microedit.PriceMicro = Microbus.PriceMicro;
                DB.SaveChanges();
                return Ok();
            }

            return NotFound();
        }
        [Route("CreateMicroBus")]
        public IHttpActionResult PostBus(MicroBus Microbus)//Add new Microbus
        {
            ListOfMicro = Getall();
            // if (ListOfMicro.FirstOrDefault(m=>m.MicroID == Microbus.MicroID) == null)
            //{
            DB.Microbus.Add(Microbus);
            DB.SaveChanges();
            return Ok();
            //}
            //return NotFound();
        }
        [Route("RemoveMicroBus/{MicroNo}")]
        public IHttpActionResult DeleteBus(int MicroNo)// deleteByBusNo.
        {
            ListOfMicro = Getall();
            List<MicroBus> MicroLocation = ListOfMicro.Where(s => s.MicroID == MicroNo).ToList();
            if (MicroLocation != null)
            {
                DB.Microbus.RemoveRange(MicroLocation);
                DB.SaveChanges();
                return Ok();
            }
            return NotFound();
        }



    }
}