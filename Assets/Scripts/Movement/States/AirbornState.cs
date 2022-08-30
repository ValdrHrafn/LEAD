using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbornState : MoveState {
    public override void OnEnter() {
        owner.elapsedTimeSinceJump = 0;
        owner.currentJumps = owner.jumpAmount - 1;
    }

    public override void OnExit() {

    }

    public override void OnUpdate() {
        owner.elapsedTimeSinceJump += Time.deltaTime;

        if (owner.velocity.x > 0) {
            owner.velocity.x -= owner.airDrag * Time.deltaTime;
        }
        else if (owner.velocity.x < 0) {
            owner.velocity.x += owner.airDrag * Time.deltaTime;
        }
        if (owner.velocity.z > 0) {
            owner.velocity.z -= owner.airDrag * Time.deltaTime;
        }
        else if (owner.velocity.z < 0) {
            owner.velocity.z += owner.airDrag * Time.deltaTime;
        }

        //inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        owner.velocity += (owner.transform.right * x + owner.transform.forward * z) * owner.airbornSpeed;

        owner.velocity.y += owner.gravity * Time.deltaTime;

        if (owner.controller.isGrounded) {
            owner.ChangeState(owner.groundedState);
        }
    }
}