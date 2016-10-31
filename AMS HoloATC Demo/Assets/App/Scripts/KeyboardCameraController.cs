using UnityEngine;

public class KeyboardCameraController : MonoBehaviour
{
  public float Rotatespeed = 0.4f;
  public float MoveSpeed = 0.02f;
  public float FastSpeedAccleration = 7.5f;

  private Quaternion _initialRotation;
  private Vector3 _initialPosition;
  void Start()
  {
    _initialRotation = Camera.main.transform.rotation;
    _initialPosition = Camera.main.transform.position;
  }

  void Update()
  {
    var speed = 1.0f;
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    {
      speed = FastSpeedAccleration * speed;
    }

    if (Input.GetKey(KeyCode.LeftArrow))
    {
      Camera.main.transform.RotateAround(Camera.main.transform.position,
          Camera.main.transform.up, -Rotatespeed * speed);
    }

    if (Input.GetKey(KeyCode.RightArrow))
      Camera.main.transform.RotateAround(Camera.main.transform.position,
          Camera.main.transform.up, Rotatespeed * speed);

    if (Input.GetKey(KeyCode.UpArrow))
    {
      Camera.main.transform.RotateAround(Camera.main.transform.position,
          Camera.main.transform.right, -Rotatespeed * speed);
    }

    if (Input.GetKey(KeyCode.DownArrow))
      Camera.main.transform.RotateAround(Camera.main.transform.position,
          Camera.main.transform.right, Rotatespeed * speed);

    if (Input.GetKey(KeyCode.A))
      Camera.main.transform.position +=
          Camera.main.transform.right * -MoveSpeed * speed;

    if (Input.GetKey(KeyCode.D))
      Camera.main.transform.position +=
          Camera.main.transform.right * MoveSpeed * speed;

    if (Input.GetKey(KeyCode.W))
      Camera.main.transform.position +=
          Camera.main.transform.forward * MoveSpeed * speed;

    if (Input.GetKey(KeyCode.S))
      Camera.main.transform.position +=
          Camera.main.transform.forward * -MoveSpeed * speed;

    if (Input.GetKey(KeyCode.PageUp))
      Camera.main.transform.position +=
          Camera.main.transform.up * MoveSpeed * speed;

    if (Input.GetKey(KeyCode.PageDown))
      Camera.main.transform.position +=
          Camera.main.transform.up * -MoveSpeed * speed;

    if (Input.GetKey(KeyCode.Escape))
    {
      Camera.main.transform.position = _initialPosition;
      Camera.main.transform.rotation = _initialRotation;
    }

  }
}
