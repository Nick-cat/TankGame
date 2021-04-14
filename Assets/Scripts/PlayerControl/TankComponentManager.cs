using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TankComponentManager : MonoBehaviour
    {
        public Transform[] wheelsLeft;
        public Transform[] wheelsRight;
        [HideInInspector] public List<Transform> wheels;
        public bool isGrounded;
        public Rigidbody rb;
        public int numberOfGroundedWheels;
        public Vector3 averageColliderSurfaceNormal;

        public float forwardSpeed;
        public float sideSpeed;

        public bool isGroundedLeft;
        public int numberOfGroundedWheelsLeft;
        public bool isGroundedRight;
        public int numberOfGroundedWheelsRight;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            wheels.AddRange(wheelsLeft);
            wheels.AddRange(wheelsRight);
        }

        private void FixedUpdate()
        {
            numberOfGroundedWheels = numberOfGroundedWheelsLeft + numberOfGroundedWheelsRight;
            if (numberOfGroundedWheelsLeft <= 0) isGroundedLeft = false;
            if (numberOfGroundedWheelsRight <= 0) isGroundedRight = false;
            if (!isGroundedLeft && !isGroundedRight) isGrounded = false;
            else isGrounded = true;
        }
    }
}
