using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ProjectilePool found!");
            return;
        }
        instance = this;
    }

    #endregion


    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelText;

    public int score = 0;
    public int level = 1;

    public List<EnemyController> activeEnemies;
    public List<EnemyController> mainEnemies;

    public bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUIText();
        activeEnemies = new List<EnemyController>();
        mainEnemies = new List<EnemyController>();
    }

    // Updates the UI text
    public void UpdateUIText()
    {
        scoreText.text = "SCORE: " + score;
        levelText.text = "Level: " + level;
    }

    public void AddToScore(int pointsScored) {
        score += pointsScored;
        UpdateUIText();
    }


    public void NextLevel()
    {
        level ++;
        UpdateUIText();
        ResetEnemies();
    }

    public bool AreEnemiesAlive()
    {

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
        ResetEnemies();
        return false;
    }


    public void ResetEnemies()
    {
        foreach (EnemyController enemy in activeEnemies)
        {
            enemy.gameObject.SetActive(true);
        }
        
    }

    public bool CheckIdle() {
        foreach (var enemy in activeEnemies)
        {
            if (enemy.enemyState != EnemyController.EnemyState.IDLE && enemy.gameObject.activeSelf)
            {
                return false;
            }
        }
        gameStarted = true;
        return true;
    }
}
