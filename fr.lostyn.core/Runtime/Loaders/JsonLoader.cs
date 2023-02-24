using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Hyperfiction.Core {

    public class JsonLoader
    {
        public static IPromise<T> LoadExternalJson<T>(string path) {
            Promise<T> promise = new Promise<T>();

            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path);
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
            ao.completed += (a) => {
                if (ao.webRequest.result != UnityWebRequest.Result.Success ) {
                    promise.Reject(new Exception($"[ExternalLoad path={path}] " + ao.webRequest.error) );
                } else {
                    promise.Resolve( JsonUtility.FromJson<T>(uwr.downloadHandler.text) );
                }
            }; 

            return promise;
        }   
    }

}