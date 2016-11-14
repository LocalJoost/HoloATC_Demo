using System;
using System.Collections.Generic;
using FlightDataService.DataObjects;
using HoloToolkit.Unity;
#if UNITY_UWP
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;
using System.Threading.Tasks;
#endif

public class DataService : Singleton<DataService>
{
  public string DataUrl = "http://yourflightdataservice.azurewebsites.net/";

#if UNITY_UWP
  private MobileServiceClient _client;
#endif

  public DataService()
  {
#if UNITY_UWP
    _client = new MobileServiceClient(new Uri(DataUrl));
#endif
  }

#if UNITY_UWP
  public async Task<List<Flight>> GetFlights()
  {
    var result = await _client.InvokeApiAsync<List<Flight>>(
                  "FlightData", HttpMethod.Get, null);
    return result;
  }
#endif

#if UNITY_UWP
  public async Task SelectFlight(string flightId)
  {
    var parms = new Dictionary<string, string>
    {
      { "activeId", flightId }
    };
    await _client.InvokeApiAsync<List<Flight>>("FlightData", HttpMethod.Post, parms);
  }
#endif
}
