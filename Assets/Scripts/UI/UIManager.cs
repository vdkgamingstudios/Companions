using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Flags")]
    public bool menuWindowIsOpen = false; //Inventory screen, equipment menu etc
    public bool popUpWindowIsOpen = false; //Item pick up, dialogue pop up

    public enum MenuType
    {
        None,
        Pause,
        Inventory,
        Journal,
        PlayerStats,
        Relationships
    } //Settings use to be there

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject inventoryMenu;
    public GameObject journalMenu;
    public GameObject playerStatsMenu;
    public GameObject relationshipsMenu;

    [Header("Pause Menu Sub Menus")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject controlsMenu;
    [SerializeField] private GameObject displayMenu;
    [SerializeField] private GameObject soundMenu;

    [Header("UI")]
    public GameObject firstPauseButton;

    //Menu Checks
    public MenuType CurrentMenu { get; private set; } = MenuType.None;
    public bool IsMenuOpen => CurrentMenu != MenuType.None;

    [Header("Warning Popup")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private float popupDuration = 1.5f;

    private Coroutine popupCoroutine;

    [Header("Player Stats UI")]
    [SerializeField] private PlayerUIManager playerUIManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1f;
    }

    void Start()
    {
        Time.timeScale = 1f;

        //Make sure all menus begin closed.
        CloseMenuObjects();
        ClosePausePanels();

        CurrentMenu = MenuType.None;
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

    public void TogglePlayerStats()
    {
        ToggleMenu(MenuType.PlayerStats);
    }

    public void ToggleRelationships()
    {
        ToggleMenu(MenuType.Relationships);
    }

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
        //Hide all currently open menus.
        CloseMenuObjects();
        ClosePausePanels();

        //Record which menu we're about to open.
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

            case MenuType.PlayerStats:
                playerStatsMenu.SetActive(true);
                //Refresh the player's current stats whenever the Player Stats menu is opened.
                if (playerUIManager != null)
                {
                    playerUIManager.UpdatePlayerUI();
                }
                break;

            case MenuType.Relationships:
                relationshipsMenu.SetActive(true);

                RelationshipUI relationshipUI = relationshipsMenu.GetComponent<RelationshipUI>();

                if (relationshipUI != null)
                {
                    relationshipUI.UpdateRelationshipUI();
                }

                break;
        }

        //Pause gameplay while a menu is open.
        Time.timeScale = 0f;

        AudioListener.pause = true;

        //Unlock and display the cursor.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Automatically select the first pause button when the Pause menu is opened.
        if (menu == MenuType.Pause &&
            firstPauseButton != null &&
            EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstPauseButton);
        }
    }

    public void NextHubMenu()
    {
        switch (CurrentMenu)
        {
            case MenuType.Inventory:
                OpenMenu(MenuType.Journal);
                break;

            case MenuType.Journal:
                OpenMenu(MenuType.PlayerStats);
                break;

            case MenuType.PlayerStats:
                OpenMenu(MenuType.Relationships);
                break;

            case MenuType.Relationships:
                //Loop back to the beginning.
                OpenMenu(MenuType.Inventory);
                break;
        }
    }

    public void PreviousHubMenu()
    {
        switch (CurrentMenu)
        {
            case MenuType.Inventory:
                //Loop backwards to the end.
                OpenMenu(MenuType.Relationships);
                break;

            case MenuType.Journal:
                OpenMenu(MenuType.Inventory);
                break;

            case MenuType.PlayerStats:
                OpenMenu(MenuType.Journal);
                break;

            case MenuType.Relationships:
                OpenMenu(MenuType.PlayerStats);
                break;
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
        ClosePausePanels();

        CurrentMenu = MenuType.None;

        Time.timeScale = 1;

        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void CloseMenuObjects()
    {
        pauseMenu.SetActive(false);
        inventoryMenu.SetActive(false);
        journalMenu.SetActive(false);
        playerStatsMenu.SetActive(false);
        relationshipsMenu.SetActive(false);
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
        SceneManager.LoadScene("GameSceneV1.2 2"); //Put the scene it should change to here
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
