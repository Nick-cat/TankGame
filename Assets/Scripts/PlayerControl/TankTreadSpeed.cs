using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TankTreadSpeed : MonoBehaviour
    {
        [SerializeField] Transform leftTread;
        [SerializeField] Transform rightTread;
        private bool drift;
        private float rotate;
        private float driftDirection;
        private TankController tank;
        private TankComponentManager tcm;
        // Start is called before the first frame update
        void Start()
        {
            tcm = GetComponent<TankComponentManager>();
            tank = GetComponent<TankController>();
        }

        private void Update()
        {
            drift = Input.GetButton("Drift");
            rotate = Input.GetAxis("Horizontal");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //calulates vector to apply force along for drift
            if (drift) driftDirection = tank.driftAngle * rotate;
            else driftDirection = 0;

            Vector3 moveDirection = Quaternion.AngleAxis(driftDirection, Vector3.up) * transform.forward;

            //adds forward force to the tank at the treads
            if (tcm.isGroundedLeft)
                tcm.rb.AddForceAtPosition(moveDirection * tank.forwardForce / 2, leftTread.position, ForceMode.VelocityChange);
            if (tcm.isGroundedRight)
                tcm.rb.AddForceAtPosition(moveDirection * tank.forwardForce / 2, rightTread.position, ForceMode.VelocityChange);
        }
    }
}
