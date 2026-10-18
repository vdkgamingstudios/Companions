using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PortalInteractable : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionText = "Examine Portal";

    [Header("Yarn Spinner")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string dialogueNode = "Portal";

    [Header("Teleport Settings")]
    [SerializeField] private Transform teleportDestination;
    [SerializeField] private Transform player;
    [SerializeField] private bool matchDestinationRotation = true;

    public string GetInteractionText()
    {
        return interactionText;
    }

    public void Interact()
    {
        if (dialogueRunner == null)
        {
            Debug.LogWarning("Portal: Dialogue Runner is missing!");
            return;
        }

        if (dialogueRunner.IsDialogueRunning)
        {
            return;
        }

        Debug.Log("Starting Portal dialogue.");

        dialogueRunner.StartDialogue(dialogueNode);
    }

    public void TeleportPlayer()
    {
        if (teleportDestination == null || player == null)
        {
            Debug.LogWarning(
                "Portal is missing a destination or player reference."
            );
            return;
        }

        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;

            playerRb.position = teleportDestination.position;

            if (matchDestinationRotation)
            {
                playerRb.rotation = teleportDestination.rotation;
            }
        }
        else
        {
            player.position = teleportDestination.position;

            if (matchDestinationRotation)
            {
                player.rotation = teleportDestination.rotation;
            }
        }

        Physics.SyncTransforms();

        Debug.Log(
            "Teleported player to: " + teleportDestination.name
        );
    }
}
