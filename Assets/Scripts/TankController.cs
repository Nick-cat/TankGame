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
        public Rigidbody rb;

        void FixedUpdate()
        {
            float forwards = Input.GetAxis("Vertical");
            float rotate = Input.GetAxis("Horizontal");
            bool brake = Input.GetKey(KeyCode.Space);
            //Vector3 moveTank = transform.position + (transform.forward * forwards * tankSpeed * Time.deltaTime);
            Vector3 moveTank = new Vector3(0f, 0f, forwards * tankSpeed * Time.deltaTime);
            //Quaternion rotateTank = transform.rotation * Quaternion.Euler(Vector3.up * (rotateSpeed * rotate * Time.deltaTime));
            // rb.MovePosition(moveTank);

            rb.AddRelativeForce(moveTank);
            rb.AddRelativeTorque(rotate * Vector3.up * rotateSpeed);

        }
    }
}
