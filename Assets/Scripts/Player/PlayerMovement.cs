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
    [SerializeField] private float jumpStrength; //How far u go up

    [Header("Forces of Nature")]
    #region range & tooltip
    [Range(0f, 100f)]
    [Tooltip(">10 = friction overtakes traction and you move slower.\n10 = perfect control on ground.\n<10 = slippery.")]
    #endregion
    public float phTractionGroundPlane; //friction of ground
    #region range & tooltip
    [Range(0f, 100f)]
    [Tooltip(">10 = friction surpasses traction and you start slowing down.\n>2 = overcorrection, you can move further back than from where you started.\n2 = full error correction in air.\n0 = no control, full preservation of momentum.")]
    #endregion
    public float phTractionAir; //how much movement control you have in air.
    public float phForceNormalGroundPlane; //How hard is the ground

    [SerializeField] private float forceVertPotential; //How hard gravity storing on you
    [SerializeField] private float forceVertGravity; //How gravity acting on you
    #endregion 

    #region Result Variables
    [Header("Movement")]
    private float movementSpeed; //What speed is the movement
    private float jumpVelocity; //What velocity does the Jump add

    private Vector2 moveInput; //translated WASD to XY
    private Vector3 moveDirection; //translated WASD to XYZ
    private Vector3 moveVelocity; //where are you moving and how fast initially. Without Y
    private Vector3 finalVelocity; //where are you moving and how fast in the end. With Y forces

    [Header("Forces of Nature")]
    private float traction; //The inherited traction on whatever surface you are on
    private float friction; //The inherited friction of ground
    private float forceVert; //Current vertical forces
    private float forceNormal; //Current normal forces
    private float forceNormalPush; //Any opposite forces pushed into the ground
    #endregion

    void Start()
    {
        charController = GetComponent<CharacterController>(); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Gravity and normal forces
        if (IsGrounded(charController) && finalVelocity.y <= moveVelocity.y)
        {
            //resetting and presetting all necessary values
            traction = phTractionGroundPlane;
            
            forceVert = forceVertPotential;

            forceNormal = phForceNormalGroundPlane + forceNormalPush;
            jumpVelocity = jumpStrength - phForceNormalGroundPlane - forceVertPotential;
        }
        else if (!IsGrounded(charController))
        {
            traction = phTractionAir;

            forceVert += forceVertGravity * Time.deltaTime;

            forceNormalPush = 0;
        }

        //Take WASD Turn XY to XZ
        moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
        //Take the normal of the ground under you and set it straight
        transformNormal.rotation = Quaternion.FromToRotation(transformNormal.up, GroundNormal(charController)) * transformNormal.rotation;
        var euler = transformNormal.localEulerAngles;
        euler.y = 0;
        transformNormal.localEulerAngles = euler;
        //Change Direction along with the normal
        moveDirection = transformNormal.TransformDirection(moveDirection);

        MovePlayer();
    }

    public bool IsGrounded(CharacterController controller) //Check for Grounded state
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        Vector3 origin = controller.transform.position + new Vector3(0, .45f, 0);

        return Physics.CheckSphere(origin, .49f, groundLayer);
    }

    public Vector3 GroundNormal(CharacterController controller) //Take Normal of the Ground
    {
        Vector3 origin = controller.transform.position + new Vector3(0, .1f, 0);
        Vector3 normal;
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        Physics.Raycast(origin, Vector3.down, out var hit, 1, groundLayer);
        normal = IsGrounded(controller) ? hit.normal : Vector3.up;

        Debug.DrawRay(origin, Vector3.down, Color.red, 1);
        return normal;
    }

    public void GetInputMove(InputAction.CallbackContext context) //Take WASD
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context) //Take Input and perform Jump
    {
        if(context.performed)
        {
            forceNormalPush = jumpVelocity;
        }
    }

    public void MovePlayer() //Do everything to move the player after receiving proper inputs
    {
        //Prepare variables for use
        movementSpeed = runSpeed * traction;
        friction = 1f - traction * .01f;

        //Add everything into the sauce
        moveVelocity += moveDirection * (movementSpeed * Time.deltaTime);
        moveVelocity *= friction;
        finalVelocity = moveVelocity;
        finalVelocity.y = moveVelocity.y + forceVert + forceNormal;

        charController.Move(finalVelocity * Time.deltaTime);
    }
}