using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using seawisphunter.custompointer;

public class ExampleUIController : MonoBehaviour {

  public GameObject[] activateTheseIfLeapMotionDisabled;

	void Start () {
    #if CP_HAVE_LEAPMOTION
    foreach (var go in activateTheseIfLeapMotionDisabled)
      go.SetActive(false);
    #else
    var input = CPUtil.FindComponent<StandaloneInputModule>("EventSystem");
    if (input != null)
      input.enabled = true;
    foreach (var go in activateTheseIfLeapMotionDisabled)
      go.SetActive(true);
    #endif
  }
	
  public void Toggled() {
    print("Toggled.");
  }

  public void ButtonPushed() {
    print("Button pushed.");
  }

  public void SliderMoved() {
    print("Slider moved.");
  }

  public void GetLeapMotionCoreAssets() {
    Application.OpenURL("https://github.com/leapmotion/LeapMotionCoreAssets/releases?after=prerelease-v2.4.0");
  }

  public void GetLeapMotionOrionAssets() {
    Application.OpenURL("https://developer.leapmotion.com/orion");
  }
  
}
