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
    public bool interact_Input;
    //public bool settings_Input; //For later

    [Header("Dialogue")]
    public bool dialogueActive = false;

    [Header("Name Entry")]
    public bool nameEntryActive = false;

    private MenuNavigation menuNavigation;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        menuNavigation = FindObjectOfType<MenuNavigation>();
    }

    //Calling and setting the variables for the player input action map controls to work
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
            playerControls.PlayerActions.Interact.performed += i => interact_Input = true;

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
        //If the player is currently entering their name, don't allow pause/inventory/journal/gameplay controls.
        if (nameEntryActive)
        {
            ClearGameplayInput();

            pause_Input = false;
            inventory_Input = false;
            journal_Input = false;
            jump_Input = false;
            interact_Input = false;

            return;
        }

        HandleMenuInputs();

        //Menus take priority over normal gameplay input.
        if (UIManager.Instance.IsMenuOpen)
        {
            ClearGameplayInput();
            return;
        }

        //Dialogue takes priority over normal gameplay input.
        if (dialogueActive)
        {
            ClearGameplayInput();

            playerLocomotion.isSprinting = false;

            animatorManager.UpdateAnimatorValues(0f,0f,false);

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

    private void HandleMenuInputs()
    {
        if (pause_Input)
        {
            pause_Input = false;

            if (!UIManager.Instance.IsMenuOpen)
            {
                UIManager.Instance.TogglePause();
            }
            else if (UIManager.Instance.CurrentMenu == UIManager.MenuType.Pause)
            {
                UIManager.Instance.HandlePauseEscape();
            }
            else
            {
                UIManager.Instance.CloseMenus();
            }
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

    }

    //Locks or unlocks normal player gameplay input during dialogue.Yarn Spinner's own dialogue input remains available because we're not disabling the Unity Input System.
    public void SetDialogueActive(bool active)
    {
        dialogueActive = active;

        if (active)
        {
            ClearGameplayInput();

            playerLocomotion.isSprinting = false;

            jump_Input = false;
            interact_Input = false;

            return;
        }
    }

    //Clears movement/camera values currently being used by gameplay.
    private void ClearGameplayInput()
    {
        movementInput = Vector2.zero;
        cameraInput = Vector2.zero;

        horizontalInput = 0f;
        verticalInput = 0f;

        cameraInputX = 0f;
        cameraInputY = 0f;

        moveAmount = 0f;

        b_Input = false;
    }

    public void SetNameEntryActive(bool active)
    {
        nameEntryActive = active;

        if (active)
        {
            ClearGameplayInput();

            pause_Input = false;
            inventory_Input = false;
            journal_Input = false;
            jump_Input = false;
            interact_Input = false;

            playerLocomotion.isSprinting = false;
        }
    }
}
