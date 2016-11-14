using UnityEngine;
using HoloToolkit.Unity;

public class HandColorCursor : Singleton<HandColorCursor>
{

  // Use this for initialization
  private GameObject _cursorOnHolograms;

  public Color HandDetectedColor;

  private Color _originalColor = Color.green;

  void Start()
  {
    _cursorOnHolograms = CursorManager.Instance.CursorOnHolograms;
    _originalColor = 
      _cursorOnHolograms.GetComponent<MeshRenderer>().material.color;
  }

  // Update is called once per frame
  void LateUpdate()
  {
    if (_cursorOnHolograms != null && HandsManager.Instance != null)
    {
      _cursorOnHolograms.GetComponent<MeshRenderer>().material.color =
        HandsManager.Instance.HandDetected ? Color.green : _originalColor;
    }
  }
}
