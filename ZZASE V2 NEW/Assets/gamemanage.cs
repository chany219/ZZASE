using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamemanage : MonoBehaviour {
    public GameObject 달걀;
    public GameObject 새;
    public int 목표클릭수 = 10;
    public Text Tex_클릭수;
    int 클릭수;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Click()
    {
        Debug.Log("클릭");
        클릭수++;
        Debug.Log("클릭수: " + 클릭수);

        Tex_클릭수.text = 클릭수.ToString();
        if(클릭수==목표클릭수)
        {
            달걀.SetActive(false);
            새.SetActive(true);
        }
    }
}
