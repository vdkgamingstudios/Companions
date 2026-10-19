using System.Text;
using TMPro;
using UnityEngine;

public class JournalUI : MonoBehaviour
{
    [Header("Quest List")]
    [SerializeField] private Transform questContent;
    [SerializeField] private QuestEntryUI questEntryPrefab;

    [Header("Quest Heading")]
    [SerializeField] private TMPro.TMP_Text questHeadingText;

    [Header("Journal Settings")]
    [SerializeField] private bool showCompleted = false;

    private void OnEnable()
    {
        //Open the journal on the Active tab.
        showCompleted = false;

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestsChanged += RefreshJournal;
        }

        UpdateQuestHeading();
        RefreshJournal();
    }

    private void OnDisable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestsChanged -= RefreshJournal;
        }
    }

    public void ShowActiveQuests()
    {
        showCompleted = false;
        UpdateQuestHeading();
        RefreshJournal();
    }

    public void ShowCompletedQuests()
    {
        showCompleted = true;
        UpdateQuestHeading();
        RefreshJournal();
    }

    public void RefreshJournal()
    {
        if (questContent == null ||
            questEntryPrefab == null ||
            QuestManager.Instance == null)
        {
            return;
        }

        // Remove the previously displayed quest entries.
        foreach (Transform child in questContent)
        {
            Destroy(child.gameObject);
        }

        // Get the appropriate quest list.
        var quests = showCompleted
            ? QuestManager.Instance.GetCompletedQuests()
            : QuestManager.Instance.GetActiveQuests();

        // Create a UI entry for each quest.
        foreach (QuestData quest in quests)
        {
            QuestEntryUI entry = Instantiate(
                questEntryPrefab,
                questContent
            );

            entry.Setup(quest);
        }
    }

    private void UpdateQuestHeading()
    {
        if (questHeadingText == null)
            return;

        questHeadingText.text = showCompleted
            ? "Quests (Completed)"
            : "Quests (Active)";
    }
}
