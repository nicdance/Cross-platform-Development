using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.enemies.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
