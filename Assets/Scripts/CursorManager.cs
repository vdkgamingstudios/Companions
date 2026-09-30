using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //Tracks whether Yarn dialogue is currently active.
    private bool dialogueActive = false;

    //Lets other scripts check whether dialogue is active.
    public bool DialogueActive => dialogueActive;

    //Calls it when the game begins
    private void Start()
    {
        SetGameplayCursor();
    }

    //Locks the cursor
    public void SetGameplayCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Unlocks the cursor
    public void SetUIWithMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Call this when Yarn dialogue begins.
    public void DialogueStarted()
    {
        dialogueActive = true;

        // Keep the cursor available for dialogue choices.
        SetUIWithMouseCursor();
    }

    //Call this when Yarn dialogue finishes.
    public void DialogueEnded()
    {
        dialogueActive = false;

        //If another menu is open, it still needs the cursor.
        if (UIManager.Instance != null &&
            UIManager.Instance.IsMenuOpen)
        {
            SetUIWithMouseCursor();
        }
        else
        {
            SetGameplayCursor();
        }
    }

    //Called when another UI closes and we need to determine what the cursor should return to.
    public void RestoreCursorState()
    {
        //Dialogue is still happening, so keep the cursor visible for dialogue choices.
        if (dialogueActive)
        {
            SetUIWithMouseCursor();
            return;
        }

        //A menu is still open.
        if (UIManager.Instance != null &&
            UIManager.Instance.IsMenuOpen)
        {
            SetUIWithMouseCursor();
            return;
        }

        //Nothing needs the cursor, so return to gameplay.
        SetGameplayCursor();
    }
}
