using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Hyperfiction.Core {

    /// <summary>
    /// Texture2D loader from external sources
    /// Async handle with promise
    /// </summary>
    public class TextureLoader
    {
        /// <summary>
        /// Load a texture from external sources (file, http, etc)
        /// Support any source supported by UnityWebResquest
        /// </summary>
        /// <param name="path">The uri of the texture</param>
        /// <returns>Texture2D</returns>
        public static IPromise<Texture2D> LoadExternalTexture(string path) {
            Promise<Texture2D> promise = new Promise<Texture2D>();
            UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path);
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();

            ao.completed += (a) => {
                if (ao.webRequest.result != UnityWebRequest.Result.Success ) {
                    promise.Reject(new Exception($"[ExternalLoad path={path}] " + ao.webRequest.error) );
                } else {
                    promise.Resolve( DownloadHandlerTexture.GetContent(ao.webRequest));
                }
                ao.webRequest.Dispose();
            }; 

            return promise;
        }   
    }

}