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
    #region range & explanation
    [Range(0f, 1f)]
    //0 = no movement
    //1 = 100% movement
    #endregion
    [SerializeField] private float walkSpeedModif; //How much slower is walking

    [Header("Jumping")]
    [SerializeField] private float jumpStrength; //How far u go up
                                                  
    [Header("Forces of Nature")]
    #region range & explanation
    [Range(0f, 100f)] 
    //<10 = slippery.
    // 10 = perfect control on ground.
    //>10 = friction overtakes traction and you move slower
    #endregion 
    [SerializeField] private float phTractionGroundPlane; //friction of ground
    [SerializeField] private float phForceNormalGroundPlane; //How hard is the ground
    #region range & explanation
    [Range(0f, 100f)] 
    // 0 = no control, full preservation of momentum.
    // 1 = allows to correct to 50%.
    // 2 = full error correction in air.
    //>2 = overcorrection, you can move further back than from where you started.
    //10-100 = friction surpasses traction and you start slowing down.
    #endregion 
    [SerializeField] private float tractionAir; //how much movement control you have in air. 
    [SerializeField] private float dragAir; //friction of air
    [SerializeField] private float forceVertGravity; //How gravity acting on you
    [SerializeField] private float forceVertPotential; //How hard gravity storing on you
    #endregion 

    #region Result Variables
    [Header("\nOutput")]

    [Header("Movement")]
    [SerializeField] private Vector3 transformVelocity; //What way u movin
    [SerializeField] private Vector3 finalVelocity; //What way u movin in the end
    [SerializeField] private float jumpVelocity; // what speed the jump be

    [Header("Forces of Nature")]
    [SerializeField] private float traction; //friction of ground
    [SerializeField] private float forceVert; //Current vertical forces
    [SerializeField] private float forceNormal; //current normal forces
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
        if (MovementEvaluator.IsGrounded(charController) && finalVelocity.y <= transformVelocity.y) 
        {
            //resetting and presetting all necessary values
            traction = phTractionGroundPlane;
            forceNormal = phForceNormalGroundPlane;
            forceVert = -forceVertPotential;
            jumpVelocity = jumpStrength - phForceNormalGroundPlane;
        }        
        else if (!MovementEvaluator.IsGrounded(charController))
        {
            forceVert += forceVertGravity * Time.deltaTime;
            traction = tractionAir;
            jumpVelocity = 0;
        }

        //Jump
        if (Input.GetKeyDown(inputJump))
        {
            Jump();
        }

        //Movement WASD
        var velocityInputX = transformNormal.right * Input.GetAxisRaw("Horizontal");
        var velocityInputZ = transformNormal.forward * Input.GetAxisRaw("Vertical");
        traction = Mathf.Clamp(traction, 0, 100);
        var movementSpeed = runSpeed * traction;
        var forceDrag = 1f - (dragAir + traction) * .01f;

        transformVelocity += (velocityInputX + velocityInputZ).normalized * movementSpeed * Time.deltaTime;
        transformVelocity *= forceDrag;
        finalVelocity = transformVelocity;
        finalVelocity.y = transformVelocity.y + forceVert + forceNormal;

        charController.Move((finalVelocity) * Time.deltaTime);
    }

    public void Jump() 
    {
        forceNormal += jumpVelocity;
    }
}