using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Data")]
    public string playerName = "";

    [Header("Character Data")]
    public string characterFLName = "";

    [Header("Relationships")]
    public int villagerAffection = 0;//Testing and will be reworked for specific characters later on

    [Header("Money")]
    [SerializeField] private int playerMoney = 100;

    // Allows other scripts to read the player's money, but prevents them from directly changing it.
    public int PlayerMoney => playerMoney;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Give money to the player.
    public void AddMoney(int amount)
    {
        playerMoney += amount;

        Debug.Log("Player received " + amount +
                  " money. Total: " + playerMoney);
    }

    //Check whether the player has enough money.
    public bool HasEnoughMoney(int amount)
    {
        return playerMoney >= amount;
    }

    //Attempt to spend money. Returns true if the purchase was successful.
    public bool SpendMoney(int amount)
    {
        if (!HasEnoughMoney(amount))
        {
            Debug.Log("Player does not have enough money.");
            return false;
        }

        playerMoney -= amount;

        Debug.Log("Player spent " + amount +
                  " money. Remaining: " + playerMoney);

        return true;
    }
}
