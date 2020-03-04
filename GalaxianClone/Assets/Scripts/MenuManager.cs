using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{


    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OptionsScene()
    {
        SceneManager.LoadScene("OptionsScene");
    }


    public void HighScoreScene()
    {
        SceneManager.LoadScene("HighScore");
    }



    public void GameInfoScene()
    {
        SceneManager.LoadScene("GameInfo");
    }



    public void GameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }


    public void MainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void ExitGame() {
        Application.Quit();
    }
}
