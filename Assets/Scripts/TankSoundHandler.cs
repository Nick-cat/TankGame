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

        void Start()
        {
            engine = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
            tank = transform.GetComponent<TankController>();
        }

        void Update()
        {
            //currently adjusts the pitch and volume of the sound based on the tank velocity
            engineSpeed = Mathf.InverseLerp(0, tank.maxVelocity, rb.velocity.magnitude);
            engine.pitch = Mathf.SmoothStep(0.75f, 1.5f, engineSpeed);
            engine.volume = Mathf.SmoothStep(0.4f, 1f, engineSpeed);
        }


    }
}
