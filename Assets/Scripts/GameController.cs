using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            ResumeGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game Paused");
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game Resumed");
    }

    void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
