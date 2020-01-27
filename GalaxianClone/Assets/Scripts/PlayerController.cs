﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private CharacterController controller;
    
    public float speed = 5f;
    public Rigidbody rb = null;

    //Used for projectiles

    [Header("Projectile")]
    public Projectile projectilePrefad;
    public Transform spawnPoint;
    public ProjectilePool projectilePool;
    public float fireSpeed= 0.5f;
    public float timeToNextFire;
    public GameObject explosionFX;


    [Header("Lives")]
    bool alreadyDead = false;
    public int startingLives;
    int currentLives;
    public int Lives
    {
        get
        {
            //Some other code
            return currentLives;
        }
        set
        {
            //Some other code
            currentLives = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Lives = startingLives;
        timeToNextFire = Time.time + fireSpeed;
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.y));
        objectWidth = 2;
        objectHeight = 2;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = GetMovement();
        Vector3 movement = new Vector3(moveHorizontal, 0.0f,0.0f);
        rb.velocity = movement * speed;

        if (GetFire() && Time.time >timeToNextFire && GameManager.instance.gameStarted)
        {
            timeToNextFire = Time.time + fireSpeed;
            Projectile clone;
            clone = projectilePool.GetProjectile();
            clone.transform.position = spawnPoint.transform.position;
            clone.gameObject.SetActive(true);
        }
    }

    private float GetMovement() {

        return Input.GetAxis("Horizontal");
    }

    private bool GetFire() {
       return Input.GetButtonDown("Fire1");
    }

    public void LooseLife() {

        Instantiate(explosionFX, transform.position, Quaternion.identity);
        Lives--;
        if (Lives <= 0 && !alreadyDead)
        {
            gameObject.SetActive(false);
            alreadyDead = true;
            Lives = 0;
            GameManager.instance.GameOver();

        } else if (alreadyDead) {
            Lives = 0;
        }
            GameManager.instance.UpdateUIText();
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1+ objectWidth, screenBounds.x - objectWidth);
        // viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
       // viewPos.z = Mathf.Clamp(viewPos.z, screenBounds.y + objectHeight, screenBounds.y* -1 - objectHeight);
        transform.position = viewPos;  
    }
}