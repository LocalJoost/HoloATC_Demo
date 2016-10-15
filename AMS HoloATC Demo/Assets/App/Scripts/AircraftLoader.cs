using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlightDataService.DataObjects;
#if UNITY_UWP
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;
#endif

public class AircraftLoader : MonoBehaviour
{
  public GameObject Aircraft;

  public string TopLevelName = "HologramCollection";

  private Dictionary<string, GameObject> _aircrafts;

  private readonly TimeSpan _waitTime = TimeSpan.FromSeconds(5);

  private DateTimeOffset _lastUpdate = DateTimeOffset.MinValue;

  private Queue<FlightSet> _receivedData;

  private GameObject _topLevelObject;

  // Use this for initialization
  void Start()
  {
    _aircrafts = new Dictionary<string, GameObject>();
    _receivedData = new Queue<FlightSet>();
    _topLevelObject = GameObject.Find(TopLevelName);
  }

#if UNITY_UWP

  private void ProcessData(Task<List<Flight>> flightData)
  {
    if (flightData.IsCompleted && !flightData.IsFaulted)
    {
      var set = new FlightSet(flightData.Result);
      _receivedData.Enqueue(set);
    }
  }
#endif

  private bool _isUpdating;
  void Update()
  {
    if ((_lastUpdate - DateTimeOffset.Now).Duration() > _waitTime)
    {
      _lastUpdate = DateTimeOffset.Now;
#if UNITY_UWP
      DataService.Instance.GetFlights().ContinueWith(ProcessData);
#endif
    }
    if (!_isUpdating)
    {
      _isUpdating = true;
      if (_receivedData.Any())
      {
        var set = _receivedData.Dequeue();
        var flightIds = set.Flights.Select(p => p.Id).ToList();

        var aircraftToDelete =
            _aircrafts.Keys.Where(p => !flightIds.Contains(p)).ToList();
        DeleteAircraft(aircraftToDelete);

        var keysToUpdate = _aircrafts.Keys.Where(p => flightIds.Contains(p));
        var aircraftToUpdate =
            set.Flights.Where(p => keysToUpdate.Contains(p.Id)).ToList();
        UpdateAircraft(aircraftToUpdate);

        var aircraftToAdd =
            set.Flights.Where(p => !_aircrafts.Keys.Contains(p.Id)).ToList();
        CreateAircraft(aircraftToAdd);
      }

      _isUpdating = false;
    }
  }

  private void CreateAircraft(IEnumerable<Flight> flights)
  {
    foreach (var flight in flights)
    {
      var aircraft = Instantiate(Aircraft);

      aircraft.transform.parent = _topLevelObject.transform;
      aircraft.transform.localScale = new Vector3(0f, 0f, 0f);

      SetNewFlightData(aircraft, flight);
      _aircrafts.Add(flight.Id, aircraft);
    }
  }

  private void UpdateAircraft(IEnumerable<Flight> flights)
  {
    foreach (var flight in flights)
    {
      if (_aircrafts.ContainsKey(flight.Id))
      {
        var aircraft = _aircrafts[flight.Id];
        SetNewFlightData(aircraft, flight);
      }
    }
  }


  private void DeleteAircraft(IEnumerable<string> keys)
  {
    foreach (var key in keys)
    {
      var aircraft = _aircrafts[key];
      Destroy(aircraft);
      _aircrafts.Remove(key);
    }
  }

  private void SetNewFlightData(GameObject aircraft, Flight flight)
  {
    var controller = aircraft.GetComponent<AircraftController>();
    if (controller != null)
    {
      controller.SetNewFlightData(flight);
    }
  }

}
