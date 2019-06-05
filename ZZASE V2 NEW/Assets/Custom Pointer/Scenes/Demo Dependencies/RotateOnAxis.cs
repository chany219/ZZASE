using UnityEngine;
using System.Collections;

public class RotateOnAxis : MonoBehaviour {
  public Vector3 axis = Vector3.up;
  public float speed = 1f;

  private void Update() 
  {
    transform.Rotate(axis, speed * Time.deltaTime);
  }
}
