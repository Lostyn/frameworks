using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    #region Transform

    /// <summary>
    /// Destroy all gameobject under given transform
    /// </summary>
    /// <param name="trs"></param>
    public static void Clear(this Transform trs)
    {
        foreach( Transform child in trs)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static T AddOrGetComponent<T>(this Transform trs) where T : Component {
        T comp = trs.GetComponent<T>();
        if( comp != null )
            return comp;

        return trs.gameObject.AddComponent<T>();
    }

    public static Vector3 FlaternedForwar(this Transform trs) {
        Vector3 result = trs.forward;
        result.y = 0;
        return result;
    }

    /// <summary>
    /// Makes the given game objects children of the transform.
    /// </summary>
    /// <param name="transform">Parent transform.</param>
    /// <param name="children">Game objects to make children.</param>
    public static void AddChildren(this Transform transform, GameObject[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }
    
    /// <summary>
    /// Iterate over child of the given Transform parent
    /// </summary>
    /// <param name="transform">The parent</param>
    /// <param name="action">Action for each child</param>
    public static void ForEachChild(this Transform transform, Action<Transform> action) {
        foreach(Transform child in transform) {
            action?.Invoke(child);
        }
    }

    /// <summary>
    /// Makes the game objects of given components children of the transform.
    /// </summary>
    /// <param name="transform">Parent transform.</param>
    /// <param name="children">Components of game objects to make children.</param>
    public static void AddChildren(this Transform transform, Component[] children)
    {
        Array.ForEach(children, child => child.transform.parent = transform);
    }

    /// <summary>
    /// Sets the x component of the transform's position.
    /// </summary>
    /// <param name="x">Value of x.</param>
    public static void SetX( this Transform transform, float x )
    {
        transform.position = new Vector3( x, transform.position.y, transform.position.z );
    }

    /// <summary>
    /// Sets the y component of the transform's position.
    /// </summary>
    /// <param name="y">Value of y.</param>
    public static void SetY( this Transform transform, float y )
    {
        transform.position = new Vector3( transform.position.x, y, transform.position.z );
    }

    /// <summary>
    /// Sets the z component of the transform's position.
    /// </summary>
    /// <param name="z">Value of z.</param>
    public static void SetZ( this Transform transform, float z )
    {
        transform.position = new Vector3( transform.position.x, transform.position.y, z );
    }

    #endregion


    #region RectTransform

    /// <summary>
    /// Change the pivot of a RectTransform and take care of positiont o keep the actual world position
    /// </summary>
    /// <param name="rtrs">The RectTransform</param>
    /// <param name="pivot">the new pivot</param>
    public static void SetPivot(this RectTransform rtrs, Vector2 pivot) {
        Vector2 size = rtrs.rect.size;
        Vector2 deltaPivot = rtrs.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        rtrs.pivot = pivot;
        rtrs.localPosition -= deltaPosition;
    }

    #endregion
}
