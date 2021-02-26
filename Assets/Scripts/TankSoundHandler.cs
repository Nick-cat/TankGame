using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{

    public class TankSoundHandler : MonoBehaviour
    {
        private AudioSource engine;
        private float engineSpeed = 0f;
        private Rigidbody rb;
        private TankController tank;

        // Start is called before the first frame update
        void Start()
        {
            engine = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
            tank = transform.GetComponent<TankController>();
        }

        // Update is called once per frame
        void Update()
        {
            engineSpeed = Mathf.InverseLerp(0, tank.maxVelocity, rb.velocity.magnitude);
            engine.pitch = Mathf.SmoothStep(0.75f, 1.5f, engineSpeed);
            engine.volume = Mathf.SmoothStep(0.4f, 1f, engineSpeed);
        }


    }
}
