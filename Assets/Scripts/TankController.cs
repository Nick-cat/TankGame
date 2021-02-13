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
        public Rigidbody rb;
        public float distanceToGround = .5f;
        public LayerMask ground;
        public Transform rayPoint;
        void FixedUpdate()
        {
            float forwards = Input.GetAxis("Vertical");
            float rotate = Input.GetAxis("Horizontal");
            bool brake = Input.GetKey(KeyCode.Space);
            
            Vector3 moveTank = new Vector3(0f, 0f, forwards * tankSpeed * Time.deltaTime);
            if (isGrounded()) {
                if (rb.velocity.sqrMagnitude < maxVelocity)
                {
                    rb.AddForce(transform.forward * forwards * tankSpeed * 100f);
                    Vector3 rotateAmount = new Vector3(0f, rotate * rotateSpeed * Time.deltaTime, 0f);
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateAmount);
                }
            }

            rb.AddForce(-transform.up * 100f);
            if (brake) 
            {
                rb.angularDrag = 1.5f;
                rb.drag = 3f;
            } else {
                rb.angularDrag = 0;
                rb.drag = 0;
            }
        }

        bool isGrounded(){
            //cast a ray from center of body to down direction, check if anything intersects the bottom of the body.
            RaycastHit hit;
            bool val = Physics.Raycast(rayPoint.position, -transform.up, out hit, distanceToGround, ground);
            return val;
        }
    }
}
