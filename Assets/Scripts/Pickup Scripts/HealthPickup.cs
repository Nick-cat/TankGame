using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class HealthPickup : MonoBehaviour
    {
        [SerializeField] float restoreAmount;
        [SerializeField] GameObject healthRestoreEffect;
        private HealthManager target;
        private GameObject particleEffect;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.transform.root.GetComponent<HealthManager>();
                if (!target.FullHealth)
                {
                    target.Hurt(-restoreAmount);
                    if (healthRestoreEffect != null)
                    {
                        particleEffect = Instantiate(healthRestoreEffect, other.transform.position, Quaternion.identity);
                        particleEffect.transform.SetParent(other.transform.root);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
