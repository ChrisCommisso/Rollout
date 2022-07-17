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
    IEnumerator pollSpawn() {
        polling = true;
        
        if (enemyInstance == null) {
        enemyInstance = Instantiate(enemyCopy,transform.position,transform.rotation);
        }
        yield return new WaitForSeconds(15);
        polling = false;

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!polling) {
            StartCoroutine(pollSpawn());
        }
    }
}
