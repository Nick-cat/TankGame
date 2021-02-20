using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class ExplosionSoundHandler : MonoBehaviour {

        [SerializeField] List<AudioClip> explosions = new List<AudioClip>();

        // Start is called before the first frame update
        void Start () {
            GetComponent<AudioSource>().PlayOneShot( explosions[Random.Range( 0 , explosions.Count - 1 )] );
        }
    }
}