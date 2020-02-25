﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int score;
    public int divingScore;
    public AudioSource audio;


    // Path Details
    public Paths pathToFollow;
    public int currentWayPoint = 0;
    public float speed = 20;
    public float increaseSpeedBy = 10;
    public float reachDistance = 0.4f;
    public float rotationSpeed = 5f;
    public bool userBezier = false;
    public bool isDiving = false;
    public bool isDead = false;
    public bool isIdle = false;
    public bool setUpStart = true;
    public bool startDive = false;

    private float distance; // disance to next point
    public float WaitToIdle = 1.0f;

    public int enemyID;
    public Formation formation;

    //  STATE MACHINE HANDLING
    public enum EnemyState {
        ONPATH, // On a Path
        FLYIN,  // Spawning
        IDLE,
        DIVING
    }

    public EnemyState enemyState;

    public Transform spawnPoint;
    public GameObject ammo;
    float currentDelay;
    public float fireRate = 2.0f;
    Transform target;

    public GameObject explosionFX;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        fireRate += Random.Range(-.2f, .2f);
        target = GameObject.Find("Player").transform;
        GameManager.instance.activeEnemies.Add(this);
       // GameManager.instance.mainEnemies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.ONPATH:
                isDiving = false;
                MoveOnPath(pathToFollow);
                break;
            case EnemyState.FLYIN:
    //            isDiving = false;
                MoveToFormation();
                break;
            case EnemyState.IDLE:
                startDive = false;
                setUpStart = false;
                isDiving = false;
               // CheckInPosition();
                break;
            case EnemyState.DIVING:
                //      isIdle = false;
                if (startDive)
                {
                    startDive = false;
                    StartCoroutine("PlayDiveSound");
                }
                isDiving = true;
                MoveOnPath(pathToFollow);
                SpawnBullet();
                break;
            default:               
                break;
        }
        
    }

    public void IncreaseSpeed(float increaseMultiplier)
    {
        //Debug.Log("Speed Was: " + speed);
        speed = speed + (increaseSpeedBy * increaseMultiplier);
        //Debug.Log("Speed Now: " + speed);
        rotationSpeed = rotationSpeed + (increaseSpeedBy * increaseMultiplier);
    }
    IEnumerator PlayDiveSound() {
        audio.Play();
        return null;
    }

    bool  CheckInPosition() {

        if (Vector3.Distance(transform.position, formation.GetVectorPosition(enemyID)) <= 0.0001f) {
            return true;
        }
        return false;
    }

    void MoveToFormation() {
        transform.position = Vector3.MoveTowards(transform.position,formation.GetVectorPosition(enemyID), speed*Time.deltaTime);

        // Rotate Enemy
        Vector3 target = formation.GetVectorPosition(enemyID) - transform.position;
        if (target != Vector3.zero)
        {
            target.z = 0;
            target = target.normalized;
            Quaternion rotation = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            // transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f));
        }

        if (Vector3.Distance(transform.position, formation.GetVectorPosition(enemyID)) <=0.0001f)
        {
            transform.SetParent(formation.gameObject.transform);
            //transform.eulerAngles = new Vector3(90, 0, 180);
            transform.eulerAngles = new Vector3(0, 180, 0);

            Invoke("SetToIdle", WaitToIdle);
        }
    }

    void SetToIdle() {
        CancelInvoke("SetToIdle");
        if (CheckInPosition())
        {
            enemyState = EnemyState.IDLE;
        }
    }
    void MoveOnPath(Paths path)
    {

        if (userBezier)
        {
            //Debug.Log("Size of Path" + path.bezierObj.Count);
            // Move Enemy
            distance = Vector3.Distance(path.bezierObj[currentWayPoint],
                                        transform.position);
            transform.position = Vector3.MoveTowards(transform.position,
                                                     path.bezierObj[currentWayPoint],
                                                     speed * Time.deltaTime);

            // Rotate Enemy
            Vector3 target = path.bezierObj[currentWayPoint] - transform.position;

            if (target != Vector3.zero)
            {
                // target.z = 0;
                target.y = 0;
                target = target.normalized;
                Quaternion rotation = Quaternion.LookRotation(target);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }
        else {
            if (currentWayPoint >= path.pathObjList.Count)
            {
                currentWayPoint = 0;
            }

            distance = Vector3.Distance(path.pathObjList[currentWayPoint].position,
                                        transform.position);
            transform.position = Vector3.MoveTowards(transform.position,
                                                     path.pathObjList[currentWayPoint].position,
                                                     speed * Time.deltaTime);
            // Rotate Enemy
            Vector3 target = path.pathObjList[currentWayPoint].position - transform.position;
            if (target != Vector3.zero)
            {
                target.z = 0;
                target = target.normalized;
                Quaternion rotation = Quaternion.LookRotation(target);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
                // transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f));
            }
        }
        if (userBezier)
        {            
            if (distance <= reachDistance)
            {
                currentWayPoint++;
            }
            if (currentWayPoint >= path.bezierObj.Count)
            {
                currentWayPoint = 0;
                //if (enemyState == EnemyState.DIVING)
                if (GameManager.instance.gameStarted)
                {
                    transform.position = GameObject.Find("SpawnManager").transform.position;
                    Destroy(pathToFollow.gameObject);
                }
                enemyState = EnemyState.FLYIN;
            }
        }
        else
        {
            if (distance <= reachDistance)
            {
                currentWayPoint++;
            }
            if (currentWayPoint >= path.pathObjList.Count )
            {
                currentWayPoint = 0;
                //if (enemyState == EnemyState.DIVING)
                if (GameManager.instance.gameStarted)
                {
                    transform.position = GameObject.Find("SpawnManager").transform.position;
                    Destroy(pathToFollow.gameObject);
                }
                enemyState = EnemyState.FLYIN;
            }
        }
    }

    public void SpawnSetup(Paths Path, int id, Formation newFormation) {
        pathToFollow = Path;
        enemyID = id;
        formation = newFormation;
        gameObject.SetActive(true);
        //GameManager.instance.activeEnemies.Add(this);

       // transform.SetParent(null);
    }

    public void HitEnemy() {
        audio.Stop();
        isDead = true;
        GameManager.instance.activeEnemies.Remove(this);

        //    if (pathToFollow.gameObject != null)
        {
            //  if (GameManager.instance.gameStarted == true)
            if (isDiving == true && pathToFollow!=null)
            {
                Destroy(pathToFollow.gameObject);
            }

        }


        // PLay sound

        // show particles

        Instantiate(explosionFX, transform.position, Quaternion.identity);

        // increment score
        if (isDiving)
        {
            GameManager.instance.AddToScore(divingScore);
        }
        else {
            GameManager.instance.AddToScore(score);
        }


        //hide enemy
        // this.gameObject.SetActive(false);
        //transform.position = GameObject.Find("SpawnManager").transform.position;

        StopCoroutine("CheckLevel");
        StartCoroutine("CheckLevel");
        Destroy(this.gameObject);
    }

    IEnumerator CheckLevel() {
        //Debug.Log("checking Level");
        GameManager.instance.CheckNewLevel();
        yield return null;
    }

    public void SetDivePath(Paths path) {

        if (!isDead)
        {
            isDiving = true;
            pathToFollow = path;
            //Debug.Log(transform.parent.parent);
            transform.SetParent(null);
            startDive = true;
            enemyState = EnemyState.DIVING;

        }
    }

    void SpawnBullet() {
        currentDelay+= Time.deltaTime;
        if (currentDelay >= fireRate && ammo != null && spawnPoint != null) {
            //spawnPoint.LookAt(target);
            //float yAngle = Random.Range(-10,10);
            //spawnPoint.transform.Rotate(0,yAngle,0, Space.Self);


            Projectile clone;
            clone = EnemyProjectilePool.instance.GetProjectile();
            clone.transform.position = spawnPoint.transform.position;
            clone.transform.rotation = spawnPoint.rotation;
            clone.gameObject.SetActive(true);


            //Instantiate(ammo, spawnPoint.position, spawnPoint.rotation);


            //spawnPoint.LookAt(target);
            //Instantiate(ammo, spawnPoint.position, spawnPoint.rotation);
            currentDelay = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.LooseLife();
        }
    }
}
