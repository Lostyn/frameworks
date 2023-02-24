using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BehaviourTreeRunner : MonoBehaviour
{
    public event Action OnCompleted;

    public BehaviourTree tree;
    public Blackboard blackboard;

    bool started = false;
    Node.State state = Node.State.Idle;

    public void Bind() {
        
    }

    public void StartTree() {
        tree = tree.Clone();
        tree.Bind(blackboard);
        tree.Start();

        started = true;
    }

    void Update() {
        if (started) {
            state = tree.Update();

            if (state == Node.State.Completed) {
                OnCompleted?.Invoke();
                started = false;
            }
        }
    }
}