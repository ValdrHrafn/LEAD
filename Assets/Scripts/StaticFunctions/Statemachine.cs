using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statemachine
{
    private readonly Stack<State> states = new();
    public delegate void State(Statemachine stateMachine, GameObject gameObject);

    public void Update(GameObject gameObject)
    {
        states.Peek()?.Invoke(this, gameObject);
    }

    public void PushState(State state)
    {
        states.Push(state);
    }

    public void PopState()
    {
        states.Pop();
    }
}