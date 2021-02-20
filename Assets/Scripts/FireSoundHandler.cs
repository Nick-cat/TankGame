using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class FireSoundHandler : MonoBehaviour {

        [SerializeField] List<AudioClip> gunSounds = new List<AudioClip>();
        private AudioSource source;

		private void Start () {
			source = GetComponent<AudioSource>();
		}

		public void Fire() {
			source.PlayOneShot( gunSounds[Random.Range( 0 , gunSounds.Count - 1 )] );
		}
    }
}