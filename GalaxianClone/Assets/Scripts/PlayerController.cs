using System.Collections;
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
    public Projectile projectilePrefad;
    public Transform spawnPoint;
    public ProjectilePool projectilePool;
    public float fireSpeed= 0.5f;
    public float timeToNextFire;


    // Start is called before the first frame update
    void Start()
    {
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
       // if (GameManager.instance.gameStarted)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f,0.0f);
            rb.velocity = movement * speed;

            if (Input.GetButtonDown("Fire1") && Time.time >timeToNextFire && GameManager.instance.gameStarted)
            {
                timeToNextFire = Time.time + fireSpeed;
                Projectile clone;
                clone = projectilePool.GetProjectile();
                clone.transform.position = spawnPoint.transform.position;
                clone.gameObject.SetActive(true);
            }

        }
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