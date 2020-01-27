using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float speed;
    private Rigidbody rb;
    public AudioSource audio;

    public enum Target {
        PLAYER,
        ENEMY,
    }

    public Target target;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        audio.Play();
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
    //private void OnCollisionEnter(Collision collision)       
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
    //        enemy.HitEnemy();
    //    }
    //    // MusicManager.instance.PlaySplatSound();
    // //   GameManager.instance.CheckEnemiesActive();
    //    DestroyProjectile();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (target == Target.ENEMY)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
                enemy.HitEnemy();
            }
        }
        else
        if (target == Target.PLAYER)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                player.LooseLife();
            }
        }
        // MusicManager.instance.PlaySplatSound();
        //   GameManager.instance.CheckEnemiesActive();
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
