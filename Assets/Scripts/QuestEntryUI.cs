using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestEntryUI : MonoBehaviour
{
    [Header("Quest Text")]
    [SerializeField] private TMP_Text questTitle;
    [SerializeField] private TMP_Text questDescription;

    public void Setup(QuestData quest)
    {
        if (quest == null)
            return;

        if (questTitle != null)
            questTitle.text = quest.questTitle;

        if (questDescription != null)
            questDescription.text = quest.questDescription;
    }
}
