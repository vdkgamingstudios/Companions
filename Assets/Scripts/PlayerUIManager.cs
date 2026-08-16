using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    public TMP_Text playerNameText;
    public TMP_Text playerLevels;
    public TMP_Text playerAffectionLevel;

    [Header("Interaction")]
    public TMP_Text interactionMessageText;

    private void Start()
    {
        UpdatePlayerUI();

        interactionMessageText.gameObject.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        playerNameText.text = GameManager.Instance.playerName;
    }

    public void ShowInteractionMessage(string message)
    {
        interactionMessageText.text = message;
        interactionMessageText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideInteractionMessage));
        Invoke(nameof(HideInteractionMessage), 2f);
    }

    public void HideInteractionMessage()
    {
        interactionMessageText.gameObject.SetActive(false);
    }
}
