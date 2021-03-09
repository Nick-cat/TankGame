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
        private float forwardForce;
        private float driftDirection;

        //get suspension values from tank controller
        public TankController tank;

        //get gas input from TankAcceleration
        public TankAcceleration gas;
        
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
                rb.AddForceAtPosition(suspensionForce, Hit.point);


                if (rb.velocity.magnitude < tank.maxVelocity)
                {
                    //adds drift
                    bool drift = Input.GetButton("Drift");
                    float rotate = Input.GetAxis("Horizontal");
                    
                    if (drift) driftDirection = tank.driftAngle * rotate;
                    else driftDirection = 0;
                    
                    Vector3 moveDirection = Quaternion.AngleAxis(driftDirection, Vector3.up) * transform.forward;

                    //adds forward force to the tank at the wheels
                    forwardForce = gas.acceleration * tank.tankSpeed * 100f;
                    rb.AddForceAtPosition(forwardForce * moveDirection * Time.deltaTime, Hit.point, ForceMode.Acceleration);
                }
            }
            //displays the suspension
            Debug.DrawRay(transform.position, -transform.up * Hit.distance, Color.green);
            

        }
    }
}
