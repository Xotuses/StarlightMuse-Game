using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausedMenu;
    public static bool IsPaused;

    void Start()
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1f;
    }

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

    public void PauseGame() // Pauses the game
    {
        pausedMenu.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void ResumeGame() // Resumes the game
    {
        pausedMenu.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void QuitToStartingScreen() // Takes the player back to the starting screen
    {
        IsPaused = false;
        SceneManager.LoadScene(0);
    }
}
