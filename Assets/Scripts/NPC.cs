using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour, IInteractable
{
    [Header("Yarn Dialogue")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string dialogueNode;

    public void Interact()
    {
        if (dialogueRunner == null)
        {
            Debug.LogError($"No Dialogue Runner assigned to {gameObject.name}.");
            return;
        }

        if (dialogueRunner.IsDialogueRunning)
        {
            return;
        }

        if (string.IsNullOrEmpty(dialogueNode))
        {
            Debug.LogError($"No dialogue node assigned to {gameObject.name}.");
            return;
        }

        Debug.Log($"Starting dialogue: {dialogueNode}");

        dialogueRunner.StartDialogue(dialogueNode);
    }

    public string GetInteractionText()
    {
        return "Talk";
    }
}
