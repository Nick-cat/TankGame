using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class EnemyWheelInfo : MonoBehaviour
    {
        public Transform[] wheels;
        public float numberOfGroundedWheels;
        public bool grounded;
        [SerializeField] float groundedDistance;
        private LayerMask ground;

        private void Start()
        {
            ground = LayerMask.GetMask("Ground");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            numberOfGroundedWheels = 0;
            foreach (var wheel in wheels)
            {
                if (Physics.Raycast(wheel.position, -wheel.up, out RaycastHit Hit, groundedDistance, ground))
                {
                    Debug.DrawRay(wheel.position, -wheel.up * Hit.distance, Color.green);
                    numberOfGroundedWheels += 1;
                }
            }

            if (numberOfGroundedWheels == wheels.Length) grounded = true;
            else grounded = false;
        }
    }
}
