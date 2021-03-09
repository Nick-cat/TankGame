using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class HealthManager : MonoBehaviour
    {
        public float currentHealth;
        public float maxHealth;
        public bool fullHealth;

        void Start()
        {
            ResetHealth();
        }

        // Update is called once per frame
        void Update()
        {
            //makes sure to not heal object over max health
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            
            //is object dead?
            if (currentHealth <= 0)
            {
                //Respawns if player, otherwise it destroys the object
                if (gameObject.CompareTag("Player"))
                {
                    gameObject.GetComponent<TankController>().Respawn();
                    ResetHealth();
                }
                else Destroy(gameObject);
            }

            //checks and sets if the object is at full health
            if (currentHealth == maxHealth) fullHealth = true;
            else fullHealth = false;
        }

        public void Hurt(float damageDealt)
        {
            currentHealth -= damageDealt;
            Debug.Log(gameObject.name + " currentHealth = " + currentHealth);
        }

        public void Heal(float healAmount)
        {
            currentHealth += healAmount;
            Debug.Log(gameObject.name + " currentHealth = " + currentHealth);
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }

    }
}
