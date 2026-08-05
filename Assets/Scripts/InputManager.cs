using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool jump_Input;
    public bool pause_Input;
    public bool inventory_Input;
    public bool journal_Input;
    //public bool settings_Input; //For later

    private MenuNavigation menuNavigation;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        menuNavigation = FindObjectOfType<MenuNavigation>();
    }

    private void OnEnable()
    {
        if (playerControls == null) 
        { 
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            //Resets the value instead of being at the last value the key was at before being released
            playerControls.PlayerMovement.Movement.canceled += i => movementInput = Vector2.zero;
            playerControls.PlayerMovement.Camera.canceled += i => cameraInput = Vector2.zero;

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;
            playerControls.PlayerActions.Jump.performed += i => jump_Input = true;

            playerControls.Menus.Pause.performed += i => pause_Input = true;
            playerControls.Menus.Inventory.performed += i => inventory_Input = true;
            playerControls.Menus.Journal.performed += i => journal_Input = true;
            //playerControls.Menus.Setting.performed += i => settings_Input = true; //For later
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs() 
    {
        //HandlePauseInput();
        //HandleMenuInput();

        //if (PauseMenu.isPaused)
        //{
        //    movementInput = Vector2.zero;
        //    cameraInput = Vector2.zero;

        //    horizontalInput = 0;
        //    verticalInput = 0;
        //    moveAmount = 0;

        //    return;
        //}

        HandleMenuInputs();

        if (UIManager.Instance.IsMenuOpen)
        {
            movementInput = Vector2.zero;
            cameraInput = Vector2.zero;

            horizontalInput = 0;
            verticalInput = 0;
            moveAmount = 0;

            return;
        }

        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpingInput();
        //HandleActionInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputY = cameraInput.y;
        cameraInputX = cameraInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount,playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (b_Input && moveAmount > 0.5f) 
        { 
            playerLocomotion.isSprinting = true;
        }
        else 
        { 
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }
    }

    //private void HandlePauseInput()
    //{
    //    if (!pause_Input)
    //        return;

    //    pause_Input = false;

    //    PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();

    //    if (PauseMenu.isPaused)
    //        pauseMenu.Resume();
    //    else
    //        pauseMenu.Pause();
    //}

    //private void HandleMenuInput()
    //{
    //    if (inventory_Input)
    //    {
    //        inventory_Input = false;
    //        menuNavigation.ToggleInventory();
    //    }

    //    if (journal_Input)
    //    {
    //        journal_Input = false;
    //        menuNavigation.ToggleJournal();
    //    }
    //}

    private void HandleMenuInputs()
    {
        if (pause_Input)
        {
            pause_Input = false;
            UIManager.Instance.TogglePause();
        }

        if (inventory_Input)
        {
            inventory_Input = false;
            UIManager.Instance.ToggleInventory();
        }

        if (journal_Input)
        {
            journal_Input = false;
            UIManager.Instance.ToggleJournal();
        }

        //if (settings_Input)
        //{
        //    settings_Input = false;
        //    UIManager.Instance.ToggleSettings();
        //}
    }

    //private void HandleActionInput() 
    //{ 

    //}
}
