using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private CharacterController controller;

    public float speed = 5f;
    public Rigidbody rb = null;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        objectWidth = 1;
        objectHeight = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.velocity = movement * speed;
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;  
    }
}