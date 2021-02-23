using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{

    public class TankController : MonoBehaviour
    {
        public float tankSpeed;
        public float rotateSpeed;
        public float brakeForce = 2f;
        public float maxVelocity;
        public float jumpForce;
        public Vector3 centerOfMass;
        
        public float distanceToGround = .5f;
        public Transform rayPoint;

        public float customGravity = 2f;
        public float groundDrag = 3f;
        public float airDrag = 0f;

        public float treadSpeed;
        
        [Header("Suspension")]
        public float suspensionHeight;
        public float springTravel;
        public float springStiffness;
        public float damperStiffness;
        public float wheelRadius;

        private Rigidbody rb;
        private bool canJump = true;


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
            bool jump = Input.GetButtonDown("Jump");

            //custom gravity
            rb.AddForce(Vector3.up * -customGravity * 100f);
            
            if (jump && canJump)
            {
                rb.AddForce(transform.up * jumpForce * 100f);
                canJump = false;
                
            }
            if (IsGrounded())
            {
                canJump = true;
                //Debug.Log("isGrounded");

                if (rb.velocity.magnitude < maxVelocity)
                {
                    //Old Tank Movement
                    //rb.AddForce(transform.forward * forwards * tankSpeed * 100f * Time.deltaTime);
                    
                    //TankRotation
                    Vector3 rotateAmount = new Vector3(0f, rotate * rotateSpeed * Time.deltaTime, 0f);
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateAmount);
                }
                //TankBrake
                if (brake)
                {
                    rb.angularDrag = brakeForce;
                    rb.drag = brakeForce;
                    
                    // Experimental, comment this out if tank is too buggy
                    if (rb.velocity.sqrMagnitude > Mathf.Sqrt(maxVelocity))
                    {
                        //Debug.Log("drift");
                        rb.angularVelocity = new Vector3(0f, rotate * (rotateSpeed/2f) * Time.deltaTime, 0f);
                    }
                }
                else
                {
                    rb.angularDrag = groundDrag / 2;
                    rb.drag = groundDrag;
                }
            }
            else 
            {
                rb.drag = 0;
                rb.angularDrag = airDrag;

            }
        }
        bool IsGrounded(){
            //cast a ray from center of body to down direction, check if anything intersects the bottom of the body.

            LayerMask ground = LayerMask.GetMask("Ground");

            bool val = Physics.Raycast(rayPoint.position, -transform.up, distanceToGround, ground);
            return val;
            
       }
    }
}

