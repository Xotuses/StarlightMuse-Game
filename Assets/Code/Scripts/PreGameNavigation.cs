using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameNavigation : MonoBehaviour
{
    int sceneIndex;

    /// <summary>
    /// Allows the user to go to the next scene
    /// </summary>
    public void LoadNextScene()
    {
        SceneManager.LoadScene(sceneIndex + 1);
    }

    /// <summary>
    /// Allows the user to go back to the previous scene
    /// </summary>
    public void GoBack()
    {
        SceneManager.LoadScene(sceneIndex - 1);
    }

    /// <summary>
    /// Allows the user to go back to the previous scene
    /// </summary>
    public void GoBackToTitleScreen()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// This allows the player to load the first level of the game.
    /// </summary>
    public void LoadLevelOne()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// This allows the player to load the tips.
    /// </summary>
    public void LoadTips()
    {
        SceneManager.LoadScene(3);
    }

    /// <summary>
    /// This method allows the player to quit the game entirely
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// This builds a scene index for the script to use
    /// </summary>
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneIndex);
    }

    /// <summary>
    /// This method allows the player to navigate back to the previous scene once they press the Escape key
    /// </summary>
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            GoBackToTitleScreen();
        }
    }
}
