using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SetSceneName : MonoBehaviour {

	// Use this for initialization
	void Start () {
    Scene scene = SceneManager.GetActiveScene();
    GetComponent<Text>().text = scene.name; // name of scene
	}
	
	
}
