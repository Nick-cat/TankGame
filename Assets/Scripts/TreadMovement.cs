using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TreadMovement : MonoBehaviour

    {
        private Renderer treads;

        //get treadspeed from tank controller
        public TankController tank;

        // Start is called before the first frame update
        void Start()
        {
            treads = GetComponent<Renderer>();

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            float forwards = Mathf.Round(-Input.GetAxis("Vertical"));
            treads.material.SetVector("textureOffset", new Vector4(Time.time * tank.treadSpeed * forwards, 0f, 0f, 0f));
        }
    }
}
