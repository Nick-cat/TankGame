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

            //checks and sets if the player is at full health
            if (currentHealth == maxHealth) fullHealth = true;
            else fullHealth = false;
        }

        public void Hurt(float damageDealt)
        {
            currentHealth -= damageDealt;
            Debug.Log("CurrentHealth =" + currentHealth);
        }

        public void Heal(float healAmount)
        {
            currentHealth += healAmount;

            //makes sure to not heal the target over max health
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            Debug.Log("CurrentHealth =" + currentHealth);
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }

    }
}
