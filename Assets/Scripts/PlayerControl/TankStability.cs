using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStability : MonoBehaviour
{
    [Header("Stability Forces")]
    public float linearStabilityForce = 100;
    public float angularStabilityForce = 100;

    private TankComponentManager tcm;

    private void Awake()
    {
        tcm = GetComponent<TankComponentManager>();
    }

    private void FixedUpdate()
    {
        ApplyWheelDownForce(tcm.rb, tcm.wheels, tcm.isGrounded, tcm.numberOfGroundedWheels);
        ApplyAntiRollForces(tcm.rb, tcm.averageColliderSurfaceNormal, tcm.isGrounded);
    }

    private void ApplyWheelDownForce(Rigidbody rigidbody, Transform[] wheels, bool grounded, int numberOfGroundedWheels)
    {
        if (grounded && numberOfGroundedWheels < wheels.Length -1)
        {
            Vector3 downwardForce = linearStabilityForce * Vector3.down * Time.fixedDeltaTime;
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
            Vector3 torqueAmount = Mathf.Sign(angle) * rigidbody.transform.forward * angularStabilityForce * Time.fixedDeltaTime;

            rigidbody.AddTorque(torqueAmount, ForceMode.Acceleration);
        }
    }
}
