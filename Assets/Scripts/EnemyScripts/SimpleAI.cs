using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class SimpleAI : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] Vector3 centerOfMass;
        [Space]
        [ReadOnly] public float moveSpeed;
        [SerializeField] Vector2 moveSpeedRange = new Vector2(10f, 20f);
        public float maxSpeed = 30f;
        [ReadOnly] [SerializeField] float turnSpeed;
        [SerializeField] Vector2 turnSpeedRange = new Vector2(0.01f, 0.05f);
        [ReadOnly] [Tooltip("The distance from the player that the enemy will start to cirle")]
        public float strafeDistance;
        [Tooltip("Randomly sets strafe distance to a value between x and y")]
        [SerializeField] Vector2 strafeRange = new Vector2(10f, 20f);

        [Header("Suspension")]
        public float suspensionHeight;
        public float springTravel;
        public float springStiffness;
        public float damperStiffness;
        public float wheelRadius;

        [HideInInspector] public int rotateDirection;
        private Rigidbody rb;

        [SerializeField] bool drawDebug = false;

        [SerializeField] SimpleSuspension[] suspension;

        private void Start()
        {
            //set random movespeed, turnspeed, adn strafeDistance
            moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
            turnSpeed = Random.Range(turnSpeedRange.x, turnSpeedRange.y);
            strafeDistance = Random.Range(strafeRange.x, strafeRange.y);

            //set random rotation direction
            bool dir = (Random.value < 0.5f);
            if (dir == true) rotateDirection = 1;
            else rotateDirection = -1;

            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = centerOfMass;
        }

        void Update()
        {
            if (target == null)
            {
                //find the player character
                target = GameObject.FindWithTag("Player");
            }
            else AutoPilot();
        }

        void AutoPilot()
        {
            //Rotation
            transform.Rotate(0, CalculateAngle() * turnSpeed, 0);
            //suspension and movement handled in SimpleSuspension script on each wheel
            foreach (SimpleSuspension wheel in suspension) wheel.Suspension();
        }

        public float CalculateDistance()
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return distance;
        }

        float CalculateAngle()
        {
            //calculate direction to target
            Vector3 pathToTarget = target.transform.position - transform.position;

            //get and set rotation to target
            float angle = (Vector3.SignedAngle(transform.forward, pathToTarget, transform.up));

            //draws forward vector and pathToTarget vector
            if (drawDebug == true)
            {
                Debug.DrawRay(transform.position, pathToTarget, Color.red, 1);
                Debug.DrawRay(transform.position, transform.forward * 10, Color.green, 1);
            }
            return angle;
        }
        void Movement()
        {
            if (CalculateDistance() > strafeDistance)
            {
                if (rb.velocity.magnitude < maxSpeed)
                {
                    Vector3 forwards = transform.forward * moveSpeed * Time.deltaTime * 100f;
                    rb.AddForce(forwards, ForceMode.Acceleration);
                }
            }
            else
            {
                //makes the enemy strafe around the player when within strafeDistance
                Vector3 strafe = transform.right * moveSpeed * Time.deltaTime * 50f * rotateDirection;
                rb.AddForce(strafe, ForceMode.Acceleration);
            }
        }
    }
}
