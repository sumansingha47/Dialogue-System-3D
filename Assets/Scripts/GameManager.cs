using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
     
    public Button playButton;
    public Button pauseButton;
    public Button exitButton;

    public bool isPaused = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

        playButton.onClick.AddListener(ResumeGame);
        pauseButton.onClick.AddListener(PauseGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            ResumeGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game Paused");

        playButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game Resumed");

        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit();
    }
}
