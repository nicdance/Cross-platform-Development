using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Intervals")]
    public float enemySpawnInterval;    //time between enemy spawns
    public float waveSpawnInterval;     //time between wave spawns
    public float untilFirstSpawn;     //time between wave spawns
    int currentWave = 0;
    int enemyID = 0;
    int mediumID = 0;
    int largeID = 0;


    [Header("Prefabs")]
    public GameObject smallEnemyPrefab;
    public GameObject mediumEnemyPrefab;
    public GameObject largeEnemyPrefab;



    [Header("Formations")]
    public Formation smallFormation; // This will be for the smaller shoips
    public Formation mediumFormation; // This will be for the medium  shoips
    public Formation largeFormation; // This will be for the larger shoips

    [System.Serializable]   
    public class Wave {
        public GameObject[] pathPrefabs;

        public int spawnPerWave; // Number of ships to spawn per wave
        public int mediumPerWave; // Number of  medium ships to spawn per wave
        public int largePerWave; // Number of large ships to spawn per wave
    }


    [Header("Waves")]
    public List<Wave> waveList = new List<Wave>();

    public List<Paths> activePaths = new List<Paths>();



    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartSpawn", untilFirstSpawn);
    }

    IEnumerator SpawnWaves() {
        while (currentWave < waveList.Count)
        {
            for (int i = 0; i < waveList[currentWave].pathPrefabs.Length; i++)
            {
                GameObject newPathObject = Instantiate(waveList[currentWave].pathPrefabs[i], transform.position, Quaternion.identity) as GameObject;
                Paths newPath = newPathObject.GetComponent<Paths>();
                activePaths.Add(newPath);
            }
            // Small Ships first
            for (int i = 0; i < waveList[currentWave].spawnPerWave; i++)
            {
                GameObject newEnemy = Instantiate(smallEnemyPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.SpawnSetup(activePaths[PathsToTake()], enemyID, smallFormation);
                enemyID++;
                // pauses for spawn interval
                yield return new WaitForSeconds(enemySpawnInterval);
            }


            // Medium  Ships first
            for (int i = 0; i < waveList[currentWave].mediumPerWave; i++)
            {
                GameObject newEnemy = Instantiate(mediumEnemyPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.SpawnSetup(activePaths[PathsToTake()], mediumID, mediumFormation);
                mediumID++;
                // pauses for spawn interval
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            // Large  Ships first
            for (int i = 0; i < waveList[currentWave].largePerWave; i++)
            {
                GameObject newEnemy = Instantiate(largeEnemyPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
                enemyController.SpawnSetup(activePaths[PathsToTake()], largeID, largeFormation);
                largeID++;
                // pauses for spawn interval
                yield return new WaitForSeconds(enemySpawnInterval);
            }


            yield return new WaitForSeconds(waveSpawnInterval);
            currentWave++;

            foreach (Paths path in activePaths)
            {
                Destroy(path.gameObject);    
            }
            activePaths.Clear();
        }
    }

    void StartSpawn() {
        StartCoroutine("SpawnWaves");
        CancelInvoke("StartSpawn");
    }

    public int PathsToTake() {
        return (enemyID+mediumID+largeID)% activePaths.Count;
    }

    private void OnValidate()
    {
        int currentSmall = 0;
        int currentMedium = 0;
        int currentLarge = 0;
        for (int i = 0; i < waveList.Count; i++)
        {
            currentSmall += waveList[i].spawnPerWave;
            currentMedium += waveList[i].mediumPerWave;
            currentLarge += waveList[i].largePerWave;
        }
        Debug.Log("CURRENT TOTAL SMALL: " + currentSmall);
        Debug.Log("CURRENT TOTAL MEDIUM: " + currentMedium);
        Debug.Log("CURRENT TOTAL LARGE: " + currentLarge);
        if (currentSmall > 20)
        {
            Debug.LogError("<color=red>Error!!</color> Your Small Amount is too high! " + currentSmall + "/20");
        }
        if (currentMedium > 16)
        {
            Debug.LogError("<color=red>Error!!</color> Your Medium Amount is too high! " + currentMedium + "/16");
        }
        if (currentLarge > 4)
        {
            Debug.LogError("<color=red>Error!!</color> Your Large Amount is too high! " + currentLarge + "/4");
        }
    }
}
