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
            if (startingHealth > 0) {
                setHealth(startingHealth);
            }
        }
        public void setHealth(int h) {
            Health = h;
        }
        public void processDamage(int dmg) {
            setHealth(Health - dmg);
        }
        public int Health = 6;
        private void Update()
        {
            if (Health <- 0) {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
