using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] int cost = 75;
    [SerializeField] int deconstuctCost = 25;
    public int Cost { get { return cost; } }
    public int DeconstuctCost { get { return deconstuctCost; } }

    [Header("Enemy Settings")]
    [SerializeField] [Range(0f, 5f)] float speed = 0.5f;
    [SerializeField] int maxHitPoints = 3;
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 5;
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;
    public float Speed { get { return speed; } }
    public int MaxHitPoints { get { return maxHitPoints; } }
    public int DifficultyRamp { get { return difficultyRamp; } }
    public int GoldReward { get { return goldReward; } }
    public int GoldPenalty { get { return goldPenalty; } }

    [Header("VFX Settings")]
    [SerializeField] TextMeshProUGUI towerPlaceGoldVFXText;
    [SerializeField] TextMeshProUGUI towerRemoveGoldVFXText;
    [SerializeField] TextMeshProUGUI enemyKillRewardGoldVFXText;
    [SerializeField] TextMeshProUGUI enemyReachPenaltyGoldVFXText;

    [Header("Game Pause & End Screens")]
    [SerializeField] GameObject pauseScreenUI;
    [SerializeField] GameObject victoryScreenUI;
    [SerializeField] GameObject failedScreenUI;

    bool gameHasEnded = false;
    public bool GameHasEnded { get { return gameHasEnded; } }
    bool gameHasPaused = false;
    public bool GameHasPaused { get { return gameHasPaused; } }

    AudioManager audioManager;

    void Start()
    {
        SetTextForGoldVFX();
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameHasPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void LateUpdate() 
    {
        if (gameHasEnded)
        {
            StopAllCoroutines();
        }
    }
    
    public void ResumeGame()
    {
        pauseScreenUI.SetActive(false);
        Time.timeScale = 1f;
        gameHasPaused = false;
    }

    public void PauseGame()
    {
        pauseScreenUI.SetActive(true);
        Time.timeScale = 0f;
        gameHasPaused = true;
    }


    void SetTextForGoldVFX()
    {
        towerPlaceGoldVFXText.text = "-" + cost + "$";      // Lose Gold when placing Tower.
        towerRemoveGoldVFXText.text = "+" + deconstuctCost + "$";       // Win Small Gold when removing tower.
        enemyKillRewardGoldVFXText.text = "+" + goldReward + "$";      // Win Gold when enemy dies.
        enemyReachPenaltyGoldVFXText.text = "-" + goldPenalty + "$";       // Lose Gold when enemy attacks gate.
    }

    public void Victory()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            victoryScreenUI.SetActive(true);
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.VictorySFX(); 
        }
    }

    public void EndGame()
    {
        if(gameHasEnded == false)
        {
            gameHasEnded = true;
            failedScreenUI.SetActive(true);
            audioManager = FindObjectOfType<AudioManager>();
            audioManager.FailedSFX(); 
        }
    }

    public void RestartGame()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.StartMainGameSFX();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void MainMenuScene()
    {
        Time.timeScale = 1f;
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.StartMainGameSFX();
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
