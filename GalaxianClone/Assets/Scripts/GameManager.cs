using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;
    private PlayerController player;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ProjectilePool found!");
            Destroy(this.gameObject);
            return;
        }
        instance = this;

      //  DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    public Animator readyText;
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

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerController>();
        UpdateUIText();
        activeEnemies = new List<EnemyController>();
      //  mainEnemies = new List<EnemyController>();
    }

    public void GameOver() {
        GameOverUI.SetActive(true);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    // Updates the UI text
    public void UpdateUIText()
    {
        scoreText.text = "SCORE: " + score;
        levelText.text = "Level: " + level;
        livesText.text = "Lives: " + player.Lives;
    }

    public void UpdateAndShowLevelText()
    {
        levelLoadText.text = "Level " + level;
        // levelDisplayText.ResetTrigger("fadeOut");
        // levelDisplayText.SetTrigger("fadeIn");
        levelDisplayText.SetBool("fadeOut", false);
        levelDisplayText.SetBool("fadeIn", true);
    }

    public void HideLevelText()
    {
        //levelDisplayText.ResetTrigger("fadeIn");
        //levelDisplayText.SetTrigger("fadeOut");
        levelDisplayText.SetBool("fadeIn", false);
        levelDisplayText.SetBool("fadeOut", true);
    }


    public void AddToScore(int pointsScored) {
        score += pointsScored;
        UpdateUIText();
    }


    public void NextLevel()
    {
        Debug.Log("Next Level Called");
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
        Debug.Log("ResetEnemies");
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
        Debug.Log("CheckIdle");
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
