using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    #region Singleton
    public static EnemyPool instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EnemyPool found!");
            return;
        }
        instance = this;
    }

    #endregion


    public EnemyController pooledEnemy;
    private bool notEnoughEnemiesInPool = true;


    public  List<EnemyController> enemies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<EnemyController>();
    }

    public bool isEnemyActive() {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public EnemyController GetEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].gameObject.activeInHierarchy)
            {
                return enemies[i];
            }
        }

        if (notEnoughEnemiesInPool)
        {
            EnemyController newEnemy = Instantiate(pooledEnemy, transform.position, Quaternion.identity);
            newEnemy.gameObject.SetActive(false);
            newEnemy.gameObject.transform.SetParent(gameObject.transform);
            enemies.Add(newEnemy);
            return newEnemy;
        }
        return null;
    }
}
