using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip mainGameSFX;
    [SerializeField] [Range(0f, 1f)] float mainGameSFXVolume = 0.015f;
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] [Range(0f, 1f)] float enemyDeathSFXVolume = 0.07f;
    [SerializeField] AudioClip victorySFX;
    [SerializeField] [Range(0f, 1f)] float victorySFXVolume = 0.1f;
    [SerializeField] AudioClip failedSFX;
    [SerializeField] [Range(0f, 1f)] float failedSFXVolume = 0.1f;

    AudioSource audioSource;
    float currentVolume;
    GameManager gameManager;

    void Awake() 
    {
        int numMusicPlayers = FindObjectsOfType<AudioManager>().Length;
        if (numMusicPlayers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentVolume = audioSource.volume;
        StartMainGameSFX();
    }

    void Update()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null) 
        {
            audioSource.volume = currentVolume;
            return; 
        }

        if (gameManager.GameHasPaused)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, currentVolume/2, Time.time);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(currentVolume/2, currentVolume, Time.time);
        }
    }

    public void StartMainGameSFX()
    {
        StopAllSFX();
        InvokeRepeating("MainGameSFX", 0, 2.0f);
    }

    void MainGameSFX()
    {
        if (!audioSource.isPlaying)
        {
            PlaySFXWithVol(mainGameSFX, mainGameSFXVolume);
        }
    }

    public void EnemyDeathSFX()
    {
        PlaySFXWithVol(enemyDeathSFX, enemyDeathSFXVolume);
    }

    public void VictorySFX()
    {
        StopAllSFX();
        PlaySFXWithVol(victorySFX, victorySFXVolume);
    }

    public void FailedSFX()
    {
        StopAllSFX();
        PlaySFXWithVol(failedSFX, failedSFXVolume);
    }

    void PlaySFXWithVol(AudioClip playSFX, float playVol)
    {
        audioSource.PlayOneShot(playSFX, playVol);
    }

    public void StopAllSFX()
    {
        CancelInvoke("MainGameSFX");
        audioSource.Stop();
    }
}
