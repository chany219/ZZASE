using UnityEngine;
using System.Collections;


// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class CamControlScript : MonoBehaviour, SpeechRecognitionInterface
{
    //	[System.NonSerialized]					
    //	public float lookWeight;					// the amount to transition when using head look
    //	
    //	[System.NonSerialized]
    //	public Transform enemy;						// a transform to Lerp the camera to during head look


    public Camera MainCamera;
    public Camera BackCamera;
    public Camera LeftCamera;
    public Camera RightCamera;
    public Camera TopCamera;
    public AvatarController Player;
    public UnityEngine.Display display1;
    



  

    [Tooltip("Overall animation speed.")]
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed

	//public float lookSmoother = 3f;				// a smoothing setting for camera motion

	[Tooltip("Whether to use the extra curves for animation or not.")]
	public bool useCurves;						// a setting for teaching purposes to show use of curves


	private SpeechManager speechManager;




	// invoked when a speech phrase gets recognized
	public bool SpeechPhraseRecognized(string phraseTag, float condidence)
	{
		switch(phraseTag)
		{
			case "FORWARD":
                MainCamera.enabled = true;
                BackCamera.enabled = false;
                LeftCamera.enabled = false;
                TopCamera.enabled = false;
                RightCamera.enabled = false;
                Player.mirroredMovement = true;
                break;

			case "BACK":

                MainCamera.enabled = false;
                BackCamera.enabled = true;
                LeftCamera.enabled = false;
                RightCamera.enabled = false;
                TopCamera.enabled = false;
                Player.mirroredMovement = true;

                break;

			case "LEFT":
                MainCamera.enabled = false;
                BackCamera.enabled = false;
                LeftCamera.enabled = true;
                RightCamera.enabled = false;
                TopCamera.enabled = false;
                Player.mirroredMovement = true;

                break;

			case "RIGHT":
                MainCamera.enabled = false;
                BackCamera.enabled = false;
                LeftCamera.enabled = false;
                RightCamera.enabled = true;
                TopCamera.enabled = false;
                Player.mirroredMovement = true;

                break;

		

			case "STOP":
				
				break;

	

			case "TOP":
				MainCamera.enabled = false;
                BackCamera.enabled = false;
                LeftCamera.enabled = false;
                RightCamera.enabled = false;
                TopCamera.enabled = true;
                Player.mirroredMovement = true;
				break;
		}

		return true;
	}


	
	


}
