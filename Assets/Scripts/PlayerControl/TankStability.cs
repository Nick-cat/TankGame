using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TankStability : MonoBehaviour
    {
        [Header("Stability Forces")]
        [Tooltip("Applies a downforce when some of the tread is on the ground but not all")]
        public float treadDownForce = 100;
        [Tooltip("Applies a torque when the tank is colliding with the ground but not on the treads")]
        public float antiRollForce = 100;

        private TankComponentManager tcm;

        private void Awake()
        {
            tcm = GetComponent<TankComponentManager>();
        }

        private void FixedUpdate()
        {
            ApplyTreadDownForce(tcm.rb, tcm.wheels, tcm.isGrounded, tcm.numberOfGroundedWheels);
            ApplyAntiRollForces(tcm.rb, tcm.averageColliderSurfaceNormal, tcm.isGrounded);
        }

        private void ApplyTreadDownForce(Rigidbody rigidbody, List<Transform> wheels, bool grounded, int numberOfGroundedWheels)
        {
            if (grounded && numberOfGroundedWheels < wheels.Count - 1)
            {
                Vector3 downwardForce = treadDownForce * Vector3.down * Time.fixedDeltaTime;
                foreach (var wheel in wheels)
                {
                    rigidbody.AddForceAtPosition(downwardForce, wheel.position, ForceMode.Acceleration);
                }
            }
        }

        private void ApplyAntiRollForces(Rigidbody rigidbody, Vector3 averageColliderSurfaceNormal, bool grounded)
        {
            if (averageColliderSurfaceNormal != Vector3.zero && !grounded)
            {
                //Gets the angle in order to determine the direction the vehicle needs to roll
                float angle = Vector3.SignedAngle(rigidbody.transform.up, averageColliderSurfaceNormal, rigidbody.transform.forward);
                Debug.Log(angle);

                //Angular stability only uses roll - Using multiple axis becomes unpredictable 
                Vector3 torqueAmount = Mathf.Sign(angle) * rigidbody.transform.forward * antiRollForce * Time.fixedDeltaTime;

                rigidbody.AddTorque(torqueAmount, ForceMode.Acceleration);
            }
        }
    }
}
