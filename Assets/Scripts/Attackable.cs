using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine;

namespace Assets.Scripts
{

    public class Attackable : MonoBehaviour
    {
        public int startingHealth;
        public void Start()
        {
            if (startingHealth > 0)
            {
                setHealth(startingHealth);
            }
            else {
                print("set starting health");
                setHealth(0);
            }
        }
        public void setHealth(int h) {
            Health = h;
        }
        public void processDamage(int dmg) {
            
            GetComponent<HealthDisplay>().displayHealthFor = 4;
            setHealth(Health - dmg);
        }
        public int Health;
        private void Update()
        {
            if (Health <- 0) {
                if (UnityEngine.Random.value > .5 && GetComponent<Units>().allegiance == Units.Allegiance.Enemy)
                {
                    //play VOICE LINE
                    Health = startingHealth;
                    GetComponent<Units>().allegiance = Units.Allegiance.Friendly;
                }
                else
                {
                    GameObject.Destroy(this.transform.parent.gameObject);
                }
            }
        }
    }
}
