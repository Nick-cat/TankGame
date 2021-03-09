using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] float restoreAmount;
    [SerializeField] GameObject healthRestoreEffect;
    private HealthManager target;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform.root.GetComponent<HealthManager>();
            if (!target.fullHealth)
            {
                target.Heal(restoreAmount);
                if (healthRestoreEffect != null) Instantiate(healthRestoreEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }

}
