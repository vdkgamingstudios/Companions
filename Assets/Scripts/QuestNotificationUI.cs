using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestNotificationUI : MonoBehaviour
{
    [Header("Notification UI")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text notificationTitle;
    [SerializeField] private TMP_Text notificationQuestName;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 3f;

    private Coroutine notificationCoroutine;

    private readonly Queue<Notification> pendingNotifications =
        new Queue<Notification>();

    private struct Notification
    {
        public string title;
        public string questName;

        public Notification(string title, string questName)
        {
            this.title = title;
            this.questName = questName;
        }
    }

    private void Awake()
    {
        if (notificationPanel != null)
        {
            notificationPanel.SetActive(false);
        }
    }

    public void ShowQuestStarted(QuestData quest)
    {
        if (quest == null)
            return;

        QueueNotification("NEW QUEST RECEIVED", quest.questTitle);
    }

    public void ShowQuestCompleted(QuestData quest)
    {
        if (quest == null)
            return;

        QueueNotification("QUEST COMPLETED", quest.questTitle);
    }

    private void QueueNotification(string title, string questName)
    {
        pendingNotifications.Enqueue(
            new Notification(title, questName)
        );

        if (notificationCoroutine == null)
        {
            notificationCoroutine = StartCoroutine(
                DisplayNotifications()
            );
        }
    }

    private IEnumerator DisplayNotifications()
    {
        while (pendingNotifications.Count > 0)
        {
            Notification notification =
                pendingNotifications.Dequeue();

            if (notificationPanel == null ||
                notificationTitle == null ||
                notificationQuestName == null)
            {
                Debug.LogWarning("Quest notification UI references are missing.");
                break;
            }

            notificationTitle.text = notification.title;
            notificationQuestName.text = notification.questName;

            notificationPanel.SetActive(true);

            //Uses realtime so notifications still work if gameplay is paused.
            yield return new WaitForSecondsRealtime(displayDuration);

            notificationPanel.SetActive(false);

            //Small gap between multiple notifications.
            yield return new WaitForSecondsRealtime(0.2f);
        }

        notificationCoroutine = null;
    }
}
