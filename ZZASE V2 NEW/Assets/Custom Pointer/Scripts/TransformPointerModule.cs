using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace seawisphunter.custompointer {

  /*
    Given any transform, it can be made into a mouse-like pointer for
    Unity's UI.
  */
  [AddComponentMenu("Event/Transform Pointer Module")]
  public class TransformPointerModule : PointerInputModule {
    /* Hovering over a button, will cause it to click. */
    public bool enableHoverClick = true;
    public bool enableHoverDrag = false;
    /* Time before progress bar comes up. */
    public float preProgress = 0.2f;
    /* Time to fill progress bar. */
    public float timeToFill = 1f;
    /* Time in seconds to deactivate if pointer is not moving. */
    public float timeToDeactivate = 5f;

    //private bool shouldClick = false; // This should be per id.
    private HashSet<int> shouldClick = new HashSet<int>();

    protected TransformPointerModule() {}

    // Maybe instead of an image, it should be just some object that
    // has the following properties?
    // ProgressIndicator.proportion
    // ProgressIndicator.screenLocation
    // ProgressIndicator.worldLocation
    /*
      Register a pointer with a particular transform and pointer ID.
    */
    public void RegisterPointer(int id, Transform pointer, Image progressBar) {
      print("register pointer id " + id);
      PointerEventData eventData;
      bool newData = GetPointerData(id, out eventData, true);
      newData |= true; // XXX avoid the warning that newData is never used.
      TransformEventData eData = (TransformEventData) eventData;
      //print("set pointer");
      eData.pointer = pointer;
      //print("set pointer " + eData.pointer);
      eData.progressBar = progressBar;
      // pointer = t;
      // pointerId = id;
      // lastPosition = pointer.position;
    }

    /*
      Has this pointer already been registered?  Return true if it has been registered.
     */
    public bool HasPointerId(int id) {
      return m_PointerData.ContainsKey(id);
    }

    public int GetNextPointerId(int id) {
      int i;
      for (i = 0; i < 20 && HasPointerId(id + i); i++) {
      }
      return id + i;
    }

    public void Click(int id) {
      //print("should click by id " + id);
      //shouldClick = true;
      shouldClick.Add(id);
    }            

    /*
      Unregister a pointer with the given id.
    */
    public void UnregisterPointer(int id) {
      // Make sure to send an exit event if the pointer goes away.
      PointerEventData pointerEvent;
      GetPointerData(id, out pointerEvent, false);
      if (pointerEvent != null
          && pointerEvent.pointerEnter != null)
        ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter,
                                       pointerEvent,
                                       ExecuteEvents.pointerExitHandler);
      m_PointerData.Remove(id);
    }


    /* The open source code says this is protected but with Unity 4.6.1,
       it's not callable from an inherited. Returns true if created,
       false otherwise.*/
    protected new bool GetPointerData(int id, out PointerEventData data, bool create)
    {
      if (!m_PointerData.TryGetValue(id, out data) && create) {
        data = new TransformEventData(eventSystem) {
          pointerId = id,
        };
        m_PointerData.Add(id, data);
        return true;
      }
      return false;
    }
  
    /*
      Has this or one of its ancestors have a component of type T.
    */
    protected bool HasAncestor<T>(GameObject g, int maxAncestor = 4)
      where T : Component {
      if (maxAncestor > 0
          && g != null
          && g.GetComponent<T>() != null) {
        return true;
      } else if (g != null && g.transform.parent != null) {
        return HasAncestor<T>(g.transform.parent.gameObject, maxAncestor - 1);
      } else {
        return false;
      }
    }
                
    /* We should activate if we're enabled, there's a pointer of ours in
       the scene, and it's moved recently. */
    public override bool ShouldActivateModule() {
      if (! enabled)
        return false;
      // If we moved recently, then we should probably activate.
      foreach (PointerEventData eventData in m_PointerData.Values) {
        TransformEventData eventDataP = (TransformEventData) eventData;
        Transform pointer = eventDataP.pointer;
        if (pointer != null
            && ! Mathf.Approximately((pointer.position - eventDataP.lastPosition).magnitude, 0f)) {
          eventDataP.lastPosition = pointer.position;
          eventDataP.timeSinceMoved = Time.unscaledTime;
        }
        if (pointer != null
            && (Time.unscaledTime - eventDataP.timeSinceMoved) < timeToDeactivate)
          return true;
      }
      return false;
    }

    private bool IsClickable(GameObject currentOverGo) {
      return HasAncestor<Button>(currentOverGo)
        || HasAncestor<Slider>(currentOverGo)
        || HasAncestor<Toggle>(currentOverGo);
    }

    private bool IsDraggable(GameObject currentOverGo) {
      return HasAncestor<Slider>(currentOverGo);
    }

    /*
      If hover click is enabled, this method can disable it temporarily.
     */
    protected virtual bool DisableHoverClick(TransformEventData data) {
      return false;
    }

    /*
      Process UI input event using the screen coordinates of the
      pointer's position.
    */
    public override void Process() {
      foreach (int pointerId in m_PointerData.Keys) {
        PointerEventData eventData;
        bool newData = GetPointerData(pointerId, out eventData, false);
        newData &= true; // XXX remove warning
        TransformEventData eData = (TransformEventData) eventData;
        // We only process if we have a pointer.
        if (eData.pointer == null)
          continue;
        var screenPosition =
          Camera.main.WorldToScreenPoint(eData.pointer.position);

        Vector2 newPosition = new Vector2(screenPosition.x, screenPosition.y);
        eventData.delta = newPosition - eventData.position;
        eventData.position = newPosition;
        //eventData.pointerPress = null;
    
        eventSystem.RaycastAll(eventData, m_RaycastResultCache);

        var raycast = FindFirstRaycast(m_RaycastResultCache);
        eventData.pointerCurrentRaycast = raycast;
        m_RaycastResultCache.Clear();
        var currentOverGo = eventData.pointerCurrentRaycast.gameObject;

        if (enableHoverClick && !DisableHoverClick(eData)) {
          // If we enter a thing that is clickable, we wait, then draw
          // the animation.  Once the animation finishes, we click it.
          if (eventData.pointerEnter == currentOverGo
              && eData.timeSinceEntered > eData.timeSinceClicked
              && !eventData.dragging
              && IsClickable(currentOverGo)) {
            //print("1");
            // We're hovering over the same item.
            float enteredDuration = Time.unscaledTime - eData.timeSinceEntered;
            if (enteredDuration < preProgress) {
              //print("2");
              // We don't show the progress immediately; we wait a little while.
              eData.SetProgress(0f);
            } else {
              //print("3");
              // Let's show the current progress.
              float progress = (enteredDuration - preProgress)/timeToFill;
              eData.SetProgress(progress); 
              if (progress >= 1f) {
                //print("4");
                // Click.
                if (enableHoverDrag && IsDraggable(currentOverGo)) {
                  if (!eventData.dragging) {
                    // Press down.
                    ProcessGesturePress(eventData,
                                        true,
                                        false);
                    eventData.dragging = true;
                  } else {
                    // Release click.
                    Assert.IsNotNull(eventData);
                    ProcessGesturePress(eventData,
                                        false,
                                        true);
                  } 
                } else {
                  ProcessGesturePress(eventData,
                                      true,
                                      true);
                  // This is probably wrong.
                  eventData.selectedObject = null;
                }
                eData.timeSinceClicked = Time.unscaledTime;
                // Reset.
                eData.SetProgress(0f);
                // Whew. These things are hard to manage.
              
                //eventData.pointerEnter = null;
              }
            }
          } else if (eventData.pointerEnter != currentOverGo) {
            if (eventData.dragging
                && IsClickable(currentOverGo)
                // Make sure CurrentOverGo is not the pointerDrag
                // and that it's not a child of the pointerDrag.
                && eventData.pointerDrag != currentOverGo
                && !currentOverGo.transform.IsChildOf(eventData.pointerDrag.transform)) {
              // Release!
              ProcessGesturePress(eventData,
                                  false,
                                  true);
              //print("over other thing and dragging " + currentOverGo);
              //Debug.Log("Hi", currentOverGo);
            }
            //print("5");
            // We're over something but we don't think it's clickable.
            eData.SetProgress(0f);
            eData.timeSinceEntered = Time.unscaledTime;
          }
        }

        if (shouldClick.Count != 0
            && shouldClick.Contains(eventData.pointerId)) {
          ProcessGesturePress(eventData, true, true);
          ((TransformEventData) eventData).timeSinceClicked = Time.unscaledTime;
          eData.SetProgress(0f);
          shouldClick.Remove(eventData.pointerId);
        }
        ProcessMove(eventData);
        if (enableHoverDrag)
          ProcessDrag(eventData);
      }
    }

    // Derivative of ProcessTouchPress
    /*
      Pressed a button? or Released a button?  So a click is simulated
      with pressed and released.
    */
    protected void ProcessGesturePress(PointerEventData pointerEvent,
                                       bool pressed,
                                       bool released)
    {
      var currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;

      // PointerDown notification
      if (pressed)
      {
        pointerEvent.eligibleForClick = true;
        pointerEvent.delta = Vector2.zero;
        pointerEvent.dragging = false;
        pointerEvent.useDragThreshold = true;
        pointerEvent.pressPosition = pointerEvent.position;
        pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;

        DeselectIfSelectionChanged(currentOverGo, pointerEvent);

        if (pointerEvent.pointerEnter != currentOverGo)
        {
          // send a pointer enter to the touched element if it isn't the one to select...
          HandlePointerExitAndEnter(pointerEvent, currentOverGo);
          pointerEvent.pointerEnter = currentOverGo;
        }

        // search for the control that will receive the press
        // if we can't find a press handler set the press
        // handler to be what would receive a click.
        var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);

        // didnt find a press handler... search for a click handler
        if (newPressed == null)
          newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

        //Debug.Log("Pressed: " + newPressed);

        float time = Time.unscaledTime;

        if (newPressed == pointerEvent.lastPress)
        {
          var diffTime = time - pointerEvent.clickTime;
          if (diffTime < 0.3f)
            ++pointerEvent.clickCount;
          else
            pointerEvent.clickCount = 1;

          pointerEvent.clickTime = time;
        }
        else
        {
          pointerEvent.clickCount = 1;
        }

        pointerEvent.pointerPress = newPressed;
        pointerEvent.rawPointerPress = currentOverGo;

        pointerEvent.clickTime = time;

        // Save the drag handler as well
        pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

        if (pointerEvent.pointerDrag != null)
          ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
      }

      // PointerUp notification
      if (released)
      {
        //Debug.Log("Executing pressup on: ");// + pointer.pointerPress);
        ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

        // Debug.Log("KeyCode: " + pointer.eventData.keyCode);

        // see if we mouse up on the same element that we clicked on...
        var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

        // PointerClick and Drop events
        if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
        {
          ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
        }
        else if (pointerEvent.pointerDrag != null)
        {
          ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
        }

        pointerEvent.eligibleForClick = false;
        pointerEvent.pointerPress = null;
        pointerEvent.rawPointerPress = null;

        if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
          ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

        pointerEvent.dragging = false;
        pointerEvent.pointerDrag = null;

        if (pointerEvent.pointerDrag != null)
          ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);

        pointerEvent.pointerDrag = null;

        // send exit events as we need to simulate this on touch up on touch device
        ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
        pointerEvent.pointerEnter = null;
      }
    }

  }
}
