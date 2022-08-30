﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Key Bindings")]
    [SerializeField] private KeyCode inputJump = KeyCode.Space;
    [SerializeField] private KeyCode inputWalk = KeyCode.LeftControl;

    #region Input Variables
    private CharacterController charController;

    [Header("\nInput")]

    [Header("Movement Speed")]
    [SerializeField] private float runSpeed; //Standard speed
    [Range(0f, 1f)]
    [SerializeField] private float walkSpeedModif; //How much slower is walking

    [Header("Jumping")]
    [SerializeField] private float jumpStrength; //How far u go up
    [Range(0f, 1f)]
    [SerializeField] private float longJump; //How far u go up
    [SerializeField] private float longJumpEndVelocity; //how high up from start of jump arc when falling the long jump will stop  

    [Header("Forces of Nature")]
    [SerializeField] private float PHFrictionGroundPlane; //friction of ground
    [SerializeField] private float frictionAir; //friction of air
    [SerializeField] private float airControl; //how much can you move in the air
    [SerializeField] private float forceVertGravity; //How gravity acting on you
    [SerializeField] private float forceNormalGround; //How hard is the ground
    [SerializeField] private float graceTime; //coyote time
    #endregion

    #region Output Variables
    [Header("\nOutput")]

    [Header("Movement")]
    [SerializeField] private Vector3 velocity; //What way u movin in the end
    [SerializeField] private float jumpVelocity; // what speed the jump be
    [SerializeField] private float longJumpEnd; //until when you can keep longjumping

    [Header("Forces of Nature")]
    [SerializeField] private float frictionGround; //friction of ground
    [SerializeField] private float forceVert; //Current vertical forces
    [SerializeField] private float forceNormal; //current normal forces
    [SerializeField] private float graceCount; //coyote time
    #endregion

    void Start() 
    {
        charController = GetComponent<CharacterController>();
    }

    void Update() 
    {
        //Gravity and normal forces
        if (!MovementEvaluator.IsGrounded(gameObject)) 
        {
            if (Timer(ref graceCount) <= 0) 
            {
                forceVert += forceVertGravity * Time.deltaTime;
                frictionGround = airControl;
            }
        }
        else if (MovementEvaluator.IsGrounded(gameObject) && velocity.y <= 0) 
        {
            //resetting and presetting all necessary values
            graceCount = graceTime;
            frictionGround = PHFrictionGroundPlane;
            forceVert = 0;
            forceNormal = forceNormalGround - forceVert;
            jumpVelocity = jumpStrength + forceNormal;
            longJumpEnd = -(jumpStrength + forceNormalGround - longJumpEndVelocity);
        }

        //Jump
        if (Input.GetKey(inputJump) && velocity.y > longJumpEnd) 
        {
            Jump();
        }

        //Movement WASD
        var velocityInputHor = transform.right * Input.GetAxisRaw("Horizontal");
        var velocityInputVer = transform.forward * Input.GetAxisRaw("Vertical");

        var movementSpeed = runSpeed * frictionGround;
        var forceDrag = 1f - (frictionAir + frictionGround) * .01f;

        velocity += (velocityInputHor + velocityInputVer).normalized * movementSpeed * Time.deltaTime;
        velocity *= forceDrag;
        velocity.y = forceVert + forceNormal;

        charController.Move((velocity) * Time.deltaTime);
    }
    
    float Timer(ref float timer) 
    {
        if ((timer -= Time.deltaTime) <= 0)
        {
            return 0f;
        }

        return timer -= Time.deltaTime;
    }

    public void Jump() 
    {
        graceCount = 0;
        forceNormal = jumpVelocity;
        jumpVelocity += -forceVertGravity * longJump * Time.deltaTime;
    }
}