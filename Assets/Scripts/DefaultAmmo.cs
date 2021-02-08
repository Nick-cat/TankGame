using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class DefaultAmmo : TankRound {

        protected AmmoType ammotype = AmmoType.Default;

        [SerializeField] private GameObject explosionEffectObject;

        public override void OnHit () {
            // TODO: Add some explosion effect or something here.
            if ( explosionEffectObject != null ) Instantiate( explosionEffectObject , transform.position , Quaternion.identity );
            Destroy( gameObject );
        }
    }
}