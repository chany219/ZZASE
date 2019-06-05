using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScriptNew : MonoBehaviour {

    void Start()
    {

    }

    void Update()
    {

    }

    public void SquatScene()
    {
        SceneManager.LoadScene("KinectPoseDetectorSquat");
    }

    


    public void LungeScene()
    {
        SceneManager.LoadScene("KinectPoseDetectorLunge2");
    }


    public void DumbelScene()
    {
        SceneManager.LoadScene("KinectPoseDetectorSqt");
    }

    public void Scene()
    {
        SceneManager.LoadScene("KinectPoseDetector");
    }
}