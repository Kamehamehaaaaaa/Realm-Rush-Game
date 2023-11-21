using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 2.5f;
    [SerializeField] [Range(1, 50)] int enemiesToDefend = 40;

    int enemiesToSurvive;
    GameObject[] pool;
    GameManager gameManager;

    void Awake() 
    {
        PopulatePool();
        enemiesToSurvive = enemiesToDefend;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnEnemy());
    }

    void Update() 
    {
        if (gameManager.GameHasEnded)
        {
            Invoke("DisableAllObjectsInPool", 2f);
        }
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                enemiesToSurvive--;
                return;
            }
        }
    }

    void DisableAllObjectsInPool()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy)
            {
                pool[i].SetActive(false);
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        //while (enemiesToSurvive > 0 && gameManager.GameHasEnded == false)
        while (gameManager.GameHasEnded == false)
        {
            if (enemiesToSurvive > 0)
            {
                EnableObjectInPool();
                yield return new WaitForSeconds(spawnTimer);
            }
            else
            {
                yield return new WaitUntil(() => CheckAllObjectPoolDisabled());
                gameManager.Victory();
            }
        }
    }

    bool CheckAllObjectPoolDisabled()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (pool[i].activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }
}
