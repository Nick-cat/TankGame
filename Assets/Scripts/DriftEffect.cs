using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class DriftEffect : MonoBehaviour
    {
        [SerializeField] List<ParticleSystem> dirt;
        [SerializeField] List<AudioClip> driftSounds = new List<AudioClip>();
        [SerializeField] AudioSource driftStartSource;
        private AudioSource driftLoop;
        private Rigidbody rb;
        private TankController tank;
        private bool drift;
        private float turn;
        private bool isDrifting = false;

        private void Start()
        {
            rb = transform.root.GetComponent<Rigidbody>();
            tank = transform.root.GetComponent<TankController>();
            driftLoop = GetComponent<AudioSource>();
        }
        private void Update()
        {
            drift = Input.GetButton("Drift");
            turn = Input.GetAxis("Horizontal");

            if ((rb.velocity.magnitude > 0) && drift && tank.IsGrounded())
            {
                Drift();
                return;
            }
            NotDrift();
        }
        void Drift()
        {
            if (turn != 0)
            {
                if (isDrifting == false && !driftLoop.isPlaying)
                {
                    driftStartSource.Play();
                    isDrifting = true;
                }
                foreach (ParticleSystem d in dirt) d.Play();
                driftLoop.pitch = 1 + Mathf.Sin(tank.driftAngle * Time.deltaTime);
                if (!driftLoop.isPlaying) driftLoop.PlayOneShot(driftSounds[Random.Range(0, driftSounds.Count - 1)], Random.Range(0.6f, .7f));
            }
        }
        void NotDrift()
        {
            foreach (ParticleSystem d in dirt) d.Stop();
            driftLoop.Stop();
            isDrifting = false;
        }
    }
}
