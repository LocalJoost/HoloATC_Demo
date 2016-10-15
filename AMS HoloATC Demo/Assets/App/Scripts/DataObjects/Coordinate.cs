namespace FlightDataService.DataObjects
{
  public class Coordinate
  {
    public double Lon { get; set; }
    public double Lat { get; set; }
    public double? Alt { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
    public bool LocationEquals(Coordinate other)
    {
      if (other == null) return false;
      return (X == other.X && Y == other.Y && Alt != null &&
              other.Alt != null &&
              Alt.Value == other.Alt.Value);
    }
  }
}
