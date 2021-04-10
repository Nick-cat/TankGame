using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TankController : MonoBehaviour
    {
        private TankComponentManager tcm;

        public float tankSpeed = 10f;
        [SerializeField] float boostSpeed = 60f;
        [Tooltip("MaxCapacity of Boost")]
        [SerializeField] float boostCapacity = 100f;
        public float BoostCapacity { get { return boostCapacity; } }
        public float rotateSpeed;
        public float brakeForce = 2f;
        public float maxVelocity;
        public AnimationCurve accelerationCurve;
        public float driftAngle;
        public float jumpForce;
        
        [Header("Gravity and Drag")]
        public float customGravity = 2f;
        public Vector3 centerOfMass;
        public float accelDrag = 0.1f;
        public float deccelDrag = 3f;
        private float groundDrag;
        public float angularDrag;
        public float airDrag = 0f;

        public float treadSpeed;
        
        [Header("Suspension")]
        public float suspensionHeight;
        public float springTravel;
        public float springStiffness;
        public float damperStiffness;
        public float wheelRadius;

        private Rigidbody rb;
        private bool canJump = true;

        private GameObject spawnPoint;

        //player inputs
        private float gas;
        private float rotate;
        private bool brake;
        private bool jump;
        private bool respawn;
        private bool boost;

        //acceleration variables
        [HideInInspector] public float acceleration;
        private float speedPercent;
        [HideInInspector] public float forwardForce;

        [HideInInspector] public float boostRemaining;
        private BoostEffect boostEffect;

        private void Start()
        {
            tcm = GetComponent<TankComponentManager>();
            rb = tcm.rb;
            rb.centerOfMass = centerOfMass;
            spawnPoint = GameObject.Find("SpawnPoint");
            boostRemaining = boostCapacity;
            boostEffect = GetComponent<BoostEffect>();
        }

        private void Update()
        {
            //get player inputs
            gas = Input.GetAxisRaw("Vertical");
            rotate = Input.GetAxis("Horizontal");
            brake = Input.GetButton("Brake");
            jump = Input.GetButton("Jump");
            respawn = Input.GetButton("Respawn");
            boost = Input.GetButton("Boost");
        }

        void FixedUpdate()
        {
            if (respawn)
            {
                Respawn();
                return;
            }

            //removed the need for this by increasing the gravity in the physics settings
            //custom gravity
            //rb.AddForce(Vector3.up * -customGravity * 100f);

            //calculates acceleration which is used by the suspension script
            Acceleration();
            groundDrag = gas != 0 ? accelDrag : deccelDrag;

            if (jump && canJump)
            {
                rb.AddForceAtPosition(transform.up * jumpForce, rb.position, ForceMode.Impulse);
                canJump = false;
            }

            if (tcm.isGrounded)
            {
                canJump = true;
                Boost();
                Rotate();
                Brake();
                return;
            }
            rb.drag = 0;
            rb.angularDrag = airDrag;
            canJump = false;
        }

        private void Brake()
        {
            rb.angularDrag = brake ? brakeForce : angularDrag;
            rb.drag = brake ? brakeForce : groundDrag;
        }

        private void Rotate()
        {
            //adjust rotatation depending on speed
            float rotateSpeedMod = Mathf.Clamp(1f - rb.velocity.magnitude / (maxVelocity * 2), 0.8f, 1f);

            float rotationTorque = rotate * rotateSpeedMod * rotateSpeed * Time.fixedDeltaTime;
            rb.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);
        }

        public void Respawn()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = spawnPoint.transform.position;
            transform.rotation = spawnPoint.transform.rotation;
            acceleration = 0f;
        }

        void Acceleration()
        {
            speedPercent = rb.velocity.magnitude / maxVelocity;
            //Debug.Log("speedPercent =" + speedPercent);
            acceleration = accelerationCurve.Evaluate(speedPercent);
            //Debug.Log("acceleration =" + acceleration);
            forwardForce = (acceleration * tankSpeed * gas);
        }

        private void Boost()
        {
            if (boostRemaining > boostCapacity) boostRemaining = boostCapacity;
            if (boost && boostRemaining != 0f)
            {
                boostRemaining -= 1f;
                boostEffect.Boost();
                rb.AddForce(transform.forward * boostSpeed * Time.fixedDeltaTime * 50f, ForceMode.Acceleration);
                return;
            }
            boostEffect.NoBoost();
        }
    }
}

