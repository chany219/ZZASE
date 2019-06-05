using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

namespace seawisphunter.custompointer {

  /**
     This holds the extra information required to make a transform
     into a pointer.  It is used by the event system.  It's not a
     MonoBehaviour and does not need to be added as a component to any
     GameObject.
   */
  public class TransformEventData : PointerEventData {

    /* This transform will be considered the cursor.  Its position in
     * world space will be converted to screen space. */
    public Transform pointer;
  
    /* This should probably just be an animation. */
    public UnityEngine.UI.Image progressBar; 
    public float timeSinceMoved;
    public float timeSinceEntered;
    public float timeSinceClicked;
    public Vector3 lastPosition;

    public TransformEventData(EventSystem eventSystem) : base(eventSystem) {
      // Set time since moved and time since entered to some reasonable
      // value in the past that won't trip up the logic.
      timeSinceMoved = Time.unscaledTime - 10f;
      timeSinceEntered = Time.unscaledTime - 10f;
      timeSinceClicked = timeSinceEntered - 1f;
    }

    /*
      If we have a progress circle, set it to between [0, 1]. Zero is 0
      progress (invisible) and one is 100% progress.
    */
    public void SetProgress(float proportion) {
      if (progressBar != null)
        progressBar.fillAmount = Mathf.Clamp01(proportion);
    }

    public override string ToString() {
      var sb = new StringBuilder();
      sb.Append(base.ToString());
      sb.AppendLine("<b>dragging</b>: " + dragging);
      sb.AppendLine("<b>pointer</b>: " + pointer);
      sb.AppendLine("<b>timeSinceMoved</b>: " + timeSinceMoved);
      sb.AppendLine("<b>timeSinceEntered</b>: " + timeSinceEntered);
      sb.AppendLine("<b>timeSinceClicked</b>: " + timeSinceClicked);
      sb.AppendLine("<b>lastPosition</b>: " + lastPosition);
      return sb.ToString();
    }
  }
    
}
