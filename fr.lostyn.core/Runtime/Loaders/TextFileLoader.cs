using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Hyperfiction.Core {

    public class TextFileLoader
    {
        public static IPromise<string> LoadExternalTextFile(string path, bool utf8 = false) {
            Promise<string> promise = new Promise<string>();

            UnityWebRequest uwr = UnityWebRequest.Get(path);
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();
            ao.completed += (a) => {
                if (ao.webRequest.result != UnityWebRequest.Result.Success ) {
                    promise.Reject(new Exception($"[TextFileLoader path={path}] " + ao.webRequest.error) );
                } else {
                    if (utf8) {
                        byte[] bytes = Encoding.Default.GetBytes( uwr.downloadHandler.text );
                        promise.Resolve( Encoding.UTF8.GetString(bytes));
                    } else {
                        promise.Resolve( uwr.downloadHandler.text );
                    }
                }
            }; 

            return promise;
        }   
    }

}