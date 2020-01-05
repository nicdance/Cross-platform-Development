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

    public List<EnemyController> enemies;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUIText();
        enemies = new List<EnemyController>();
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

    public bool CheckEnemiesActive() {

        foreach ( EnemyController enemy in enemies)
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
        foreach (EnemyController enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
        }
        
    }
}
