using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Node
{
    public enum State {
        Idle,
        Waiting,
        Running,
        Completed,
        Canceled
    }

    public string Name => GetType().Name;
    public string guid = System.Guid.NewGuid().ToString();
    public State state = State.Idle;
    public bool canceled = false;
    public Vector2 position;
    public virtual Color NodeColor => Color.black;

    [SerializeReference]
    public List<Node> nexts = new List<Node>();
    public Blackboard blackboard;
    
    [NonSerialized] BehaviourTree _tree;
    public BehaviourTree tree {
        get => _tree;
        set => _tree = value;
    }

    public State Update() {
        if (state == State.Idle) {
            state = State.Running;
            OnStart();
        }

        if (canceled) state = State.Canceled;
        else state = OnUpdate();

        switch(state) {
            case State.Completed:
                OnStop();
                break;
            case State.Canceled:
                OnCancel();
                break;
        }

        return state;
    }

    public void Cancel() {
        canceled = true;
        Update();
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract void OnCancel();
    protected abstract State OnUpdate();
    public abstract List<Node> GetNexts();

#if UNITY_EDITOR
    public virtual void OnDataGUI() {  }
    public virtual void InjectForDuplicate(Node node) { }
#endif
}
