using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    #region Input Variables
    private CharacterController charController;

    [Header("\nInput")]

    [Header("Movement Speed")]
    [SerializeField] private Transform transformNormal;
    [SerializeField] private float runSpeed; //Standard movement speed
    [SerializeField] private float sprintSpeedModifyer; //How much faster is run with empty hands
    #region range & tooltip
    [Range(0f, 1f)]
    [Tooltip("0 = no movement\n1 = 100% movement")]
    #endregion
    [SerializeField] private float walkSpeedModif; //How much slower is walking
    [SerializeField] private float jumpStrength; //How far u go up

    [Header("Forces of Nature")]
    #region range & tooltip
    [Range(0f, 100f)]
    [Tooltip("<10 = slippery.\n 10 = perfect control on ground.\n>10 = friction overtakes traction and you move slower")]
    #endregion
    [SerializeField] private float phTractionGroundPlane; //friction of ground
    [SerializeField] private float phForceNormalGroundPlane; //How hard is the ground
    #region range & tooltip
    [Range(0f, 100f)]
    [Tooltip(" 0 = no control, full preservation of momentum.\n 1 = allows to correct to 50%.\n 2 = full error correction in air.\n>2 = overcorrection, you can move further back than from where you started.\n10-100 = friction surpasses traction and you start slowing down.")]
    #endregion
    [SerializeField] private float tractionAir; //how much movement control you have in air. 
    [SerializeField] private float dragAir; //friction of air
    [SerializeField] private float forceVertGravity; //How gravity acting on you
    [SerializeField] private float forceVertPotential; //How hard gravity storing on you
    #endregion 

    #region Result Variables
    [Header("\nOutput")]

    [Header("Movement")]
    [SerializeField] private Vector2 moveInput;//buttons to make move
    [SerializeField] private Vector3 transformVelocity; //What way u movin
    [SerializeField] private Vector3 finalVelocity; //What way u movin in the end
    [SerializeField] private float jumpVelocity; // what speed the jump be

    [Header("Forces of Nature")]
    [SerializeField] private float traction; //friction of ground
    [SerializeField] private float forceVert; //Current vertical forces
    [SerializeField] private float forceNormal; //current normal forces
    [SerializeField] private float forceNormalPush;
    #endregion

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
       //Gravity and normal forces
        if (MovementEvaluator.IsGrounded(charController) && finalVelocity.y <= transformVelocity.y)
        {   
           //resetting and presetting all necessary values
            traction = phTractionGroundPlane;
            forceVert = -forceVertPotential;
            jumpVelocity = jumpStrength - phForceNormalGroundPlane;
            forceNormal = phForceNormalGroundPlane + forceNormalPush;
        }
        else if (!MovementEvaluator.IsGrounded(charController))
        {
            forceVert += forceVertGravity * Time.deltaTime;
            traction = tractionAir;
            forceNormalPush = 0;
        }

       //Take the normal of the ground under you and adjust Movement
        transformNormal.rotation = Quaternion.FromToRotation(transformNormal.up, MovementEvaluator.GroundNormal(charController)) * transformNormal.rotation;
        var euler = transformNormal.localEulerAngles;
        euler.y = 0;
        transformNormal.localEulerAngles = euler;

       //WASD
        var moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        moveDirection = transformNormal.TransformDirection(moveDirection);

       //resistance and acceleration calcs
        var friction = Mathf.Clamp(traction, 0, 100);
        var movementSpeed = runSpeed;
        var forceDrag = 1f - (friction) * 0.01f;

       //add everything into the sauce
        transformVelocity += moveDirection * movementSpeed * Time.deltaTime;
        transformVelocity *= forceDrag; 
        finalVelocity = transformVelocity;
        finalVelocity.y = transformVelocity.y + forceVert + forceNormal;

        charController.Move(finalVelocity * Time.deltaTime);
    }

    public void GetInputMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        forceNormalPush = jumpVelocity; 
    }
}