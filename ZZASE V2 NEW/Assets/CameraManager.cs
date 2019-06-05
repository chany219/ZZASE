using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public Camera MainCamera;
    public Camera BackCamera;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowOverheadView()
    {
        MainCamera.enabled = false;
        BackCamera.enabled = true;
    }

    public void ShowFirstPersonView()
    {
        MainCamera.enabled = true;
        BackCamera.enabled = false;
    }
}
