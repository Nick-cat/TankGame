using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SBC
{
    public class NavMeshAgentAI : MonoBehaviour
    {
        [SerializeField] Vector2 moveSpeedRange = new Vector2(40f, 60f);
        [SerializeField] float maxSpeed = 30f;
        [SerializeField] Vector2 turnSpeedRange = new Vector2(110f, 130f);
        [SerializeField] Vector2 roamingRange = new Vector2(50f, 100f);
        [SerializeField] Vector2 strafeRange = new Vector2(20f, 40f);
        [SerializeField] float respawnTime;
        [SerializeField] Transform turret;
        

        [Header("Materials")]
        [SerializeField] MeshRenderer gemMat;
        [SerializeField] Material idleGlow;
        [SerializeField] Material searchingGlow;
        [SerializeField] Material chaseGlow;

        private Vector3 target;
        private GameObject player;
        private NavMeshAgent agent;
        private Rigidbody rb;
        private EnemyWheelInfo wheels;
        private float moveSpeed;
        private float turnSpeed;
        private float roamingDistance;
        private float strafeDistance;
        private int rotateDirection;
        private bool searchingForPlayer;

        private void Start()
        {
            moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
            turnSpeed = Random.Range(turnSpeedRange.x, turnSpeedRange.y);
            roamingDistance = Random.Range(roamingRange.x, roamingRange.y);
            strafeDistance = Random.Range(strafeRange.x, strafeRange.y);

            //set random strafe direction
            bool dir = (Random.value < 0.5f);
            if (dir == true) rotateDirection = 1;
            else rotateDirection = -1;

            if (player == null) player = GameObject.FindGameObjectWithTag("Player");

            rb = GetComponent<Rigidbody>();
            wheels = GetComponent<EnemyWheelInfo>();

            agent = GetComponent<NavMeshAgent>();
            agent.speed = maxSpeed;
            agent.acceleration = moveSpeed;
            agent.angularSpeed = turnSpeed;
            
            //gets a random point roaming distance away, then finds the 
            //closest point on the navmesh to that point and sets target to it
            GetRandomNavPoint();

            //NavMesh Agent function to find a path to a given point on the navmesh
            agent.SetDestination(target);
        }

        private void FixedUpdate()
        {
            if (rb.isKinematic)
            {
                //sets the turret transform to point to the target
                LookAtTarget();
                //checks if player in within strafeDistance*6
                if (CanDetectPlayer())
                {
                    //checks if player in within strafeDistance*5
                    if (CanHuntPlayer())
                    {
                        Hunt();
                        return;
                    }

                    Search();
                    return;
                }

                //gets a new random point on the navmesh to move towards 
                //if it is close enough to the previous target location
                Roam(); 
            }
            else
            {
                agent.enabled = false;
                if (wheels.grounded)
                {
                    agent.enabled = true;
                    rb.isKinematic = true;
                }
            }
        }

        private bool CanDetectPlayer()
        {
            return CalculateDistance(player.transform.position) < strafeDistance * 6f;
        }

        private bool CanHuntPlayer()
        {
            return CalculateDistance(player.transform.position) < strafeDistance * 5f;
            //if (CalculateDistance(player.transform.position) < strafeDistance * 5f) return true;
            //return false;
        }

        private void Roam()
        {
            gemMat.material = idleGlow;
            if (agent.remainingDistance < strafeDistance)
            {
                GetRandomNavPoint();
                agent.SetDestination(target);
            }
        }

        private void Search()
        {
            gemMat.material = searchingGlow;
            if (!searchingForPlayer)
            {
                target = player.transform.position;
                searchingForPlayer = true;
            }
            agent.SetDestination(target);
        }

        private void Hunt()
        {
            if (searchingForPlayer) searchingForPlayer = false;
            gemMat.material = chaseGlow;
            if (CalculateDistance(player.transform.position) <= strafeDistance)
            {
                //makes the AI strafe around the player
                Strafe();
                return;
            }
            target = player.transform.position;
            agent.SetDestination(target);
        }

        private void Strafe()
        {
            Vector3 pathToTarget = target - transform.position;
            Vector3 dir = Vector3.Cross(pathToTarget, Vector3.up);
            agent.SetDestination(transform.position + (dir * rotateDirection));
        }

        private void LookAtTarget()
        {
            Quaternion angleToTarget = Quaternion.LookRotation(target - turret.position);
            turret.rotation = Quaternion.Lerp(turret.rotation, angleToTarget, 10f * Time.deltaTime);
        }

        /// <summary>
        /// Returns the distance between this transform and the specified point
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private float CalculateDistance(Vector3 point)
        {
            float distance = Vector3.Distance(transform.position, point);
            return distance;
        }

        /// <summary>
        /// gets a random point roaming distance away, then finds the 
        /// closest point on the navmesh to that point and sets target to it
        /// </summary>
        private void GetRandomNavPoint()
        {
            //gets a random point roaming distance away
            Vector3 randomDirection = new Vector3(RandomWithinRange(roamingDistance), 0, RandomWithinRange(roamingDistance));
            randomDirection = Vector3.ClampMagnitude(randomDirection, roamingDistance);
            Debug.DrawRay(transform.position, randomDirection, Color.green, 2f);
            randomDirection += transform.position;

            //finds the closest point on the navmesh to the random point
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamingDistance, 1);
            target = hit.position;
        }

        private float RandomWithinRange(float r)
        {
            return Random.Range(-r, r);
        }
    }
}
