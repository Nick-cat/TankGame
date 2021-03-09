using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool fullHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) Destroy(gameObject);
        
        if (currentHealth == maxHealth) fullHealth = true;
        else fullHealth = false;
    }

    public void Hurt (float damageDealt)
    {
        currentHealth -= damageDealt;
        Debug.Log("CurrentHealth =" + currentHealth);
    }

    public void Heal(float healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log("CurrentHealth =" + currentHealth);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

}
