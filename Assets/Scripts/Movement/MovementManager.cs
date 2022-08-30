using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public Vector3 velocity;
    [HideInInspector] public CharacterController controller;

    [HideInInspector] public GroundedState groundedState;
    [HideInInspector] public AirbornState airbornState;
    [HideInInspector] public CrouchingState crouchingState;
    private MovementStateMachine stateMachine;

    [Header("Objects Needed")]


    [Header("World Settings")]
    public float gravity = -19.62f;
    public float airDrag = 10;

    [Header("Player Settings")]
    public float speed;
    public float airbornSpeed;
    public float runSpeed;
    public float slideSpeed;
    public float crouchSpeed;
    public float wallSpeedMultiplier;
    public float jumpHeight;
    public float jumpAmount;
    public float dashAmount; 

    //Vectors for Wallrunning
    [HideInInspector] public Vector3 lastWallPosition;
    [HideInInspector] public Vector3 lastWallNormal;
    [HideInInspector] public Vector3 alongWall;
    [HideInInspector] public Vector3[] directions;

    //Wallrunning hits 
    [HideInInspector] public RaycastHit[] hits;

    //Keep track
    [HideInInspector] public float currentJumps;
    [HideInInspector] public float currentDashes;
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float elapsedTimeSinceJump = 0;
    [HideInInspector] public float elapsedTimeSinceWallAttach = 0;
    [HideInInspector] public float elapsedTimeSinceWallDetatch = 0;
    [HideInInspector] public float maxAngleRoll = 20;
    [HideInInspector] public float cameraTransitionDuration = 1;
    [HideInInspector] public float normalizedAngleThreshold = 0.1f;

    void Start() {
        controller = GetComponent<CharacterController>();

        groundedState = new();
        groundedState.owner = this;
        airbornState = new();
        airbornState.owner = this;
        crouchingState = new();
        crouchingState.owner = this;

        stateMachine = new();
        stateMachine.ChangeState(groundedState);
    }

    void Update() {
        stateMachine.OnUpdate();

        controller.Move(velocity * Time.deltaTime);
    }

    public void ChangeState(MoveState state) {
        stateMachine.ChangeState(state);
    }
}