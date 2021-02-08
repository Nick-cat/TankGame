using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    // A script for objects meant to be instantiated and then deleted after some time.
    public class TempObject : MonoBehaviour {
		// Duration of object, in seconds.
        [SerializeField] float duration;

        private float curTime;

		private void Update () {
			curTime += Time.deltaTime;
			if ( curTime > duration ) Destroy( gameObject );
		}
	}
}