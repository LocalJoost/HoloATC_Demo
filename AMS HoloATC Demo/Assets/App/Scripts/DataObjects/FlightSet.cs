using System;
using System.Collections.Generic;

namespace FlightDataService.DataObjects
{
  public class FlightSet
  {
    public FlightSet()
    {
    }

    public FlightSet(List<Flight> flights)
    {
      Flights = flights;
      TimeStamp = DateTimeOffset.Now;
    }
    public DateTimeOffset TimeStamp { get; set; }

    public List<Flight> Flights { get; set; }
  }
}
