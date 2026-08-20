using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Falling")]
    public float inAirTimer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public bool isFalling;
    public bool canMove = true;

    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 5f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 12f;

    [Header("Jump")]
    public float jumpHeight = 3f;
    public float gravityIntensity = -15f;

    //Call to set variables on wake
    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();

        cameraObject = Camera.main.transform;
    }

    //Handle detections and movement for player
    public void HandleAllMovement() 
    {
        //Ground detection should keep running.
        HandleFallingAndLanding();

        //Stop gameplay movement while a menu is open.
        if (UIManager.Instance.IsMenuOpen)
        {
            StopPlayer();
            return;
        }

        //Stop gameplay movement while dialogue is active.
        if (!canMove)
        {
            StopPlayer();
            return;
        }

        if (playerManager.isInteracting)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement() 
    {
        //Movement check
        if (!canMove)
        {
            return;
        }

        //Camera movement
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        //Check for the player sprinting and adjust as needed
        if (isSprinting)
        {
            moveDirection *= sprintingSpeed;
        }
        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection *= runningSpeed;
            }
            else
            {
                moveDirection *= walkingSpeed;
            }
        }

        Vector3 movementVelocity = moveDirection;

        // Preserve vertical Rigidbody velocity.
        movementVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        //If player movement has been disabled, such as during dialogue,don't allow the player to rotate.
        if (!canMove)
        {
            return;
        }

        //Stores the direction that the player should face.
        Vector3 targetDirection = Vector3.zero;

        //Use the camera's forward direction and the player's forward/backward input to calculate the forward direction.
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        
        // Add the camera's right direction and the player's left/right input to calculate the final movement direction.
        targetDirection += cameraObject.right * inputManager.horizontalInput;

        //Make the direction vector have a length of 1. This prevents diagonal input from affecting the rotation calculation.
        targetDirection.Normalize();

        // Remove any vertical rotation.The player should only rotate left/right, not tilt up/down based on the camera angle.
        targetDirection.y = 0f;

        //If the player isn't pressing a movement key, keep them facing their current direction.
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        //Convert the direction we calculated into a rotation.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        //Smoothly rotate from the player's current rotation towards the target rotation. Time.deltaTime makes the rotation independent of frame rate
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);

        //Only rotate the character while they are touching the ground. This prevents movement input from rotating the player in mid-air.
        if (isGrounded)
        {
            transform.rotation = playerRotation;
        }
    }

    private void HandleFallingAndLanding()
    {
        //Ground check to prevent null reference exception
        if (groundCheck == null)
        {
            return;
        }

        //Create an invisible sphere at the GroundCheck object's position. If this sphere overlaps an object on the Ground Layer, Unity considers ground to be underneath the player.
        bool groundDetected = Physics.CheckSphere(groundCheck.position,groundCheckRadius,groundLayer,QueryTriggerInteraction.Ignore);

        //JUMP CHECK
        if (isJumping)
        {
            //While jumping upwards, the player should not be considered grounded.
            isGrounded = false;

            //Reached the peak and started falling.
            if (playerRigidbody.velocity.y <= 0f)
            {
                isJumping = false;
                isFalling = true;

                animatorManager.PlayAnimation("Falling");
            }

            return;
        }

        //FALLING / WALKED OFF EDGE
        if (!groundDetected)
        {
            isGrounded = false;

            if (!isFalling)
            {
                //Enter falling state and track how long in air
                isFalling = true;
                inAirTimer = 0f;

                animatorManager.PlayAnimation("Falling");
            }

            inAirTimer += Time.deltaTime;

            return;
        }

        //GROUNDED / LANDED
        if (groundDetected && playerRigidbody.velocity.y <= 0.1f)
        {
            if (isFalling)
            {
                //Stop any leftover vertical motion so the player doesn't bounce back up after touching the ground.
                Vector3 velocity = playerRigidbody.velocity;
                velocity.y = 0f;
                playerRigidbody.velocity = velocity;

                animatorManager.PlayAnimation("Land");
            }

            isGrounded = true;
            isJumping = false;
            isFalling = false;

            inAirTimer = 0f;
        }
    }

    public void HandleJumping()
    {
        //Checking variables to see if the player has jumped based on certain conditions
        if (!canMove)
        {
            return;
        }

        if (!isGrounded)
        {
            return;
        }

        isGrounded = false;
        isJumping = true;
        isFalling = false;

        inAirTimer = 0f;

        //Play the Jump animation.
        animatorManager.PlayAnimation("Jump");

        //Calculate the upward velocity required to reach the specified jumpHeight using the gravity value set for the player.
        float jumpingVelocity =Mathf.Sqrt( -2f * gravityIntensity * jumpHeight);

        //Get the player's current velocity so we can preserve their existing horizontal movement.
        Vector3 velocity = playerRigidbody.velocity;

        //Push player upwards
        velocity.y = jumpingVelocity;

        //Apply veloctiy to player body
        playerRigidbody.velocity = velocity;
    }

    // Called when dialogue starts/ends.
    public void SetMovementEnabled(bool enabled)
    {
        //Store whether normal player movement is currently allowed.
        canMove = enabled;

        //If movement is disabled
        if (!enabled)
        {
            // Stop horizontal movement but keep vertical physics.
            if (!playerRigidbody.isKinematic)
            {
                playerRigidbody.velocity = new Vector3(0f,playerRigidbody.velocity.y, 0f);

                playerRigidbody.angularVelocity =Vector3.zero;
            }

            moveDirection = Vector3.zero;
            isSprinting = false;

            animatorManager.SetDialogueLocked(true);
        }
        else
        {
            moveDirection = Vector3.zero;
            isSprinting = false;

            animatorManager.SetDialogueLocked(false);
        }
    }

    private void StopPlayer()
    {
        // Safety check in case something else ever makes the Rigidbody kinematic.
        if (playerRigidbody.isKinematic)
        {
            return;
        }

        //Stop horizontal movement only. Keep Y velocity so gravity / falling still works.
        playerRigidbody.velocity = new Vector3(0f,playerRigidbody.velocity.y,0f);

        playerRigidbody.angularVelocity = Vector3.zero;

        moveDirection = Vector3.zero;
        isSprinting = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        // Draw a wireframe sphere in the Scene view showing exactly where the player's ground detection sphere is located. Normally a visual debug tool 
        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}
