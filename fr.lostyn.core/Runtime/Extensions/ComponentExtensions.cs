using UnityEngine;

public static class Extensions {

    /// <summary>
    /// Tries to get a component of the given type on the GameObject of the given MonoBehaviour. If none exists adds a component to the GameObject. 
    /// </summary>
    /// <returns>The existing or added component.</returns>
    /// <param name="target">target.</param>
    /// <typeparam name="T">Component</typeparam>
    public static T AddComponentIfNotExists<T>(this MonoBehaviour target) where T : Component {
        return AddComponentIfNotExists<T>(target.gameObject);
    }

    /// <summary>
    ///  Tries to get a component of the given type on the given GameObject. If none exists adds a component to the GameObject. 
    /// </summary>
    /// <returns>The existing or added component.</returns>
    /// <param name="target">target.</param>
    /// <typeparam name="T">Component</typeparam>
    public static T AddComponentIfNotExists<T>(this GameObject target) where T : Component {
        T comp = target.GetComponent<T>();
        if(comp != null) {
            return comp;
        }

        return target.AddComponent<T>();
    }

    /// <summary>
    /// Attaches a component to the given component's game object.
    /// </summary>
    /// <param name="component">Component.</param>
    /// <returns>Newly attached component.</returns>
    public static T AddComponent<T>(this Component component) where T : Component
    {
        return component.gameObject.AddComponent<T>();
    }

    /// <summary>
    /// Gets a component attached to the given component's game object.
    /// If one isn't found, a new one is attached and returned.
    /// </summary>
    /// <param name="component">Component.</param>
    /// <returns>Previously or newly attached component.</returns>
    public static T GetOrAddComponent<T>(this Component component) where T : Component
    {
        return component.GetComponent<T>() ?? component.AddComponent<T>();
    }

    /// <summary>
    /// Checks whether a component's game object has a component of type T attached.
    /// </summary>
    /// <param name="component">Component.</param>
    /// <returns>True when component is attached.</returns>
    public static bool HasComponent<T>(this Component component) where T : Component
    {
        return component.GetComponent<T>() != null;
    }
    
    /// <summary>
    /// Gets a component attached to the given component's game object.
    /// If one isn't found, search in parent.
    /// </summary>
    /// <param name="component">Component.</param>
    /// <returns>The component if one is found</returns>
    public static T GetComponentOrInParent<T>(this Component component) where T : Component => component.GetComponent<T>() ?? component.GetComponentInParent<T>();
}