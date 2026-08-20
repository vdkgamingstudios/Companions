using UnityEngine;


public class Interactable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        //Testing that the button is being pressed
        Debug.Log("Button pressed!");

        PlayerUIManager playerUI = FindFirstObjectByType<PlayerUIManager>();

        if (playerUI != null)
        {
            playerUI.ShowInteractionMessage("Who's Over There ->");
        }
    }

    public string GetInteractionText()
    {
        return "Press";
    }
}
