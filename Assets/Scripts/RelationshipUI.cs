using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelationshipUI : MonoBehaviour
{
    [Header("Character One")]
    [SerializeField] private TMP_Text characterOneNameText;
    [SerializeField] private TMP_Text characterOneAffectionText;

    [Header("Character Two")]
    [SerializeField] private TMP_Text characterTwoNameText;
    [SerializeField] private TMP_Text characterTwoAffectionText;

    //Called automatically whenever the Relationships menu becomes active.
    private void OnEnable()
    {
        UpdateRelationshipUI();
    }

    public void UpdateRelationshipUI()
    {
        if (RelationshipManager.Instance == null)
        {
            Debug.LogWarning("RelationshipManager not found.");
            return;
        }

        NPCRelationship characterOne =
            RelationshipManager.Instance.GetRelationship(
                "characterone"
            );

        NPCRelationship characterTwo =
            RelationshipManager.Instance.GetRelationship(
                "charactertwo"
            );

        if (characterOne != null)
        {
            if (characterOneNameText != null)
            {
                characterOneNameText.text =
                    characterOne.displayName;
            }

            if (characterOneAffectionText != null)
            {
                characterOneAffectionText.text =
                    characterOne.affection.ToString();
            }
        }

        if (characterTwo != null)
        {
            if (characterTwoNameText != null)
            {
                characterTwoNameText.text =
                    characterTwo.displayName;
            }

            if (characterTwoAffectionText != null)
            {
                characterTwoAffectionText.text =
                    characterTwo.affection.ToString();
            }
        }
        ////Make sure the RelationshipManager exists before trying to get any relationship data.
        //if (RelationshipManager.Instance == null)
        //{
        //    Debug.LogWarning("RelationshipManager not found.");
        //    return;
        //}

        ////Get Character One's current affection level from the RelationshipManager.
        //int characterOneAffection = RelationshipManager.Instance.GetAffection("characterone");

        ////Get Character Two's current affection level from the RelationshipManager.
        //int characterTwoAffection = RelationshipManager.Instance.GetAffection("charactertwo");

        ////Display the current affection values in the Relationships menu.
        //characterOneAffectionText.text = characterOneAffection.ToString();

        //characterTwoAffectionText.text = characterTwoAffection.ToString();
    }
}
