using UnityEngine;

namespace seawisphunter.custompointer {
  
  /**
    Register a pointer.  

    This is most useful when a pointer object may enter into a scene
    during runtime (as happens with the Leap Motion hands).  Otherwise,
    it's probably best just to set the pointer and progress bar directly
    in the TransformPointerModule's editor window.
  */
  public static class CPUtil {
    public static T FindComponent<T>(string name) where T : class {
      var go = GameObject.Find(name);
      if (go == null) {
        Debug.Log("No '" + name + "' in scene. Please add one or manually set the " + typeof(T).Name + ".");
        return null;
      }
      T c = go.GetComponent<T>();
      if (c == null) {
        Debug.Log("'" + name + "' does not have a " + typeof(T).Name + " component.  Please add one.");
        return null;
      }
      return c;
    }
  }
}

