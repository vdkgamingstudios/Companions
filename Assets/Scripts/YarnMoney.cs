using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnMoney : MonoBehaviour
{
    //Lets Yarn check how much money the player has.
    [YarnFunction("get_money")]
    public static int GetMoney()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return 0;
        }

        return GameManager.Instance.PlayerMoney;
    }

    //Lets Yarn spend money.
    [YarnCommand("spend_money")]
    public void SpendMoney(int amount)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        GameManager.Instance.SpendMoney(amount);
    }

    //Lets Yarn give the player money.
    [YarnCommand("add_money")]
    public void AddMoney(int amount)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        GameManager.Instance.AddMoney(amount);
    }
}
