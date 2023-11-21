using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField] float buildDelay = 1f;
    [Header("Gold VFX")]
    [SerializeField] GameObject towerPlaceGoldVFX;
    [SerializeField] GameObject towerRemoveGoldVFX;
    GameObject spawnAtRuntime;

    int cost;
    int deconstuctCost;

    Vector2Int coordinates;
    Bank bank;
    GridManager gridManager;
    Pathfinder pathfinder;
    GameManager gameManager;

    void Awake()
    {
        bank = FindObjectOfType<Bank>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
        spawnAtRuntime = GameObject.FindWithTag("SpawnAtRuntime");
    }

    void Start()
    {
        StartCoroutine(Build());
        PlayTowerGoldVFX(towerPlaceGoldVFX);
    }

    void SetTowerSettings()
    {
        cost = gameManager.Cost;
        deconstuctCost = gameManager.DeconstuctCost;
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        gameManager = FindObjectOfType<GameManager>();
        SetTowerSettings();
        bank = FindObjectOfType<Bank>();

        if (bank == null || gameManager.GameHasPaused == true)
        {
            return false;
        }

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }

    public void OnMouseOver() 
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameManager = FindObjectOfType<GameManager>();
            SetTowerSettings();
            bank.Deposit(deconstuctCost);
            PlayTowerGoldVFX(towerRemoveGoldVFX);
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
            Destroy(gameObject);
            gridManager.OpenNode(coordinates);
            pathfinder.NotifyReceivers();
        }
    }

    IEnumerator Build()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }

    void PlayTowerGoldVFX(GameObject towerGoldVFX)
    {
        Vector3 newVFXPosition = towerGoldVFX.transform.position + transform.position;
        GameObject goldVFX = Instantiate(towerGoldVFX, newVFXPosition, Quaternion.identity, spawnAtRuntime.transform);
    }
}
