using UnityEngine;
using System.Collections;

namespace seawisphunter.custompointer {
  
  public class MoveViaKeyboard : MonoBehaviour {

    public float speed = 1f;
     
    void Update() {
      var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
      transform.position += move * speed * Time.deltaTime;
    }
  }
  
}
