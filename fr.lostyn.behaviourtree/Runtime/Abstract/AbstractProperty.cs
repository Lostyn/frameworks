using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AbstractProperty
{
    [SerializeField] public string GUID = Guid.NewGuid().ToString();
    [SerializeField] private string name;
    [SerializeField] private Type type;
}

[Serializable]
public abstract class AbstractProperty<T>: AbstractProperty {
    private T value;
    public T Value {
        get => value;
        set => this.value = value;
    }
}