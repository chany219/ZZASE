using UnityEngine;
using System.Collections;

public class GameControlCamScript : MonoBehaviour 
{
	[Tooltip("Prefab used to create the scene fence.")]
	public GameObject cratePrefab;

	[Tooltip("GUI-Window rectangle in screen coordinates (pixels).")]
	public Rect guiWindowRect = new Rect(10, 200, 200, 300);

	[Tooltip("GUI-Window skin (optional).")]
	public GUISkin guiSkin;


	// whether the fence is already created
	private bool isFenceCreated = false;

	
	void Update () 
	{
		if(!isFenceCreated)
		{
			SpeechManager speechManager = SpeechManager.Instance;

			if(speechManager && speechManager.IsSapiInitialized())
			{
				Quaternion quatRot90 = Quaternion.Euler(new Vector3(0, 90, 0));
				GameObject newObj = null;

			

				isFenceCreated = true;
			}
		}
	}

	private void ShowGuiWindow(int windowID) 
	{
		GUILayout.BeginVertical();

		GUILayout.Label("");
		GUILayout.Label("<b>* FORWARD</b>");
		GUILayout.Label("<b>* BACK</b>");
		GUILayout.Label("<b>* LEFT</b>");
		GUILayout.Label("<b>* RIGHT</b>");
		GUILayout.Label("<b>* TOP</b>");
		GUILayout.Label("<b>* STOP</b>");

		GUILayout.Label("<i>Speech Each direction to change the view of Screen</i>");
		
		GUILayout.EndVertical();
		
		// Make the window draggable.
		GUI.DragWindow();
	}
	
	void OnGUI()
	{
		GUI.skin = guiSkin;
		guiWindowRect = GUI.Window(0, guiWindowRect, ShowGuiWindow, "Audio Commands");
	}
	
}
