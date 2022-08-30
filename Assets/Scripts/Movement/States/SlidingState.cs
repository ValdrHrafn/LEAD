using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : MoveState {
    public override void OnEnter() {
        owner.elapsedTimeSinceJump = 0;
        owner.currentJumps = owner.jumpAmount - 1;
    }

    public override void OnExit() {

    }

    public override void OnUpdate() {

    }
}
