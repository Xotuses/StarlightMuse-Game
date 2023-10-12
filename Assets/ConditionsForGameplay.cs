using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ConditionsForGameplay : MonoBehaviour
{
    [Header("Conditions")]
    private bool completed40Waves = false;
    public static bool isOnVictoryScreen = false;
    private bool isDead = false;
    public static bool isOnDeathScreen = false;

    [Header("Conditional Screens")]
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject deathScreen;

    [Header("Conditional Events")]
    public static UnityEvent Victory = new();
    public static UnityEvent Death = new();

    public void Awake()
    {
        Victory?.AddListener(VictoryConditions); // When conditions are met, event triggers the Victory conditions

        Death?.AddListener(FailureConditions); // When conditions are met, event triggers the Death conditions
    }

    public void Update()
    {
        if (EnemySpawner.enemiesAlive == 0 && EnemySpawner.currentWave == 41)
        {
            Victory.Invoke();
        }

        if (LevelManager.healthPoints <= 0)
        {
            Death.Invoke();
        }
    }

    public void VictoryConditions()
    {
        CheckForCompletion();

        if (completed40Waves)
        {
            LoadVictoryScreen();
        }
    }

    public void FailureConditions()
    {
        CheckHealthPoints();

        if (isDead)
        {
            LoadDeathScreen();
        }
    }

    public void CheckForCompletion()
    {
        if (EnemySpawner.enemiesAlive == 0 && EnemySpawner.currentWave == 41)
        {
            completed40Waves = true;
        }
    }

    public void CheckHealthPoints() // Checks healthpoints
    {
        if (LevelManager.healthPoints <= 0)
        {
            isDead = true;
        }
    }

    public void LoadVictoryScreen() // Loads the Victory Screen 
    {
        victoryScreen.SetActive(true);
        isOnVictoryScreen = true;
        Time.timeScale = 0f;
    }

    public void LoadDeathScreen() // Loads death screen 
    {
        deathScreen.SetActive(true);
        isOnDeathScreen = true;
        Time.timeScale = 0f; // Stops any actively running processes
    }

    public void FreePlay() // Continues into endless mode
    {
        victoryScreen.SetActive(false);
        isOnVictoryScreen = false;
        Time.timeScale = 1f; // Starts running processes
    }

    public void ReturnToTitleScreen() // Returns to the start screen
    {
        victoryScreen.SetActive(false);
        Time.timeScale = 1f; // Starts running processes
        isOnVictoryScreen = false;
        SceneManager.LoadScene(0);
    }
}
