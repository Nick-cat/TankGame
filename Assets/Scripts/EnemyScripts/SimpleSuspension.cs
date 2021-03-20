using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class SimpleSuspension : MonoBehaviour
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

        //get suspension values
        public GameObject enemy;
        private SimpleAI simpleAI;
        
        void Start()
        {
            //get rigidbody
            rb = enemy.GetComponent<Rigidbody>();

            //get simpleAI
            simpleAI = enemy.GetComponent<SimpleAI>();

            minlength = simpleAI.suspensionHeight - simpleAI.springTravel;
            maxLength = simpleAI.suspensionHeight + simpleAI.springTravel;
        }
        public void Suspension()
        {
            //this provides suspension
            LayerMask ground = LayerMask.GetMask("Ground");
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit Hit, maxLength + simpleAI.wheelRadius, ground))
            {
                lastLength = springLength;
                springLength = Hit.distance - simpleAI.wheelRadius;
                springLength = Mathf.Clamp(springLength, minlength, maxLength);
                springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
                springForce = simpleAI.springStiffness * (simpleAI.suspensionHeight - springLength);
                damperForce = simpleAI.damperStiffness * springVelocity;

                suspensionForce = (springForce + damperForce) * transform.up;
                rb.AddForceAtPosition(suspensionForce, Hit.point);

                //Movement
                if (simpleAI.CalculateDistance() > simpleAI.strafeDistance)
                {
                    if (rb.velocity.magnitude < simpleAI.maxSpeed)
                    {
                        Vector3 forwards = transform.forward * simpleAI.moveSpeed * Time.deltaTime * 100f;
                        rb.AddForceAtPosition(forwards, Hit.point, ForceMode.Acceleration);
                    }
                }
                else
                {
                    //makes the enemy strafe around the player when within strafeDistance
                    Vector3 strafe = transform.right * simpleAI.moveSpeed * Time.deltaTime * 50f * simpleAI.rotateDirection;
                    rb.AddForceAtPosition(strafe, Hit.point, ForceMode.Acceleration);
                }
            }
            //displays the suspension
            Debug.DrawRay(transform.position, -transform.up * Hit.distance, Color.green);
        }
    }
}
