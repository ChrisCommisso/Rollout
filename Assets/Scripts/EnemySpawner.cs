using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyCopy;
    GameObject enemyInstance;
    bool polling;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void pollSpawn() { 
    
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!polling) {
            StartCoroutine(pollSpawn());
        }
    }
}
