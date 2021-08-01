using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } } // set it as a property
    [SerializeField] TextMeshProUGUI balanceDisplayed;

    void Awake()
    {
        currentBalance = startingBalance;
        UpdateBalanceDisplayed();
    }

    public void Deposite(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateBalanceDisplayed();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        // if lose
        if (currentBalance < 0)
        {
            ReloadScene();
        }
        UpdateBalanceDisplayed();
    }

    void ReloadScene() 
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void UpdateBalanceDisplayed()
    {
        balanceDisplayed.text = "GOLD: " + currentBalance;
    }
}
