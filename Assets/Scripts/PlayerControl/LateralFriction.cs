using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class LateralFriction : MonoBehaviour
    {
        [SerializeField] AnimationCurve slideFrictionCurve;
        //determines how much lateral friction is applied when drifiting and not drifting
        [SerializeField] [Range(0, 1)] float baseTireStickiness; 
        [SerializeField] [Range(0, 1)] float driftTireStickiness; 

        private float currentTireStickiness;
        private float slidingFrictionRatio;
        private float slidingFrictionForceAmount;

        private TankComponentManager tcm;

        private void Awake()
        {
            tcm = GetComponent<TankComponentManager>();
        }

        private void FixedUpdate()
        {
            tcm.forwardSpeed = Vector3.Dot(tcm.rb.transform.forward, tcm.rb.velocity);
            tcm.sideSpeed = (Vector3.Dot(tcm.rb.transform.right, tcm.rb.velocity));
            CalculateLateralFriction();
            ApplyLateralFriction();
        }

        private void CalculateLateralFriction()
        {
            float slideFrictionRatio = 0;

            if (Mathf.Abs(tcm.sideSpeed + tcm.forwardSpeed) > 0.01f)
                slideFrictionRatio = Mathf.Clamp01(Mathf.Abs(tcm.sideSpeed) / (Mathf.Abs(tcm.sideSpeed) + tcm.forwardSpeed));

            slidingFrictionRatio = slideFrictionCurve.Evaluate(slideFrictionRatio);

            slidingFrictionForceAmount = slidingFrictionRatio * -tcm.sideSpeed * currentTireStickiness;
        }

        private void ApplyLateralFriction()
        {
            if (!tcm.isGrounded) return;

            tcm.rb.AddForce(slidingFrictionForceAmount * tcm.rb.transform.right, ForceMode.Impulse);
            currentTireStickiness = tcm.isDrifting ? driftTireStickiness : baseTireStickiness;
        }
    }
}
