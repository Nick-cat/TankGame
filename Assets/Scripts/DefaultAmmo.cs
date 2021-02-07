using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class DefaultAmmo : TankRound {

        protected AmmoType ammotype = AmmoType.Default;

        public override void OnHit () {
            // TODO: Add some explosion effect or something here.
            Destroy( gameObject );
        }
    }
}