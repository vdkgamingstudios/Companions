using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    public TMP_Text playerNameText;
    public TMP_Text playerLevels;
    //public TMP_Text playerAffectionLevel;
    public TMP_Text playerMoneyText;

    [Header("Interaction")]
    public TMP_Text interactionMessageText;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionMessageDuration = 2f;

    private void Start()
    {
        UpdatePlayerUI();

        //Make sure the interaction message starts hidden.
        HideInteractionMessage();
    }

    //Runs whenever this GameObject becomes active.This makes sure the Player Stats menu displays the latest name, level and money values.
    private void OnEnable()
    {
        UpdatePlayerUI();
    }

    public void UpdatePlayerUI()
    {
        if (GameManager.Instance == null)
        {
            //Debug.LogError("GameManager instance not found!");
            return;
        }

        //Display the player's name.
        if (playerNameText != null)
        {
            playerNameText.text = GameManager.Instance.playerName;
        }

        //Display the player's current money.
        if (playerMoneyText != null)
        {
            playerMoneyText.text = GameManager.Instance.PlayerMoney.ToString();
        }
    }

    public void ShowInteractionMessage(string message)
    {
        // Safety check in case the TMP text
        // has not been assigned in the Inspector.
        if (interactionMessageText == null)
        {
            return;
        }

        // Cancel any previous hide timer.
        // This prevents an older timer from hiding
        // a newly displayed message.
        CancelInvoke(nameof(HideInteractionMessage));

        // Make sure the actual TMP GameObject is active.
        interactionMessageText.gameObject.SetActive(true);

        // Set the message.
        interactionMessageText.text = message;

        // Automatically hide it after a short time.
        Invoke(
            nameof(HideInteractionMessage),
            interactionMessageDuration
        );
        //if (interactionMessageText == null)
        //{
        //    return;
        //}

        //interactionMessageText.text = message;
        //interactionMessageText.gameObject.SetActive(true);

        //CancelInvoke(nameof(HideInteractionMessage));
        //Invoke(nameof(HideInteractionMessage), 2f);
    }

    public void HideInteractionMessage()
    {
        if (interactionMessageText == null)
        {
            return;
        }

        // Cancel any pending hide timer.
        CancelInvoke(nameof(HideInteractionMessage));

        // Clear the text.
        interactionMessageText.text = "";

        // Hide the TMP GameObject.
        interactionMessageText.gameObject.SetActive(false);
        //if (interactionMessageText != null)
        //{
        //    interactionMessageText.gameObject.SetActive(false);
        //}
    }
}
