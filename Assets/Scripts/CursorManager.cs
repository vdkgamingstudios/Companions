using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
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
}
