using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class DefaultAmmo : TankRound {

        protected AmmoType ammotype = AmmoType.Default;

        [SerializeField] private GameObject explosionEffectObject;
        [SerializeField] float explosionRadius = 2;
        [SerializeField] float explosionForce = 100;

        public override void OnHit () {
            // Explosion force.
            Vector3 pos = transform.position;
            Collider[] cols = Physics.OverlapSphere(pos, explosionRadius);
            foreach ( Collider c in cols ) {
                Rigidbody rb = c.attachedRigidbody;
                if ( rb != null ) rb.AddExplosionForce( explosionForce , pos , explosionRadius , 1.0f );
            }

            if ( explosionEffectObject != null ) Instantiate( explosionEffectObject , transform.position , Quaternion.identity );
            Destroy( gameObject );
        }
    }
}