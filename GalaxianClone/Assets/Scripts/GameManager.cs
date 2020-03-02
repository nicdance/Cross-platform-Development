

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

///
///    \brief Game Manager Holds core detalis of the Game.
///
///    And update  the UI Elements as required. 
///
public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance; /// Singleton Game Manage object which is accessable from any object.
    private PlayerController player; /// Player Stores aReference to the player.

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ProjectilePool found!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    #endregion

    public Animator readyText; /// Reference to the Ready Text
    public Animator setText;
    public Animator goText;
    public Animator levelDisplayText;


    public TextMeshProUGUI levelLoadText;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI livesText;

    public GameObject GameOverUI;

    public int score = 0;
    public int level = 1;

    public List<EnemyController> activeEnemies;
    public List<EnemyController> mainEnemies;

    public bool gameStarted;

    public SpawnManager spawnManager;

    public Image[] lives;

    ///
    ///Start is called before the first frame update
    /// 
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerController>();
        UpdateUIText();
        activeEnemies = new List<EnemyController>();
      //  mainEnemies = new List<EnemyController>();
    }

    ///
    ///   Displays the Game Over Screen 
    ///
    public void GameOver() {
        GameOverUI.SetActive(true); 
    }


    ///
    ///   Loads the Main Menu 
    ///
    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }


    ///
    ///   Update the UI Score, Level and Playe Lives values
    ///
    public void UpdateUIText()
    {
        scoreText.text = "SCORE: " + score;
        levelText.text = "Level: " + level;
        livesText.text = "Lives: " + player.Lives;
        UpdateLives();
    }

    ///
    /// Updates the UI element showing current lives.
    ///
    private void UpdateLives() {
        for (int ii = 0; ii < lives.Length; ii++)
        {
            if (ii <= player.Lives - 1)
            {
                lives[ii].gameObject.SetActive(true);
            }
            else {
                lives[ii].gameObject.SetActive(false);
            }
        }
    }

    ///
    ///  Shows the Level title 
    ///
    public void UpdateAndShowLevelText()
    {
        levelLoadText.text = "Level " + level;
        levelDisplayText.SetBool("fadeOut", false); /// Fade
        levelDisplayText.SetBool("fadeIn", true);
    }


    ///
    ///  Hides the Level Title 
    ///
    public void HideLevelText()
    {
        levelDisplayText.SetBool("fadeIn", false);
        levelDisplayText.SetBool("fadeOut", true);
    }


    public void AddToScore(int pointsScored) {
        score += pointsScored;
        UpdateUIText();
    }


    public void NextLevel()
    {
        //Debug.Log("Next Level Called");
        level ++;
        UpdateUIText();
        ResetEnemies();

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void CheckNewLevel()
    {
        if (!AreEnemiesAlive())
            {
            Debug.Log("All Enemies Dead");
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DivePath"))
            {
                Destroy(obj);
            }
            NextLevel();
        }
        else
        {
            Debug.Log("Some Enemies Stil Alive");
        }
    }


    /**
     * Checks If all enenmies are alive
     * @return bool if all enemmies alive
     */
    public bool AreEnemiesAlive()
    {
        if (activeEnemies.Count <1)
        {
            return false;
        }
        foreach (EnemyController enemy in activeEnemies)
        {
            if (!enemy.isDead)
            {
                return true;
            }
        }
        return false;

    }
    public bool CheckEnemiesActive() {

        foreach ( EnemyController enemy in activeEnemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                return true;
            }
        }

        if (EnemyPool.instance.isEnemyActive())
        {
            return true;
        }
        NextLevel();
       // ResetEnemies();
        return false;
    }


    public void ResetEnemies()
    {
        StopAllCoroutines();
        //Debug.Log("ResetEnemies");
        activeEnemies.Clear();
      //  EnemyPool.instance.ResetEnemies();
        gameStarted = false;
        spawnManager.ResetWave();
        //foreach (EnemyController enemy in activeEnemies)
        //{
        //    enemy.gameObject.SetActive(true);
        //}
        
    }

    public bool CheckIdle() {
        //Debug.Log("CheckIdle");
        foreach (var enemy in activeEnemies)
        {
            if (enemy.enemyState != EnemyController.EnemyState.IDLE)
            {
                return false;
            }
        }
        gameStarted = true;
        return true;
    }
}
