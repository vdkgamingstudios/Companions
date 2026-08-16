using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Button pressed!");

        PlayerUIManager playerUI = FindFirstObjectByType<PlayerUIManager>();

        if (playerUI != null)
        {
            playerUI.ShowInteractionMessage("Button pressed!");
        }
    }

    public string GetInteractionText()
    {
        return "Press";
    }

    //Old code
    #region
    //public string interactableText;//Text prompt for when the player enters the interaction collider
    //[SerializeField] protected Collider interactableCollider; //Collider that checks for the player interaction

    //protected virtual void Awake() 
    //{ 
    //    if(interactableCollider == null)
    //    {
    //        interactableCollider = GetComponent<Collider>();
    //    }
    //}

    //protected virtual void Start() 
    //{ 

    //}

    //public virtual void Interact(PlayerManager player)
    //{

    //}

    //public virtual void OnTriggerEnter(Collider other)
    //{

    //}

    //public virtual void OnTriggerExit(Collider other)
    //{

    //}
    #endregion
}
