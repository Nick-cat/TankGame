using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField] Vector2 moveSpeedRange;
    [SerializeField] float maxSpeed = 30f;
    [SerializeField] Vector2 turnSpeedRange;
    [SerializeField] Vector2 roamingRange;
    [SerializeField] Vector2 strafeRange;
    [SerializeField] Transform turret;

    [Header("Materials")]
    [SerializeField] MeshRenderer gemMat;
    [SerializeField] Material idleGlow;
    [SerializeField] Material searchingGlow;
    [SerializeField] Material chaseGlow;

    private Vector3 target;
    private GameObject player;
    private NavMeshAgent agent;
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

        agent = GetComponent<NavMeshAgent>();
        agent.speed = maxSpeed;
        agent.acceleration = moveSpeed;
        agent.angularSpeed = turnSpeed;
        GetRandomNavPoint();
        agent.SetDestination(target);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        LookAtTarget();
        if(CanDetectPlayer())
        {
            if (CanHuntPlayer())
            {
                Debug.Log("chasing");
                Hunt();
                return;
            }

            Debug.Log("searching");
            Search();
            return;
        }
        Debug.Log("roaming");
        //gets a new random point on the navmesh to move towards if it is close enough to the previous target location
        Roam();
    }

    private bool CanHuntPlayer()
    {
        return CalculateDistance(player.transform.position) < strafeDistance * 5f;
        //if (CalculateDistance(player.transform.position) < strafeDistance * 5f) return true;
        //return false;
    }

    private bool CanDetectPlayer()
    {
        return CalculateDistance(player.transform.position) < strafeDistance * 6f;
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

    private float CalculateDistance(Vector3 point)
    {
        float distance = Vector3.Distance(transform.position, point);
        return distance;
    }

    private void GetRandomNavPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamingDistance;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, roamingDistance, 1);
        target = hit.position;
    }
}
