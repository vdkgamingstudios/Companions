using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    public Transform targetTransform; //Object the camera follows
    public Transform cameraPivot; //Object the camera uses to pivot (look up and down)
    public Transform cameraTransform; //Transform of the actual camera object located in the scene
    public LayerMask collisionLayers; //Layers we want our camera to collide with

    private float defaultPosition;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;

    [Header("Camera Collision")]
    public float cameraCollisionOffSet = 0.2f; //How much the camera will jump off of objects it is colliding with
    public float minimumCollisionOffSet = 0.2f;
    public float cameraCollisionRadius = 0.2f;
    public float cameraFollowSpeed = 0.2f;

    [Header("Camera Sensitivity")]
    public float cameraLookSpeed = 4500f;
    public float cameraPivotSpeed = 4400f;
    //public float cameraSmoothTime = 0.08f;

    //private Vector2 currentCameraInput;
    //private Vector2 cameraInputVelocity;

    [Header("Camera Control")]
    public bool canLook = true;

    [Header("Camera Angles")]
    public float lookAngle; //Camera looking up and down
    public float pivotAngle; //Camera looking left and right
    public float minimumPivotAngle = -35;
    public float maximumPivotAngle = 35;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();

        targetTransform = FindObjectOfType<PlayerManager>().transform;

        cameraTransform = Camera.main.transform;

        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement() 
    {
        // Don't allow camera movement while dialogue is active.
        if (!canLook)
        {
            return;
        }

        // Don't move camera while a menu is open.
        if (UIManager.Instance.IsMenuOpen)
        {
            return;
        }

        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        if (!canLook)
        {
            return;
        }

        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle += inputManager.cameraInputX * cameraLookSpeed * Time.deltaTime;

        pivotAngle -= inputManager.cameraInputY * cameraPivotSpeed * Time.deltaTime;

        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle,maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;

        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffSet); //targetPosition = targetPosition - (distance - cameraCollisionOffSet);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffSet) 
        {
            targetPosition = -minimumCollisionOffSet;  //targetPosition = targetPosition - minimumCollisionOffSet;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

    //Called when dialogue starts/ends.
    public void SetCameraEnabled(bool enabled)
    {
        canLook = enabled;

        if (!enabled)
        {
            //Make sure any existing input isn't immediately applied when the camera gets unlocked again.
            inputManager.cameraInputX = 0f;
            inputManager.cameraInputY = 0f;
        }
    }
}
