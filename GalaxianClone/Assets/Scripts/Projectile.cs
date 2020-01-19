﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // rb.velocity = Vector3.up * speed;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (transform.position.z > 42)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)       
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            enemy.HitEnemy();
        }
        // MusicManager.instance.PlaySplatSound();
        GameManager.instance.CheckEnemiesActive();
        DestroyProjectile();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.HitEnemy();
        }
        // MusicManager.instance.PlaySplatSound();
        GameManager.instance.CheckEnemiesActive();
        DestroyProjectile();
    }

    /*
    Instantiates the Explosion effect and then destroys the rock object
*/
    void DestroyProjectile()
    {
        // Instantiate(this.effect, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

}
