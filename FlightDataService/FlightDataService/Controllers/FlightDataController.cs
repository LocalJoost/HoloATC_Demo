using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using FlightDataService.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;

namespace FlightDataService.Controllers
{
    [MobileAppController]
    public class FlightDataController : ApiController
    {
    // GET api/FlightData
    [HttpGet]
    public List<Flight> Get()
    {
      var dataDirectory = new DirectoryInfo(
        System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data"));

      var time = DateTimeOffset.UtcNow;
      var maxPattern = $"{(time.Hour % 10) + 8:00}_{time.Minute:00}_{time.Second:00}.json";
      var datafile =
          dataDirectory.EnumerateFiles()
              .Where(p => string.Compare(p.Name, maxPattern, StringComparison.Ordinal) >= 0)
              .OrderBy(p => p.Name)
              .First();
      using (var stream = datafile.OpenText())
      {
        var flights = JsonConvert.DeserializeObject<List<Flight>>(stream.ReadLine());
        return flights;
      }
    }
  }
}
