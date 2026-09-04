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
    [SerializeField] private TMP_Text namePromptText;

    [SerializeField] private PlayerUIManager playerUIManager;
    [SerializeField] private RelationshipUI relationshipUI;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private CursorManager cursorManager;

    [Header("Yarn")]
    [SerializeField] private DialogueRunner dialogueRunner;

    private bool nameSubmitted = false;

    //Tracks which name is currently being entered.
    private enum NameEntryType
    {
        Player,
        CharacterFL
    }

    private NameEntryType currentNameEntry;

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
        currentNameEntry = NameEntryType.Player;

        string existingName = "";

        if (GameManager.Instance != null)
        {
            existingName = GameManager.Instance.playerName;
        }

        yield return ShowNameEntry( "What is your name?",existingName);
    }

    [YarnCommand("enter_character_fl_name")]
    public IEnumerator EnterCharacterFLName()
    {
        currentNameEntry = NameEntryType.CharacterFL;

        string existingName = "";

        if (GameManager.Instance != null)
        {
            existingName = GameManager.Instance.characterFLName;
        }

        yield return ShowNameEntry(
            "What is her name?",
            existingName
        );
    }

    private IEnumerator ShowNameEntry(string prompt,string existingName)
    {
        nameSubmitted = false;

        //Stop gameplay/menu shortcuts while typing.
        if (inputManager != null)
        {
            inputManager.SetNameEntryActive(true);
        }

        //Show and unlock cursor for the input box.
        if (cursorManager != null)
        {
            cursorManager.SetUIWithMouseCursor();
        }

        //Show the name-entry UI.
        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(true);
        }

        //Change the prompt depending on whose name is being entered.
        if (namePromptText != null)
        {
            namePromptText.text = prompt;
        }

        //Fill in an existing name, if one exists.
        if (!string.IsNullOrWhiteSpace(existingName))
        {
            nameInput.text = existingName;
        }
        else
        {
            nameInput.text = "";
        }

        nameInput.Select();
        nameInput.ActivateInputField();

        //Pause Yarn here until ConfirmName() is pressed.
        while (!nameSubmitted)
        {
            yield return null;
        }

        nameInput.DeactivateInputField();

        //Hide the entry panel again.
        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(false);
        }

        //Return normal input after typing is finished.
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

        if (dialogueRunner == null)
        {
            Debug.LogError("Dialogue Runner not assigned!");
            return;
        }

        string enteredName = nameInput.text.Trim();

        switch (currentNameEntry)
        {
            case NameEntryType.Player:

                if (string.IsNullOrWhiteSpace(enteredName))
                {
                    enteredName = "Asura";
                }

                //Store player name in Unity.
                GameManager.Instance.playerName = enteredName;

                //Store player name in Yarn.
                dialogueRunner.VariableStorage.SetValue("$playerName",enteredName);

                //Refresh Player Stats UI.
                if (playerUIManager != null)
                {
                    playerUIManager.UpdatePlayerUI();
                }

                Debug.Log(
                    "Player name set to: " +
                    enteredName
                );

                break;

            case NameEntryType.CharacterFL:

                if (string.IsNullOrWhiteSpace(enteredName))
                {
                    enteredName = "Unknown";
                }

                //Store FL name in Unity.
                GameManager.Instance.characterFLName =enteredName;

                //Store FL name in Yarn.
                dialogueRunner.VariableStorage.SetValue("$characterFLName",enteredName);

                //Change the visible relationship name for Character One.
                if (RelationshipManager.Instance != null)
                {
                    RelationshipManager.Instance.SetDisplayName( "characterone",enteredName);
                }

                // Refresh Relationships menu.
                if (relationshipUI != null)
                {
                    relationshipUI.UpdateRelationshipUI();
                }

                Debug.Log(
                    "FL character name set to: " +
                    enteredName
                );

                break;
        }

        // Allows the Yarn command to finish.
        nameSubmitted = true;
    }
}
