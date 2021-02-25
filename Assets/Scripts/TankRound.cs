using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public abstract class TankRound : MonoBehaviour {
        public enum AmmoType {
            Default,
            Knockback,
            BouncyBall,
        }
        public float projectileSpeed = 250f;
        public float projectileForce = 3000f;

        protected AmmoType ammoType;

        public virtual void Shoot(Vector3 worldpos, Vector3 dir, float force) {
            transform.position = worldpos;
            GetComponent<Rigidbody>().velocity = dir * force;
		}

        public abstract void OnHit ();

		public virtual void OnCollisionEnter ( Collision collision ) {
            OnHit();
		}
	}
}