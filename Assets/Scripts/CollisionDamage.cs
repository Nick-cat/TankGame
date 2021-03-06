using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class CollisionDamage : MonoBehaviour
    {
        private HealthManager healthManager;
        private void Start()
        {
            healthManager = GetComponent<HealthManager>();
        }
        private void OnCollisionEnter(Collision collision)
        {
            float collisionForce = Mathf.Round((collision.impulse.magnitude / Time.fixedDeltaTime) / 1000f);
            if (collisionForce > 10f)
            {
                healthManager.Hurt(collisionForce - 10f);
            }

        }
    }
}
