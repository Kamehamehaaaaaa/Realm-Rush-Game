using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("Gold VFX")]
    [SerializeField] GameObject enemyKillRewardGoldVFX;
    [SerializeField] GameObject enemyReachPenaltyGoldVFX;
    GameObject spawnAtRuntime;

    int goldReward;
    int goldPenalty;

    Bank bank;
    GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetEnemySettings();
    }

    void Start()
    {
        bank = FindObjectOfType<Bank>();
        spawnAtRuntime = GameObject.FindWithTag("SpawnAtRuntime");
    }

    void SetEnemySettings()
    {
        goldReward = gameManager.GoldReward;
        goldPenalty = gameManager.GoldPenalty;
    }

    public void RewardGold()
    {
        if (bank == null) { return; }
        bank.Deposit(goldReward);
        PlayEnemyGoldVFX(enemyKillRewardGoldVFX);
    }

    public void StealGold()
    {
        if (bank == null) { return; }
        bank.Withdraw(goldPenalty);
        PlayEnemyGoldVFX(enemyReachPenaltyGoldVFX);
    }

    void PlayEnemyGoldVFX(GameObject enemyGoldVFX)
    {
        Vector3 newVFXPosition = enemyGoldVFX.transform.position + transform.position;
        GameObject goldVFX = Instantiate(enemyGoldVFX, newVFXPosition, Quaternion.identity, spawnAtRuntime.transform);
    }
}
