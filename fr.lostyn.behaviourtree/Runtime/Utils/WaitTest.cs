using System;
using UnityEngine;

[Serializable]
public abstract class WaitTest<T> {
    public bool active;
    public T value;
}

[Serializable]
public class IntWait : WaitTest<int> {
    public bool Test(int valueToTest) {
        if (!active) return true;

        return Mathf.Abs(valueToTest - value) <= 2;
    }
}

[Serializable]
public class BoolWait : WaitTest<bool> {
    public bool Test(bool valueToTest) {
        if (!active) return true;

        return value == valueToTest;
    }
}