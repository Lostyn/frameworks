using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    static Promise _promise;
    public static List<string> _scenesToLoad;

    /// <summary>
    /// Check if the given the scene is loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <returns></returns>
    public static bool IsSceneLoaded(string scene) {
        return SceneManager.GetSceneByName(scene).isLoaded;
    }

    /// <summary>
    /// Load all scenes in params async
    /// </summary>
    /// <param name="scenes"></param>
    /// <returns></returns>
    public static IPromise LoadScenesAsync(params string[] scenes) {
        List<IPromise> promises = new List<IPromise>();

        for(int i = 0; i < scenes.Length; i++) {
            if (!SceneManager.GetSceneByName(scenes[i]).isLoaded) {
                promises.Add( LoadSceneAsync(scenes[i], LoadSceneMode.Additive) );
            } else {
                Debug.LogWarning($"Scene {scenes[i]} is already loaded");
            }
        }

        if (promises.Count > 0)
            return Promise.All( promises );

        Debug.LogWarning($"All scenes seems to be loaded");
        return Promise.Resolved();
    }   

    /// <summary>
    /// Load the given scene
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static IPromise LoadSceneAsync(string scene, LoadSceneMode mode = LoadSceneMode.Single) {
        if (string.IsNullOrEmpty(scene)) {
            Debug.LogWarning("Scene name is empty or null");
            return Promise.Rejected( new System.Exception("Scene name is empty or null"));
        }

        var promise = new Promise();
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene, mode);
        ao.completed += (aop) => {
            promise.Resolve();
        };

        return promise;
    }
}