using UnityEngine;
using UnityEngine.UI;

namespace seawisphunter.custompointer {
  
  /**
    Register a pointer.  

    This is most useful when a pointer object may enter into a scene
    during runtime (as happens with the Leap Motion hands).  Otherwise,
    it's probably best just to set the pointer and progress bar directly
    in the TransformPointerModule's editor window.
  */
  public class RegisterTransformPointer : MonoBehaviour {

    public int pointerId = 1;
    public TransformPointerModule module;
    // XXX This should probably be made into something generic to show
    // progress of which Image is merely one type.
    public Image progressBar;

    protected void OnEnable () {
      if (module == null) {
        module = CPUtil.FindComponent<TransformPointerModule>("EventSystem");
        if (module == null) {
          Debug.Log("Disabling.");
          enabled = false;
          return;
        }
      }
      pointerId = module.GetNextPointerId(pointerId);
      Register();
    }
    
    void OnDisable() {
      module.UnregisterPointer(pointerId);
    }

    /*
      Register the current transform with the current pointerId and progressBar.
     */
    protected virtual void Register() {
      module.RegisterPointer(pointerId, this.transform, progressBar);
    }

    public virtual void Click() {
      if (module == null)
        return;
      module.Click(pointerId);
    } 

    public virtual void OnDestroy() {
      if (module == null)
        return;
      module.UnregisterPointer(pointerId);
    }
  }

}
