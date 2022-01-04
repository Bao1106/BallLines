using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static bool isGameEnded;
    static bool isPaused = false;

    public GameObject gameOver;
    public GameObject pauseMenu;

    public void GameOver()
    {
        if(isGameEnded == false)
        {
            isGameEnded = true;
            gameOver.SetActive(true);
            //Restart Game
            Invoke("Restart", 5f);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
