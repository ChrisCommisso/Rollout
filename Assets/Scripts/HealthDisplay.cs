using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject healthBar;
    Attackable healthPool;
    Units thisUnit;
    float maxhealth;
    float prevhealth;
    public float displayHealthFor;
    bool displaying;
    private void Awake()
    {
        if (healthPool == null)
        {
            healthPool = GetComponent<Attackable>();
            thisUnit = GetComponent<Units>();
        }
        prevhealth = healthPool.startingHealth;
        maxhealth = healthPool.startingHealth;
        healthBar.SetActive(false);
    }
    IEnumerator displayHealth(float forTime) {
        if (healthBar != null) {

            healthBar.SetActive(true);
            displaying = true;
            yield return new WaitForSeconds(forTime);
            displayHealthFor = 0;
            displaying = false;
            healthBar.SetActive(false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
       
        if(healthPool!=null)
        {
            healthBar.transform.position = transform.position+new Vector3(0, thisUnit.Height+.5f, 0);
            healthBar.transform.localScale = new Vector3(thisUnit.Width * (prevhealth/maxhealth), .5f, .5f);
            if (displayHealthFor > 0&&!displaying) {
                StartCoroutine(displayHealth(displayHealthFor));
            }
        }
        prevhealth = healthPool.Health;
    }
}
