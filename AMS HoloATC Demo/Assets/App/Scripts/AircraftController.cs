using System;
using UnityEngine;
using FlightDataService.DataObjects;

public class AircraftController : MonoBehaviour
{
  private Flight _flightData;
  private float? _speed;
  private float? _heading;
  private TextMesh _text;
  private bool _initComplete;
  private bool _firstMove;

  void Start()
  {
    _text = transform.GetComponentInChildren<TextMesh>();
    _initComplete = true;
  }


  public void SetNewFlightData(Flight newFlightData)
  {
    if (_initComplete)
    {
      var move = _flightData == null ||
                 !_flightData.Location.LocationEquals(newFlightData.Location);
      _flightData = newFlightData;

      ExtractSpeedAndHeading();
      if (move)
      {
        SetLocationOrientation();
      }
      else
      {
        SetNewFlightText();
      }
    }
  }

  private void ExtractSpeedAndHeading()
  {
    if (_flightData.Heading != null)
    {
      _heading = (float)_flightData.Heading;
    }
    if (_heading < 0)
    {
      _heading += 360;
    }
    if (_flightData.Speed != null)
    {
      _speed = (float)_flightData.Speed;
    }
  }

  private void SetNewFlightText()
  {
    var speedText =
      _speed != null ? string.Format("{0}km/h", _speed) : string.Empty;

    var headingText =
      _heading != null ? string.Format("{0}⁰", _heading) : string.Empty;

    var text = string.Format("{0} {1} {2}m {3} {4}", _flightData.FlightNr,
      _flightData.Aircraft, _flightData.Location.Alt, speedText,
      headingText).Trim();
    _text.text = text;
  }

  private void SetLocationOrientation()
  {
    SetNewFlightText();

    transform.localPosition = GetFlightLocation();
    if (_flightData.Heading != null)
    {
      transform.localEulerAngles = GetNewRotation();
    }
    if (!_firstMove)
    {
      transform.localScale = new Vector3(0.0015f, 0.0015f, 0.0015f);
      _firstMove = true;
    }
  }

  private Vector3 GetFlightLocation()
  {
    return GetLocalCoordinates(_flightData.Location);
  }

  private Vector3 GetLocalCoordinates(Coordinate c)
  {
    return new Vector3((float)c.X / 15000,
      c.Alt != null ? (float)c.Alt / 2000.0f : 0f,
      (float)c.Y / 15000);
  }

  private Vector3 GetNewRotation()
  {
    var heading = _heading ?? 0;
    var rotation = Quaternion.AngleAxis(heading, Vector3.up).eulerAngles +
        Quaternion.AngleAxis(GetVerticalAngle(), Vector3.right).eulerAngles;
    return rotation;
  }

  private float GetVerticalAngle()
  {
    var tracksize = _flightData.Track.Count;
    if (tracksize > 2)
    {
      var pLast = _flightData.Track[tracksize - 1];
      var pSecondLast = _flightData.Track[tracksize - 3];
      var delta = pLast.Alt - pSecondLast.Alt;
      if (Math.Abs(delta.Value) > 2.5f)
      {
        return delta < 0 ? 10 : -20;
      }
    }
    return 0;
  }
}
