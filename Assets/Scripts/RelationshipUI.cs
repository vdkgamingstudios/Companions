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

    [Header("Character Three")]
    [SerializeField] private TMP_Text characterThreeNameText;
    [SerializeField] private TMP_Text characterThreeAffectionText;

    [Header("Character Four")]
    [SerializeField] private TMP_Text characterFourNameText;
    [SerializeField] private TMP_Text characterFourAffectionText;

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

        NPCRelationship characterOne = RelationshipManager.Instance.GetRelationship("characterone");

        NPCRelationship characterTwo = RelationshipManager.Instance.GetRelationship("charactertwo");

        NPCRelationship characterThree = RelationshipManager.Instance.GetRelationship("characterthree");

        NPCRelationship characterFour = RelationshipManager.Instance.GetRelationship("characterfour");

        if (characterOne != null)
        {
            if (characterOneNameText != null)
            {
                characterOneNameText.text = characterOne.displayName;
            }

            if (characterOneAffectionText != null)
            {
                characterOneAffectionText.text =characterOne.affection.ToString();
            }
        }

        if (characterTwo != null)
        {
            if (characterTwoNameText != null)
            {
                characterTwoNameText.text = characterTwo.displayName;
            }

            if (characterTwoAffectionText != null)
            {
                characterTwoAffectionText.text = characterTwo.affection.ToString();
            }
        }

        if (characterThree != null)
        {
            if (characterThreeNameText != null)
            {
                characterThreeNameText.text = characterThree.displayName;
            }

            if (characterThreeAffectionText != null)
            {
                characterThreeAffectionText.text = characterThree.affection.ToString();
            }
        }

        if (characterFour != null)
        {
            if (characterFourNameText != null)
            {
                characterFourNameText.text = characterFour.displayName;
            }

            if (characterFourAffectionText != null)
            {
                characterFourAffectionText.text = characterFour.affection.ToString();
            }
        }
    }
}
