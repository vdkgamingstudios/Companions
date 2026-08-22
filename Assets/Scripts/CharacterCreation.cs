using System.Collections;
using TMPro;
using UnityEngine;
using Yarn.Unity;
//using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private PlayerUIManager playerUIManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private CursorManager cursorManager;

    [Header("Yarn")]
    [SerializeField] private DialogueRunner dialogueRunner;

    private bool nameSubmitted = false;

    private void Awake()
    {
        //Hide the name entry UI when the scene starts.
        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(false);
        }
    }

    //Yarn can call this using: <<enter_player_name GameManager>>. It needs the name of the game object to find it
    [YarnCommand("enter_player_name")]
    public IEnumerator EnterPlayerName()
    {
        nameSubmitted = false;

        //Block gameplay/menu input while typing.
        if (inputManager != null)
        {
            inputManager.SetNameEntryActive(true);
        }

        // Show and unlock the mouse cursor.
        if (cursorManager != null)
        {
            cursorManager.SetUIWithMouseCursor();
        }

        //Show the input panel.
        nameInputPanel.SetActive(true);

        if (GameManager.Instance != null &&
            !string.IsNullOrWhiteSpace(GameManager.Instance.playerName))
        {
            nameInput.text = GameManager.Instance.playerName;
        }
        else
        {
            nameInput.text = "";
        }

        //Focus the TMP input field automatically.
        nameInput.Select();
        nameInput.ActivateInputField();

        while (!nameSubmitted)
        {
            yield return null;
        }

        nameInput.DeactivateInputField();

        //Hide the name-entry UI.
        nameInputPanel.SetActive(false);

        //Give normal input back.
        if (inputManager != null)
        {
            inputManager.SetNameEntryActive(false);
        }
    }

    //Called by the Confirm button.
    public void ConfirmName()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        if (nameInput == null)
        {
            Debug.LogError("Name Input Field has not been assigned!");
            return;
        }

        string enteredName = nameInput.text.Trim();

        //Use the default name if nothing was entered.
        if (string.IsNullOrWhiteSpace(enteredName))
        {
            enteredName = "Asura";
        }

        //Store the name in Unity.
        GameManager.Instance.playerName = enteredName;

        //Refresh the player UI immediately.
        if (playerUIManager != null)
        {
            playerUIManager.UpdatePlayerUI();
        }

        //Store the name in Yarn so dialogue can use {$playerName}.
        if (dialogueRunner != null)
        {
            dialogueRunner.VariableStorage.SetValue(
                "$playerName",
                enteredName
            );
        }
        else
        {
            Debug.LogError("Dialogue Runner not assigned!");
            return;
        }

        Debug.Log("Player name set to: " + enteredName);

        //Allows EnterPlayerName() to finish,which lets the Yarn dialogue continue.
        nameSubmitted = true;
    }
}
