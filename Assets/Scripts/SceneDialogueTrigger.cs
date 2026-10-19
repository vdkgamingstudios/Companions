using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SceneDialogueTrigger : MonoBehaviour
{
    [Header("Yarn Dialogue")]
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private string startingNode = "SceneIntroduction";

    [Header("Settings")]
    [SerializeField] private bool playOnStart = true;

    [Header("Camera Delay")]
    [SerializeField] private float dialogueDelay = 1.5f;

    private void Start()
    {
        if (playOnStart)
        {
            StartCoroutine(StartDialogueAfterDelay());
        }
    }

    private IEnumerator StartDialogueAfterDelay()
    {
        // Wait for Unity to finish the current frame.
        yield return new WaitForEndOfFrame();

        // Allow time for the camera to position itself.
        yield return new WaitForSecondsRealtime(dialogueDelay);

        StartSceneDialogue();
    }

    public void StartSceneDialogue()
    {
        Debug.Log("SCENE DIALOGUE: Attempting to start.");

        if (dialogueRunner == null)
        {
            Debug.LogError("SCENE DIALOGUE: Dialogue Runner is missing!");
            return;
        }

        if (dialogueRunner.IsDialogueRunning)
        {
            Debug.LogWarning("SCENE DIALOGUE: Dialogue already running.");
            return;
        }

        Debug.Log("SCENE DIALOGUE: Starting node " + startingNode);

        dialogueRunner.StartDialogue(startingNode);
    }
}
