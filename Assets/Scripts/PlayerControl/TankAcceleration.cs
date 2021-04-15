using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {

    public class TankAcceleration : MonoBehaviour
    {
        public AnimationCurve velocityCurve;

        public float accelerationToApply;
        public float currentTimeValue;
        public float nextTimeValue;
        public float nextVelocityMagnitude;

        public float gas;

        private TankComponentManager tcm;

        private void Awake()
        {
            tcm = GetComponent<TankComponentManager>();
        }

        private void Update()
        {
            gas = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            tcm.forwardSpeed = Vector3.Dot(tcm.rb.transform.forward, tcm.rb.velocity);
            accelerationToApply = GetAccelerationFromVelocityCurve();

            Vector3 force = tcm.rb.transform.forward * Mathf.Abs(gas * GetAccelerationFromVelocityCurve());
            tcm.rb.AddForce(force, ForceMode.Acceleration);
        }

        private float GetAccelerationFromVelocityCurve()
        {
            float maxSpeed = velocityCurve.keys[velocityCurve.length - 1].value;
            if (tcm.forwardSpeed > maxSpeed) 
                return 0;

            float clampedSpeed = Mathf.Clamp(tcm.forwardSpeed, velocityCurve.keys[0].value, maxSpeed);

            currentTimeValue = GetTimeFromSpeed(velocityCurve, clampedSpeed);

            if (currentTimeValue != -1)
            {
                //float inputDir = input.accelInput > 0 ? 1 : -1;
                nextTimeValue = currentTimeValue + gas * Time.fixedDeltaTime;
                nextTimeValue = Mathf.Clamp(nextTimeValue, velocityCurve.keys[0].time,
                    velocityCurve.keys[velocityCurve.length - 1].time);

                nextVelocityMagnitude = velocityCurve.Evaluate(nextTimeValue);
                float accelMagnitude = (nextVelocityMagnitude - tcm.forwardSpeed) / (Time.fixedDeltaTime);

                return accelMagnitude;
            }

            return 0;
        }

        private float GetTimeFromSpeed(AnimationCurve velCurve, float curVel)
        {
            const int timeScale = 10000;

            int minTime = (int)(velCurve.keys[0].time * timeScale);
            int maxTime = (int)(velCurve.keys[velCurve.length - 1].time * timeScale);
            int numSteps = 0;

            while (minTime <= maxTime)
            {
                int mid = (minTime + maxTime) / 2;

                float scaledMid = (float)mid / timeScale;
                if (Mathf.Abs(velCurve.Evaluate(scaledMid) - curVel) <= 0.01f)
                {
                    //Debug.Log(string.Format("Final mid: {0}", mid));
                    return (float)mid / timeScale;
                }

                if (curVel < velCurve.Evaluate(scaledMid))
                {
                    maxTime = mid - 1;
                }
                else
                {
                    minTime = mid + 1;
                }

                //Debug.Log(string.Format("minTime: {0}   maxTime:{1}   mid: {2}   numSteps: {3}", minTime, maxTime, mid, numSteps));
                numSteps += 1;
            }

            //Debug.Log("[BinarySearchDisplay] Something went wrong with the BinarySearch - Returning -1");
            return -1;
        }
    }
}
