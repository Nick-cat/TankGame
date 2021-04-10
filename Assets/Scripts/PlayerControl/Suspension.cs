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
        private float springForce;
        private float damperForce;
        private float springVelocity;
        private Vector3 suspensionForce;

        //get suspension values from tank controller
        public TankController tank;
        private TankComponentManager tcm;

        private void Awake()
        {
            tcm = GetComponent<TankComponentManager>();
            tank = GetComponent<TankController>();
        }

        void Start()
        {
            minlength = tank.suspensionHeight - tank.springTravel;
            maxLength = tank.suspensionHeight + tank.springTravel;
        }

        void FixedUpdate()
        {
            LayerMask ground = LayerMask.GetMask("Ground");
            ApplySuspension(tcm.wheelsLeft, tcm.wheelsRight, ground, tcm.rb);
        }

        float CalculateSuspension(float springLength, RaycastHit Hit)
        {
            lastLength = springLength;
            springLength = Hit.distance - tank.wheelRadius;
            springLength = Mathf.Clamp(springLength, minlength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = tank.springStiffness * (tank.suspensionHeight - springLength);
            damperForce = tank.damperStiffness * springVelocity;
            return springForce + damperForce;
        }

        private void ApplySuspension(Transform[] wheelsLeft, Transform[] wheelsRight,LayerMask ground, Rigidbody rb)
        {
            tcm.numberOfGroundedWheelsLeft = 0;
            foreach (var wheel in wheelsLeft)
            {
                WheelInfo wheelInfo = wheel.GetComponent<WheelInfo>();
                if (Physics.Raycast(wheel.position, -transform.up, out RaycastHit Hit, maxLength + tank.wheelRadius, ground))
                {
                    suspensionForce = CalculateSuspension(wheelInfo.springLength, Hit) * wheel.up;
                    rb.AddForceAtPosition(suspensionForce, Hit.point);

                    Debug.DrawRay(wheel.position, -wheel.up * Hit.distance, Color.green);

                    tcm.isGroundedLeft = true;
                    tcm.numberOfGroundedWheelsLeft += 1;
                } 
            }

            tcm.numberOfGroundedWheelsRight = 0;
            foreach (var wheel in wheelsRight)
            {
                WheelInfo wheelInfo = wheel.GetComponent<WheelInfo>();
                if (Physics.Raycast(wheel.position, -transform.up, out RaycastHit Hit, maxLength + tank.wheelRadius, ground))
                {
                    suspensionForce = CalculateSuspension(wheelInfo.springLength, Hit) * wheel.up;
                    rb.AddForceAtPosition(suspensionForce, Hit.point);

                    Debug.DrawRay(wheel.position, -wheel.up * Hit.distance, Color.green);

                    tcm.isGroundedRight = true;
                    tcm.numberOfGroundedWheelsRight += 1;
                }
            }

        }
    }
}
