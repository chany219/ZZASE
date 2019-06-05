using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exercise : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void shoulder()
    {
        SceneManager.LoadSceneAsync("04 어깨운동");
    }
    public void chest()
    {
        SceneManager.LoadSceneAsync("05 가슴운동");
    }
    public void lowerbody()
    {
        SceneManager.LoadSceneAsync("06 하체운동");
    }
    public void Back()
    {
        SceneManager.LoadSceneAsync("01 메인메뉴");
    }
}
