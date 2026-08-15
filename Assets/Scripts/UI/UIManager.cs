using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public enum MenuType
    {
        None,
        Pause,
        Inventory,
        Journal,
        Settings
    }

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject inventoryMenu;
    public GameObject journalMenu;
    //public GameObject settingsMenu;

    [Header("Pause Menu Sub Menus")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject soundMenu;

    [Header("UI")]
    public GameObject firstPauseButton;

    //[Header("Warning")]
    //public GameObject Warning;

    //Menu Checks
    public MenuType CurrentMenu { get; private set; } = MenuType.None;
    public bool IsMenuOpen => CurrentMenu != MenuType.None;

    [Header("Warning Popup")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private float popupDuration = 1.5f;

    private Coroutine popupCoroutine;

    void Start()
    {
        Time.timeScale = 1f;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        ToggleMenu(MenuType.Pause);
    }

    public void ToggleInventory()
    {
        ToggleMenu(MenuType.Inventory);
    }

    public void ToggleJournal()
    {
        ToggleMenu(MenuType.Journal);
    }

    //public void ToggleSettings()
    //{
    //    ToggleMenu(MenuType.Settings);
    //}

    public void ToggleMenu(MenuType menu)
    {
        if (CurrentMenu == menu)
        {
            CloseMenus();
        }
        else
        {
            OpenMenu(menu);
        }
    }

    public void OpenMenu(MenuType menu)
    {
        CloseMenuObjects();

        CurrentMenu = menu;

        switch (menu)
        {
            case MenuType.Pause:
                pauseMenu.SetActive(true);
                break;

            case MenuType.Inventory:
                inventoryMenu.SetActive(true);
                break;

            case MenuType.Journal:
                journalMenu.SetActive(true);
                break;

            //case MenuType.Settings:
            //    settingsMenu.SetActive(true);
            //    break;
        }

        Time.timeScale = 0;

        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (firstPauseButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstPauseButton);
        }
    }

    public void OpenPauseMenu()
    {
        ClosePausePanels();
        pauseMenu.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        ClosePausePanels();
        settingsMenu.SetActive(true);
    }

    public void OpenControlsMenu()
    {
        ClosePausePanels();
        controlsMenu.SetActive(true);
    }

    public void OpenDisplayMenu()
    {
        ClosePausePanels();
        displayMenu.SetActive(true);
    }

    public void OpenSoundMenu()
    {
        ClosePausePanels();
        soundMenu.SetActive(true);
    }

    public void HandlePauseEscape()
    {
        if (soundMenu.activeSelf)
        {
            OpenSettingsMenu();
            return;
        }

        if (displayMenu.activeSelf)
        {
            OpenSettingsMenu();
            return;
        }

        if (controlsMenu.activeSelf)
        {
            OpenSettingsMenu();
            return;
        }

        if (settingsMenu.activeSelf)
        {
            OpenPauseMenu();
            return;
        }

        if (pauseMenu.activeSelf)
        {
            CloseMenus();
        }
    }

    public void CloseMenus()
    {
        CloseMenuObjects();

        CurrentMenu = MenuType.None;

        Time.timeScale = 1;

        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CloseMenuObjects()
    {
        pauseMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        journalMenu.SetActive(false);
        //settingsMenu.SetActive(false);
    }

    private void ClosePausePanels()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        displayMenu.SetActive(false);
        soundMenu.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameSceneV1"); //Put the scene it should change to here
        Debug.Log("The player's name is " + GameManager.Instance.playerName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowPopup(string message)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
        }

        popupCoroutine = StartCoroutine(ShowPopupRoutine(message));
    }

    private IEnumerator ShowPopupRoutine(string message)
    {
        popupText.text = message;

        popupPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(popupDuration);

        popupPanel.SetActive(false);

        popupCoroutine = null;
    }
}
