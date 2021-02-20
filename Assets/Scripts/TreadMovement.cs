using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TreadMovement : MonoBehaviour

    {
        
        [SerializeField] float treadSpeed;
        private Renderer treads;
        private float treadposition;


        // Start is called before the first frame update
        void Start()
        {
            treads = GetComponent<Renderer>();

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            
            //treads.material.SetVector("textureOffset", );
            
        }
    }
}
