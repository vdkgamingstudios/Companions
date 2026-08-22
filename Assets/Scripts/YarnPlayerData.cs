using UnityEngine;
using Yarn.Unity;

public class YarnPlayerData : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    private void Start()
    {
        SyncPlayerDataToYarn();
    }

    public void SyncPlayerDataToYarn()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        if (dialogueRunner == null)
        {
            Debug.LogError("Dialogue Runner not assigned!");
            return;
        }

        //Only sync the name if the player has already chosen one.
        if (!string.IsNullOrWhiteSpace(GameManager.Instance.playerName))
        {
            dialogueRunner.VariableStorage.SetValue(
                "$playerName",
                GameManager.Instance.playerName
            );
        }
    }
}
