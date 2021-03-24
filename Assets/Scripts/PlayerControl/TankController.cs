using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class TankController : MonoBehaviour
    {
        public float tankSpeed = 10f;
        [SerializeField] float boostSpeed = 60f;
        public float boostMax = 100f;
        [SerializeField] float accelerationTime = 0.05f;
        public float rotateSpeed;
        public float rotateSmoothing = 0.5f;
        public float brakeForce = 2f;
        public float maxVelocity;
        public float driftAngle;
        public float jumpForce;
        
        public float distanceToGround = .5f;
        public Transform rayPoint;

        [Header("Gravity and Drag")]
        public float customGravity = 2f;
        public Vector3 centerOfMass;
        public float groundDrag = 3f;
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

        private Vector3 rotateAmount;
        private Vector3 rotateChangeVelocity;

        //player inputs
        private float gas;
        private float rotate;
        private bool brake;
        private bool jump;
        private bool respawn;
        private bool boost;

        //acceleration variables
        private float oldAcceleration = 0f;
        private float forwards = 0f;
        [HideInInspector] public float acceleration;
        [HideInInspector] public float forwardForce;
        private float velocityChange = 0f;

        [HideInInspector] public float boostRemaining;
        private BoostEffect boostEffect;

        private void Start()
        {
            rb = GetComponent <Rigidbody>();
            rb.centerOfMass = centerOfMass;
            spawnPoint = GameObject.Find("SpawnPoint");
            boostRemaining = boostMax;
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
            //custom gravity
            rb.AddForce(Vector3.up * -customGravity * 100f);

            //calculates acceleration which is used by the suspension script
            Acceleration();

            if (jump && canJump)
            {
                rb.AddForce(transform.up * jumpForce * 100f);
                canJump = false;
            }

            if (IsGrounded())
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
            if (brake)
            {
                forwards = 0f;
                rb.angularDrag = brakeForce;
                rb.drag = brakeForce;
                return;
                //Experimental Drift, comment this out if tank is too buggy
                //if (rb.velocity.sqrMagnitude > Mathf.Sqrt(maxVelocity))
                //{
                //    rb.angularVelocity = new Vector3(0f, rotate * (rotateSpeed / 2f) * Time.deltaTime, 0f);
                //}
            }
            forwards = gas;
            rb.angularDrag = groundDrag / 2;
            rb.drag = groundDrag;
        }

        private void Rotate()
        {
            //turn the tank correctly when upside down
            int upsidedown = 1;
            if (Vector3.Dot(transform.up, Vector3.down) > 0) upsidedown = -1;

            //adjust rotatation depending on speed
            float rotateSpeedMod = Mathf.Clamp(1f - rb.velocity.magnitude / (maxVelocity * 2), 0.8f, 1f);

            //smooths tank rotation value
            Vector3 rotateOld = rotateAmount;
            Vector3 rotateNew = new Vector3(0f, rotate * rotateSpeed * rotateSpeedMod * Time.deltaTime * upsidedown, 0f);
            rotateAmount = Vector3.SmoothDamp(rotateOld, rotateNew, ref rotateChangeVelocity, rotateSmoothing);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotateAmount);
        }

        bool IsGrounded()
        {
            //cast a ray from center of body to down direction, check if anything intersects the bottom of the body.
            LayerMask ground = LayerMask.GetMask("Ground");

            bool val = Physics.Raycast(rayPoint.position, -transform.up, distanceToGround, ground);
            return val;
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
            oldAcceleration = acceleration;
            acceleration = Mathf.SmoothDamp(oldAcceleration, forwards, ref velocityChange, accelerationTime);
            forwardForce = acceleration * tankSpeed * 100f;
        }
        private void Boost()
        {
            if (boostRemaining > boostMax) boostRemaining = boostMax;
            if (boost && boostRemaining != 0f)
            {
                boostRemaining -= 1f;
                boostEffect.Boost();
                rb.AddForce(transform.forward * boostSpeed * 100f * Time.deltaTime, ForceMode.Acceleration);
                return;
            }
            boostEffect.NoBoost();
        }
    }
}

