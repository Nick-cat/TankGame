using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class HealthManager : MonoBehaviour
    {
        private float currentHealth;
        [SerializeField] float maxHealth;

        public float CurrentHealth { get { return currentHealth; } }
        public float MaxHealth { get { return maxHealth; } }
        public bool FullHealth { get { return currentHealth == maxHealth; } }

        void Start()
        {
            ResetHealth();
        }

        void Update()
        {
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
        }

        public void Hurt(float damageDealt)
        {
            currentHealth = Mathf.Min(currentHealth - damageDealt, maxHealth);
            Debug.Log(gameObject.name + " currentHealth = " + currentHealth);
        }

        //public void Heal(float healAmount)
        //{
        //    currentHealth += healAmount;
        //    Debug.Log(gameObject.name + " currentHealth = " + currentHealth);
        //}

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }
    }
}
