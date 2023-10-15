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
    public bool isCalledOnce = false;

    [Header("Conditional Screens")]
    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject deathScreen;

    [Header("Conditional Events")]
    public static UnityEvent Victory = new(); 
    public static UnityEvent Death = new();

    public void Awake()
    {
        // When conditions are met, event triggers the Victory conditions
        Victory?.AddListener(VictoryConditions); 
        // When conditions are met, event triggers the Death conditions
        Death?.AddListener(FailureConditions); 
    }

    /// <summary>
    /// This method invokes Victory or Death events when conditions are met.
    /// </summary>
    public void Update()
    {
        if ((EnemySpawner.enemiesAlive == 0 && EnemySpawner.currentWave == 41) && !isCalledOnce)
        {
            Victory.Invoke();
            isCalledOnce = true;
        }

        if (LevelManager.healthPoints <= 0 && !isCalledOnce)
        {
            Death.Invoke();
            isCalledOnce = true;
        }
    }

    /// <summary>
    /// This allows the game to load the Victory screen once conditions are met
    /// </summary>
    public void VictoryConditions() 
    {
        CheckForCompletion();

        if (completed40Waves)
        {
            LoadVictoryScreen();
        }
    }

    /// <summary>
    /// This allows the game to load the death screen once conditions are met.
    /// Specfically, if the player is dead.
    /// </summary>
    public void FailureConditions() 
    {
        CheckHealthPoints();

        if (isDead)
        {
            LoadDeathScreen();
        }
    }

    /// <summary>
    /// This method checks if the player has completed the 40 waves.
    /// If they have, it changes the completed40Waves boolean value to true.
    /// </summary>
    public void CheckForCompletion()  
    {
        if (EnemySpawner.enemiesAlive == 0 && EnemySpawner.currentWave == 41)
        {
            completed40Waves = true;
        }
    }

    /// <summary>
    /// This method checks if the players healthPoints have dropped to or below 0.
    /// If true, it sets the boolean value to true
    /// </summary>
    public void CheckHealthPoints() 
    {
        if (LevelManager.healthPoints <= 0)
        {
            isDead = true;
        }
    }

    /// <summary>
    /// This method load the victory screen and pauses all other processes
    /// </summary>
    public void LoadVictoryScreen() 
    {
        victoryScreen.SetActive(true);
        isOnVictoryScreen = true;
        // Stops any actively running processes
        Time.timeScale = 0f; 
    }

    /// <summary>
    /// This method Loads the death screen and pauses all other processes.
    /// </summary>
    public void LoadDeathScreen() 
    {
        deathScreen.SetActive(true);
        isOnDeathScreen = true;
        Time.timeScale = 0f; 
    }

    /// <summary>
    /// This method continues into freeplay mode and resumes all other processes.
    /// </summary>
    public void FreePlay() 
    {
        victoryScreen.SetActive(false);
        isOnVictoryScreen = false;
        // Starts running processes
        Time.timeScale = 1f;
        completed40Waves = false;
    }

    /// <summary>
    /// Returns to the Title screen.
    /// </summary>
    public void ReturnToTitleScreen() 
    {
        victoryScreen.SetActive(false);
        Time.timeScale = 1f; 
        isOnVictoryScreen = false;
        SceneManager.LoadScene(0);
    }
}
