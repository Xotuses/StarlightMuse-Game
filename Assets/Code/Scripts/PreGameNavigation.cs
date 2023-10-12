using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreGameNavigation : MonoBehaviour
{
    int sceneIndex;

    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log(sceneIndex);
    }
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Takes the user back if they press escape
        {
            GoBack();
        }
    }

    public void LoadNextScene() // Allows the user to go to the next scene
    {
        SceneManager.LoadScene(sceneIndex + 1);
    }

    public void GoBack() // Allows the user to go back to the previous scene
    {
        SceneManager.LoadScene(sceneIndex - 1);
    }

    public void LoadLevelOne() 
    {
        Debug.Log("Loading");
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
