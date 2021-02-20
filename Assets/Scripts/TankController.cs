using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{

    public class TankController : MonoBehaviour
    {
        public float tankSpeed;
        public float rotateSpeed;
        public float brakeForce;
        public float maxVelocity;
        public Vector3 centerOfMass;
        private Rigidbody rb;
        public float distanceToGround = .5f;
        public Transform rayPoint;

        [Header("Suspension")]
        public float suspensionHeight;
        public float springTravel;
        public float springStiffness;
        public float damperStiffness;
        public float wheelRadius;

        private void Start()
        {
            rb = GetComponent <Rigidbody>();
            rb.centerOfMass = centerOfMass;

        }
        void FixedUpdate()
        {
            
            float forwards = Input.GetAxis("Vertical");
            float rotate = Input.GetAxis("Horizontal");
            bool brake = Input.GetButton("Brake");

            //old unused tank movement
            //Vector3 moveTank = new Vector3(0f, 0f, forwards * tankSpeed * Time.deltaTime);

            if (isGrounded()) {
                if (rb.velocity.sqrMagnitude < maxVelocity)
                {
                    rb.AddForce(transform.forward * forwards * tankSpeed * 100f);

                    Vector3 rotateAmount = new Vector3(0f, rotate * rotateSpeed * Time.deltaTime, 0f);
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateAmount);
                }
                if (brake)
                {
                    rb.angularDrag = 1.5f;
                    rb.drag = 3f;
                }
                else
                {
                    rb.angularDrag = 0;
                    rb.drag = 0;
                }
            }

            //I think lowering the centre of mass of the tank accomplishes this but I left it in just incase
            //rb.AddForce(-transform.up * 100f);

        }
        bool isGrounded(){
            //cast a ray from center of body to down direction, check if anything intersects the bottom of the body.

            //Doesn't need to be declared, this happens automatically when you cast the ray
            //RaycastHit hit;
            LayerMask ground = LayerMask.GetMask("Ground");

            bool val = Physics.Raycast(rayPoint.position, -transform.up, distanceToGround, ground);
            return val;
       }
    }
}
