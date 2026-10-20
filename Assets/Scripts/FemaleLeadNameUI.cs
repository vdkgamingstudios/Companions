using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Yarn.Unity;

public class FemaleLeadNameUI : MonoBehaviour
{
    [Header("Female Lead Name - Relationships Menu")]
    [SerializeField] private TMP_Text femaleLeadNameText;

    [Header("Female Lead Name - Above NPC")]
    [SerializeField] private TMP_Text overheadNameText;

    [Header("Yarn Dialogue")]
    [SerializeField] private DialogueRunner dialogueRunner;

    private string lastName = "";

    private void Start()
    {
        RefreshName();
    }

    private void Update()
    {
        RefreshName();
    }

    public void RefreshName()
    {
        if (dialogueRunner == null)
            return;

        string chosenName = "";

        bool found = dialogueRunner.VariableStorage.TryGetValue<string>(
            "$characterFLName",
            out chosenName
        );

        if (!found || string.IsNullOrWhiteSpace(chosenName))
        {
            chosenName = "?";
        }

        // Only update when the name changes
        if (chosenName == lastName)
            return;

        lastName = chosenName;

        // Update Relationships menu
        if (femaleLeadNameText != null)
        {
            femaleLeadNameText.text = chosenName;
        }

        // Update name above Female Lead NPC
        if (overheadNameText != null)
        {
            overheadNameText.text =
                chosenName == "?" ? "Female Lead" : chosenName;
        }

        // Update Relationship Manager
        if (RelationshipManager.Instance != null &&
            chosenName != "?")
        {
            RelationshipManager.Instance.SetDisplayName(
                "characterfl",
                chosenName
            );
        }
    }
}
