using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TreadMovement : MonoBehaviour

    {
        private Renderer treads;

        //get treadspeed from tank controller
        [SerializeField] TankController tank;

        void Start()
        {
            treads = GetComponent<Renderer>();
        }

        void FixedUpdate()
        {
            float forwards = Mathf.Round(-Input.GetAxis("Vertical"));
            treads.material.SetVector("textureOffset", new Vector4(Time.time * tank.treadSpeed * forwards, 0f, 0f, 0f));
        }
    }
}
