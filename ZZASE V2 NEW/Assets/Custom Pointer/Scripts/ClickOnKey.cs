using UnityEngine;
using System.Collections;
using seawisphunter.custompointer;

public class ClickOnKey : MonoBehaviour {
  public KeyCode clickKey = KeyCode.Space;

	// Update is called once per frame
	void Update () {
    if (Input.GetKeyDown(clickKey)) {
      GetComponent<RegisterTransformPointer>().Click();
    }
	}
}
