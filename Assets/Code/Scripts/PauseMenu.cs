using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedMenu;
    public static bool IsPaused;

    /// <summary>
    /// This method sets the timeScale to default and turns off the pause menu
    /// </summary>
    void Start()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// This method resumes the game if the player presses escape.
    /// If the player presses escape while unpaused, it will pause the game.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (IsPaused)
            {
                ResumeGame(); 
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// This method stops active processes running, pausing the game
    /// </summary>
    public void PauseGame() 
    {
        pausedMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    /// <summary>
    /// This method allows active processes to run again, unpausing the menu
    /// </summary>
    public void ResumeGame() 
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    /// <summary>
    /// This method takes the player back to the starting screen
    /// </summary>
    public void QuitToStartingScreen() 
    {
        IsPaused = false;
        SceneManager.LoadScene(0);
    }
}
