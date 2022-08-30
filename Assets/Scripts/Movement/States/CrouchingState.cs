using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : MoveState
{
    public override void OnEnter() {
        owner.controller.height = 1;
        owner.controller.center = new Vector3(0, .5f, 0);
    }

    public override void OnExit() {
        owner.controller.height = 2;
        owner.controller.center = new Vector3(0, 1, 0);
    }

    public override void OnUpdate() {
        Vector3 velocity = Vector3.zero;

        //inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        velocity += (owner.transform.right * x + owner.transform.forward * z) * owner.crouchSpeed;

        //jump 
        if (Input.GetKeyDown(KeyCode.Space)) {
            velocity += new Vector3(0, Mathf.Sqrt(owner.jumpHeight * -2 * owner.gravity), 0);

            owner.ChangeState(owner.airbornState);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            owner.ChangeState(owner.groundedState);
        }

        owner.velocity = velocity;
    }
}