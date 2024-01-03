using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;
    
    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    [SerializeField] TextMeshProUGUI displayBalance;

    GameManager gameManager;

    void Awake()
    {
        currentBalance = startingBalance;
        UpdateDisplay();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateDisplay();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);

        if (currentBalance < 0)
        {
            gameManager.EndGame();
        }
        else
        {
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        displayBalance.text = "Gold: " + currentBalance + "$";
    }
}
