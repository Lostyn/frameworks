using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// This class is used to do some work that will be done
/// on the main thread
/// </summary>
public class ThreadingBridge : MonoBehaviour
{
    public static GameObject threadingBridge;
    static Queue<Action> todo = new Queue<Action>();

    /// <summary>
    /// Initialize the threading bridge.
    /// This is called by Game logic class.
    /// </summary>
    public static void Initialize() {
        threadingBridge = new GameObject("_ThreadingBridge");
        threadingBridge.AddComponent<ThreadingBridge>();

        if (Application.isPlaying) {
            DontDestroyOnLoad(threadingBridge);
        }
    }

    /// <summary>
    /// Destroy the bridge
    /// </summary>
    public static void Destroy() {
        if (threadingBridge != null) {
            if (Application.isPlaying) {
                GameObject.Destroy(threadingBridge);
            } else {
                GameObject.DestroyImmediate(threadingBridge);
            }
        }
    }

    private void Update() {
        if (todo.Count > 0) {
            StartCoroutine( Dequeue() );
        }
    }

    IEnumerator Dequeue() {
        Action action = todo.Dequeue();
        yield return 0;
        action();
    }

    #region public interface 

    /// <summary>
    /// Enqueue some work that will be done on the main thread during the next update
    /// </summary>
    /// <param name="action">The action to execute</param>
    public static void Dispatch(Action action) {
        todo.Enqueue(action);
    }

    /// <summary>
    /// Enqueue some work to be executed in a separate thread
    /// </summary>
    /// <param name="action">The action to be executed</param>
    public static void ExecuteThreaded(Action action) {
        ThreadPool.QueueUserWorkItem( new WaitCallback((state) => {action();}) );
    }

    #endregion
}