using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class Suspension : MonoBehaviour
    {
        private float minlength;
        private float maxLength;
        private float lastLength;
        private float springLength;
        private float springForce;
        private float damperForce;
        private float springVelocity;
        private Vector3 suspensionForce;
        private Rigidbody rb;
        private Vector3 wheelVelocityLocal;
        private float forwardForce;
        private float turnForce;



        //get suspension values from tank controller
        public TankController tank;
        

        void Start()
        {
            //get tank rigidbody
            rb = transform.root.GetComponent <Rigidbody>();
            minlength = tank.suspensionHeight - tank.springTravel;
            maxLength = tank.suspensionHeight + tank.springTravel;
            
        }
        void FixedUpdate()
        {
            
            //this provides suspension but does not actually move the wheels
            LayerMask ground = LayerMask.GetMask("Ground");
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit Hit, maxLength + tank.wheelRadius, ground))
            {
                lastLength = springLength;
                springLength = Hit.distance - tank.wheelRadius;
                springLength = Mathf.Clamp(springLength, minlength, maxLength);
                springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
                springForce = tank.springStiffness * (tank.suspensionHeight - springLength);
                damperForce = tank.damperStiffness * springVelocity;

                suspensionForce = (springForce + damperForce) * transform.up;

                wheelVelocityLocal = transform.InverseTransformDirection(rb.GetPointVelocity(Hit.point));

                if (rb.velocity.magnitude < tank.maxVelocity)
                {
                    forwardForce = Input.GetAxis("Vertical") * tank.tankSpeed * 100f;
                }




                rb.AddForceAtPosition(suspensionForce + (forwardForce * transform.forward * Time.deltaTime), Hit.point);
                //rb.AddForceAtPosition(suspensionForce, Hit.point);


            }
            Debug.DrawRay(transform.position, -transform.up * Hit.distance, Color.green);
        }
    }
}
