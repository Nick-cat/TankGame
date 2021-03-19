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
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                target = other.transform.root.GetComponent<TankController>();
                if (target.boostRemaining < target.boostTimer)
                {
                    target.boostRemaining += restoreAmount;
                    Debug.Log(restoreAmount);
                    if (boostRestoreEffect != null) Instantiate(boostRestoreEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}
