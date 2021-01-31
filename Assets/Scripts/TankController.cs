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
    
        public List<WheelCollider> movementWheels;
        public List<WheelCollider> rotateWheels;

        void FixedUpdate()
        {
            float forwards = Input.GetAxis("Vertical");
            float rotate = Input.GetAxis("Horizontal");
            bool brake = Input.GetKey(KeyCode.Space);
            foreach (WheelCollider wheel in movementWheels)
            {
                wheel.motorTorque = tankSpeed * forwards * Time.deltaTime;
            }

            foreach (WheelCollider wheel in rotateWheels)
            {
                wheel.steerAngle = rotateSpeed * rotate * Time.deltaTime;
            }

            if (brake) 
            {
                foreach (WheelCollider wheel in movementWheels)
                {
                    wheel.brakeTorque = brakeForce;
                }
            } else 
            {
                foreach (WheelCollider wheel in movementWheels)
                {
                    wheel.brakeTorque = 0f;
                }
            }

        }
    }
}
