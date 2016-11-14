using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using FlightDataService.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System.Runtime.Caching;
using System.Net.Http;
using System.Net;

namespace FlightDataService.Controllers
{
  [MobileAppController]
  public class FlightDataController : ApiController
  {
    private ObjectCache Cache = MemoryCache.Default;

    private const string ActiveIdKey = "ActiveId";

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

        var activeKey = Cache[ActiveIdKey] as string;
        if (activeKey != null)
        {
          var activeFlight = flights.FirstOrDefault(p => p.Id == activeKey);
          if (activeFlight != null)
          {
            activeFlight.IsActive = true;
          }
        }

        return flights;
      }
    }

    [HttpPost]
    public HttpResponseMessage Post(string activeId)
    {
      Cache[ActiveIdKey] = activeId;
      return Request.CreateResponse(HttpStatusCode.OK);
    }
  }
}
