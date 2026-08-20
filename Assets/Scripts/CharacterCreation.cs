using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void ConfirmName()
    {
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            GameManager.Instance.playerName = "Asura";
        }
        else
        {
            GameManager.Instance.playerName = nameInput.text;
        }

        SceneManager.LoadScene("GameSceneV1.2 2");
    }
}
