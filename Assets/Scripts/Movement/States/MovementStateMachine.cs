using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStateMachine
{
    public MoveState currentMoveState;

    public void OnUpdate() {
        if(currentMoveState != null)
            currentMoveState.OnUpdate();
    }

    public void ChangeState(MoveState state) {
        if(currentMoveState == state) {
            Debug.LogError("Can't go into state it's already in");
            return;
        }

        if(currentMoveState != null)
            currentMoveState.OnExit();

        currentMoveState = state;
        currentMoveState.OnEnter();
    }
}