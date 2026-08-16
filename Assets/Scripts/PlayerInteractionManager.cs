using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    PlayerManager player;

    private List<Interactable> currentInteractableActions;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
    }

    private void FixedUpdate()
    {
        //Check if the UI menu is not open and there is no pop up and if there is none check for interactables
        if (!UIManager.Instance.menuWindowIsOpen && !UIManager.Instance.popUpWindowIsOpen) 
        { 
            CheckForInteractable();
        }
    }

    private void CheckForInteractable()
    {
        if(currentInteractableActions.Count == 0)
        {
            return;
        }

        if (currentInteractableActions[0] == null)
        {
            currentInteractableActions.RemoveAt(0);
            return;
        }

        if (currentInteractableActions[0] != null) 
        { 
            
        }
    }
}
