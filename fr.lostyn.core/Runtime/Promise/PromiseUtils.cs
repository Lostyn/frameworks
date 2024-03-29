﻿using System.Threading;
using UnityEngine;

public static class PromiseUtils
{
    public static IPromise WaitFor(float seconds) {
        var promise = new Promise();
        ThreadingBridge.ExecuteThreaded( () => {
            Thread.Sleep(Mathf.RoundToInt(seconds * 1000));
            ThreadingBridge.Dispatch(promise.Resolve);
        });
        return promise;
    }
}