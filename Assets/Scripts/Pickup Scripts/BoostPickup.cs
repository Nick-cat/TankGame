using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class BoostPickup : MonoBehaviour
    {
        [SerializeField] float restoreAmount;
        [SerializeField] GameObject boostRestoreEffect;
        private TankController target;
        private GameObject particleEffect;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.transform.root.GetComponent<TankController>();
                if (target.boostRemaining < target.boostMax)
                {
                    target.boostRemaining += restoreAmount;
                    if (boostRestoreEffect != null)
                    {
                        particleEffect = Instantiate(boostRestoreEffect, other.transform.position, Quaternion.identity);
                        particleEffect.transform.SetParent(other.transform.root);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }
}
