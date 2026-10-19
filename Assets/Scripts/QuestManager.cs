using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest Notifications")]
    [SerializeField] private QuestNotificationUI questNotificationUI;

    [Header("Available Quests")]
    [SerializeField]
    private List<QuestData> allQuests = new List<QuestData>();

    private HashSet<string> activeQuests = new HashSet<string>();

    private HashSet<string> completedQuests = new HashSet<string>();

    public event System.Action OnQuestsChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public QuestData GetQuest(string questID)
    {
        return allQuests.Find(
            quest => quest != null && quest.questID == questID
        );
    }

    public bool IsQuestActive(string questID)
    {
        return activeQuests.Contains(questID);
    }

    public bool IsQuestCompleted(string questID)
    {
        return completedQuests.Contains(questID);
    }

    public List<QuestData> GetActiveQuests()
    {
        List<QuestData> result = new List<QuestData>();

        foreach (QuestData quest in allQuests)
        {
            if (quest != null && activeQuests.Contains(quest.questID))
                result.Add(quest);
        }

        return result;
    }

    public List<QuestData> GetCompletedQuests()
    {
        List<QuestData> result = new List<QuestData>();

        foreach (QuestData quest in allQuests)
        {
            if (quest != null && completedQuests.Contains(quest.questID))
                result.Add(quest);
        }

        return result;
    }

    [YarnCommand("start_quest")]
    public void StartQuest(string questID)
    {
        QuestData quest = GetQuest(questID);

        if (quest == null)
        {
            Debug.LogWarning("Quest not registered: " + questID);
            return;
        }

        // Don't start the same quest twice.
        if (activeQuests.Contains(questID) ||
            completedQuests.Contains(questID))
        {
            return;
        }

        activeQuests.Add(questID);

        Debug.Log("Quest started: " + quest.questTitle);

        OnQuestsChanged?.Invoke();

        // Show quest received notification.
        if (questNotificationUI != null)
        {
            questNotificationUI.ShowQuestStarted(quest);
        }
    }

    [YarnCommand("complete_quest")]
    public void CompleteQuest(string questID)
    {
        QuestData quest = GetQuest(questID);

        if (quest == null)
        {
            Debug.LogWarning("Quest not registered: " + questID);
            return;
        }

        if (!activeQuests.Remove(questID))
        {
            Debug.LogWarning("Quest is not active: " + questID);
            return;
        }

        completedQuests.Add(questID);

        Debug.Log("Quest completed: " + quest.questTitle);

        OnQuestsChanged?.Invoke();

        // Show quest completed notification.
        if (questNotificationUI != null)
        {
            questNotificationUI.ShowQuestCompleted(quest);
        }
    }
}
