using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class BounceAmmo : TankRound
    {
        protected AmmoType ammotype = AmmoType.BouncyBall;

        [SerializeField] private GameObject explosionEffectObject;
        public float explosionRadius = 2;
        public float explosionForce = 100;
        [SerializeField] int bouncesUntilDestroy = 3;

        public override void OnHit()
        {
            // Explosion force.
            Vector3 pos = transform.position;
            Collider[] cols = Physics.OverlapSphere(pos, explosionRadius);
            foreach (Collider c in cols)
            {
                Rigidbody rb = c.attachedRigidbody;
                if (rb != null) rb.AddExplosionForce(explosionForce, pos, explosionRadius, 1.0f);

                //deal damage
                if (c.gameObject.CompareTag("Enemy"))
                {
                    c.gameObject.GetComponent<HealthManager>().Hurt(damage);
                }
            }
            bouncesUntilDestroy -= 1;

            if (explosionEffectObject != null) Instantiate(explosionEffectObject, transform.position, Quaternion.identity);
            if(bouncesUntilDestroy == 0) Destroy(gameObject);
        }
    }
}
