using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class SimpleAI : MonoBehaviour
    {
        [SerializeField] GameObject target;
        [SerializeField] Vector3 centerOfMass;
        [SerializeField] Vector2 moveSpeedRange;
        public float maxSpeed = 30f;
        [SerializeField] Vector2 turnSpeedRange;
        [SerializeField] Vector2 strafeRange;
        [SerializeField] TankRound ammo;
        [SerializeField] GameObject cannonTip;
        [SerializeField] FireSoundHandler fireSoundHandler;
        [Header("Suspension")]
        public float suspensionHeight;
        public float springTravel;
        public float springStiffness;
        public float damperStiffness;
        public float wheelRadius;
        [SerializeField] SimpleSuspension[] suspension;
        [SerializeField] bool drawDebug = false;

        [HideInInspector] public float moveSpeed;
        [HideInInspector] public float strafeDistance;
        private float angle;
        private float turnSpeed;
        [HideInInspector] public int rotateDirection;
        private Rigidbody rb;

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

        void FixedUpdate()
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
            CalculateAngle();
            //Rotation
            transform.Rotate(0, angle * turnSpeed, 0);
            //suspension and movement handled in SimpleSuspension script on each wheel
            foreach (SimpleSuspension wheel in suspension) wheel.Suspension();
            
            if (Input.GetKeyDown("t"))
            {
                TankRound round = Instantiate(ammo);
                round.Shoot(cannonTip.transform.position, cannonTip.transform.forward, ammo.projectileSpeed);
                rb.AddForceAtPosition(-cannonTip.transform.forward * ammo.projectileForce, cannonTip.transform.position);
                if (fireSoundHandler != null) fireSoundHandler.Fire();
            }
        }

        public float CalculateDistance()
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            return distance;
        }

        void CalculateAngle()
        {
            //calculate direction to target
            Vector3 pathToTarget = target.transform.position - transform.position;

            //get rotation to target
            angle = (Vector3.SignedAngle(transform.forward, pathToTarget, transform.up));
            //Debug.Log(angle);
            //draws forward vector and pathToTarget vector
            if (drawDebug == true)
            {
                Debug.DrawRay(transform.position, pathToTarget, Color.red, 1);
                Debug.DrawRay(transform.position, transform.forward * 10, Color.green, 1);
            }
        }
    }
}
