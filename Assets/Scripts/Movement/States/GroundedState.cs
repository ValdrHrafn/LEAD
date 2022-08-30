using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : MoveState {
    public override void OnEnter() {
        owner.velocity = new Vector3(0, -2f, 0);
        owner.currentDashes = owner.dashAmount;
    }

    public override void OnExit() {

    }

    public override void OnUpdate() {
        Vector3 velocity = Vector3.zero;

        //inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //move
        velocity += (owner.transform.right * x + owner.transform.forward * z) * owner.speed;
        
        //jump 
        if (Input.GetKeyDown(KeyCode.Space)) {
            velocity += new Vector3(0, Mathf.Sqrt(owner.jumpHeight * -2 * owner.gravity), 0);

            owner.ChangeState(owner.airbornState);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            owner.ChangeState(owner.crouchingState);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            owner.currentSpeed = owner.runSpeed;
        }
        if (z <= 0) {
            owner.runSpeed = owner.speed;
        }

        owner.velocity = velocity;
    }
}