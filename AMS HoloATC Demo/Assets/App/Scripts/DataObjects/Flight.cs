using System.Collections.Generic;

namespace FlightDataService.DataObjects
{
  public class Flight
  {
    public Flight()
    {
      Track = new List<Coordinate>();
    }

    public string Id { get; set; }

    public string FlightNr { get; set; }

    public string Aircraft { get; set; }

    public Coordinate Location { get; set; }

    public double? Speed { get; set; }

    public double? Heading { get; set; }

    public List<Coordinate> Track { get; set; }

    public TrackType TypeTrack { get; set; }

    public bool IsActive { get; set; }

    public override string ToString()
    {
      return string.Format("{0} {1} {2} {3} {4} {5}", FlightNr, Aircraft, 
        Location.Alt, Speed, Heading, TypeTrack).Trim();
    }
  }
}
