using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class FemaleLeadNameUI : MonoBehaviour
{
    [Header("Female Lead Name")]
    [SerializeField] private TMP_Text femaleLeadNameText;

    [Header("Yarn Dialogue")]
    [SerializeField] private DialogueRunner dialogueRunner;

    private void OnEnable()
    {
        RefreshName();
    }

    public void RefreshName()
    {
        if (femaleLeadNameText == null || dialogueRunner == null)
        {
            Debug.LogWarning(
                "FemaleLeadNameUI: Missing UI or DialogueRunner reference."
            );
            return;
        }

        string chosenName = "";

        bool found = dialogueRunner.VariableStorage.TryGetValue<string>(
            "$characterFLName",
            out chosenName
        );

        if (!found || string.IsNullOrWhiteSpace(chosenName))
        {
            femaleLeadNameText.text = "?";
            return;
        }

        femaleLeadNameText.text = chosenName;

        if (RelationshipManager.Instance != null)
        {
            RelationshipManager.Instance.SetDisplayName(
                "characterfl",
                chosenName
            );
        }
    }
}
