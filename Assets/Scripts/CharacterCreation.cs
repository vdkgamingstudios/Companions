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
        GameManager.Instance.playerName = nameInput.text;

        SceneManager.LoadScene("GameSceneV1");
    }
}
