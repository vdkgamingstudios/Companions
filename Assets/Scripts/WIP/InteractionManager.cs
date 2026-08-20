using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TMP_Text interactionText;

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckForInteractable();

        if (currentInteractable != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E pressed - interacting!");

            currentInteractable.Interact();
        }
    }

    private void CheckForInteractable()
    {
        currentInteractable = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionRange,
            interactableLayer))
        {
            currentInteractable =
                hit.collider.GetComponentInParent<IInteractable>();

            if (currentInteractable != null)
            {
                ShowInteractionPrompt(
                    currentInteractable.GetInteractionText()
                );

                return;
            }
        }

        HideInteractionPrompt();
    }

    private void ShowInteractionPrompt(string text)
    {
        interactionUI.SetActive(true);
        interactionText.text = "E - " + text;
    }

    private void HideInteractionPrompt()
    {
        interactionUI.SetActive(false);
    }

    public void ClearInteraction()
    {
        currentInteractable = null;
        HideInteractionPrompt();
    }
}

