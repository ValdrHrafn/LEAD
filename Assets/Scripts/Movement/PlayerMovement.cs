using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Key Bindings")]
    [SerializeField] private KeyCode inputJump = KeyCode.Space;

    [SerializeField] private Transform transformNormal;

    #region Input Variables
    private CharacterController charController;

    [Header("\nInput")]

    [Header("Movement Speed")]
    [SerializeField] private float runSpeed; //Standard speed
    [SerializeField] private float sprintSpeedModifyer; //How much faster is run with empty hands
    [Range(0f, 1f)]
    [SerializeField] private float walkSpeedModif; //How much slower is walking

    [Header("Jumping")]
    [SerializeField] private float jumpStrength; //How far u go up
    [Range(0f, 1f)]
    [SerializeField] private float longJump; //How far u go up
    [Range(0f, 1f)]
    [SerializeField] private float longJumpEndAt; //at what speed it stop
                                                  
    [Header("Forces of Nature")]
    [SerializeField] private float PHFrictionGroundPlane; //friction of ground
    [SerializeField] private float frictionAir; //friction of air
    [SerializeField] private float airControl; //how much can you move in the air
    [SerializeField] private float forceVertGravity; //How gravity acting on you
    [SerializeField] private float forceVertPotential; //How hard gravity storing on you
    [SerializeField] private float forceNormalGround; //How hard is the ground
    [SerializeField] private float graceTime; //coyote time
    #endregion

    #region Output Variables
    [Header("\nOutput")]

    [Header("Movement")]
    [SerializeField] private float transformVert;
    [SerializeField] private Vector3 transformVelocity; //What way u movin
    [SerializeField] private Vector3 finalVelocity; //What way u movin in the end
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
        transformNormal.rotation = Quaternion.FromToRotation(transformNormal.up, MovementEvaluator.GroundNormal(charController)) * transformNormal.rotation;
        var euler = transformNormal.localEulerAngles;
        euler.y = 0;
        transformNormal.localEulerAngles = euler;

        //Gravity and normal forces
        if (MovementEvaluator.IsGrounded(charController) && finalVelocity.y <= 0) 
        {
            //resetting and presetting all necessary values
            graceCount = graceTime;
            frictionGround = PHFrictionGroundPlane;
            forceVert = 0;
            forceNormal = forceNormalGround;
            jumpVelocity = jumpStrength + forceNormal;
            longJumpEnd = -(jumpStrength + forceNormalGround) * longJumpEndAt + transformVelocity.y;
        }        
        else if (!MovementEvaluator.IsGrounded(charController))
        {
            if (Timer.NegTimer(ref graceCount) <= 0)
            {
                forceVert += forceVertGravity * Time.deltaTime;
                frictionGround = airControl;
            }
        }

        //Jump
        if ((Input.GetKey(inputJump) && finalVelocity.y > longJumpEnd) || (Input.GetKeyDown(inputJump)))
        {
            Jump();
        }

        //Movement WASD
        var velocityInputX = transformNormal.right * Input.GetAxisRaw("Horizontal");
        var velocityInputZ = transformNormal.forward * Input.GetAxisRaw("Vertical");

        var movementSpeed = runSpeed * frictionGround;
        var forceDrag = 1f - (frictionAir + frictionGround) * .01f;

        transformVelocity += (velocityInputX + velocityInputZ).normalized * movementSpeed * Time.deltaTime;
        transformVelocity *= forceDrag;
        finalVelocity.y = transformVelocity.y + forceVert + forceNormal;
        finalVelocity.x = transformVelocity.x;
        finalVelocity.z = transformVelocity.z;

        charController.Move((finalVelocity) * Time.deltaTime);
    }

    public void Jump() 
    {
        graceCount = 0;
        forceNormal = jumpVelocity;
        jumpVelocity += -forceVertGravity * longJump * Time.deltaTime;
    }
}