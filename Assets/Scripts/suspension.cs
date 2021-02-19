using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class suspension : MonoBehaviour
    {
        private Rigidbody rb;

        public float restLength;
        public float springTravel;
        public float springStiffness;
        public float damperStiffness;
        private float minlength;
        private float maxLength;
        private float lastLength;
        private float springLength;
        private float springForce;
        private float damperForce;
        private float springVelocity;
        public float wheelRadius;
        private Vector3 suspensionForce;

        


        void Start()
        {
            rb = transform.root.GetComponent<Rigidbody>();

            minlength = restLength - springTravel;
            maxLength = restLength + springTravel;
        }
        void FixedUpdate()
        {
            if(Physics.Raycast(transform.position, -transform.up, out RaycastHit Hit, maxLength + wheelRadius))
            {
                lastLength = springLength;
                springLength = Hit.distance - wheelRadius;
                springLength = Mathf.Clamp(springLength, minlength, maxLength);
                springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
                springForce = springStiffness * (restLength - springLength);
                damperForce = damperStiffness * springVelocity;

                suspensionForce = (springForce + damperForce) * transform.up;

                rb.AddForceAtPosition(suspensionForce, Hit.point);

            }            
        }
    }
}
