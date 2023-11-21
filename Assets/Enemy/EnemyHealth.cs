using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    int maxHitPoints;
    int difficultyRamp;
    int currentHitPoints = 0;

    AudioManager audioManager;
    GameManager gameManager;
    Enemy enemy;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetEnemySettings();
    }

    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    void Start() 
    {
        enemy = GetComponent<Enemy>();
    }

    void SetEnemySettings()
    {
        maxHitPoints = gameManager.MaxHitPoints;
        difficultyRamp = gameManager.DifficultyRamp;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints--;
        
        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.EnemyDeathSFX();
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
